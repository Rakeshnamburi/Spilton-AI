using System.Collections.Concurrent;
using Npgsql;
using Microsoft.EntityFrameworkCore;
using Spilton.Api.Data;
namespace Spilton.Api.Chat;

// Session advisory lock: excludes competing sends/renames/deletes across API processes.
public sealed class ConversationLock : IAsyncDisposable
{
    private readonly NpgsqlConnection connection;
    private ConversationLock(NpgsqlConnection connection) => this.connection = connection;
    public static async Task<ConversationLock?> TryAcquire(AppDbContext db, Guid id, CancellationToken ct)
    {
        // Clone retains authentication when EF uses an NpgsqlDataSource (e.g. UseVector).
        // GetConnectionString can omit its password after a connection has opened.
        var connection = (NpgsqlConnection)((ICloneable)db.Database.GetDbConnection()).Clone();
        try {
            await connection.OpenAsync(ct);
            await using var command = new NpgsqlCommand("SELECT pg_try_advisory_lock(@key)", connection);
            command.Parameters.AddWithValue("key", BitConverter.ToInt64(id.ToByteArray(), 0));
            if ((bool)(await command.ExecuteScalarAsync(ct))!) return new(connection);
            await connection.DisposeAsync(); return null;
        } catch { await connection.DisposeAsync(); throw; }
    }
    public async ValueTask DisposeAsync()
    {
        try { await using var command = new NpgsqlCommand("SELECT pg_advisory_unlock_all()", connection); await command.ExecuteNonQueryAsync(); }
        finally { await connection.DisposeAsync(); }
    }
}
public sealed class GenerationControl
{
    private readonly ConcurrentDictionary<Guid, CancellationTokenSource> active = new();
    public void Add(Guid conversation, CancellationTokenSource source) => active[conversation] = source;
    public void Remove(Guid conversation) => active.TryRemove(conversation, out _);
    public bool Stop(Guid conversation)
    {
        if (!active.TryGetValue(conversation, out var source)) return false;
        try { source.Cancel(); return true; } catch (ObjectDisposedException) { return false; }
    }
}
// A process crash releases advisory locks; persisted unfinished messages become retryable.
public sealed class GenerationRecovery(IServiceScopeFactory scopes, ILogger<GenerationRecovery> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try {
                using var scope = scopes.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var cutoff = DateTimeOffset.UtcNow.AddMinutes(-3);
                await db.Messages.Where(m => m.Status == "generating" && m.CreatedAt < cutoff)
                    .ExecuteUpdateAsync(s => s.SetProperty(m => m.Status, "failed"), stoppingToken);
            } catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested) { break; }
            catch (Exception ex) { logger.LogWarning("Generation recovery waiting for database/migration. ErrorCategory={ErrorCategory} ErrorType={ErrorType}", "database", ex.GetType().Name); }
            try { await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken); }
            catch (OperationCanceledException) { break; }
        }
    }
}

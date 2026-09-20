using System.Text;
using System.Net;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Spilton.Api.Auth;
using Spilton.Api.Data;
using Spilton.Api.Infrastructure;
using Spilton.Api.Chat;
using Spilton.Api.Documents;
using Pgvector.EntityFrameworkCore;
using Spilton.Api.Preparation;
using Spilton.Api.Government;
using Spilton.Api.Web;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddJsonConsole();
var jwt = builder.Configuration.GetSection("Jwt").Get<JwtSettings>() ?? new();
if (Encoding.UTF8.GetByteCount(jwt.Secret) < 32 || jwt.Secret.Contains("GENERATE", StringComparison.OrdinalIgnoreCase))
    throw new InvalidOperationException("Jwt:Secret must be configured with at least 32 random bytes. See README.");
if (string.IsNullOrWhiteSpace(jwt.Issuer) || string.IsNullOrWhiteSpace(jwt.Audience) || jwt.ExpiryMinutes is < 1 or > 60)
    throw new InvalidOperationException("JWT issuer, audience and expiry (1–60 minutes) are required.");
var connection = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("ConnectionStrings:DefaultConnection is required. See README.");
builder.Services.AddSingleton(jwt);
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connection,o=>o.UseVector()));
var rag = builder.Configuration.GetSection("Rag").Get<RagSettings>() ?? new();
builder.Services.AddSingleton(rag);
builder.Services.AddSingleton<MiniLmTokenizer>();
builder.Services.AddSingleton<IEmbeddingProvider,LocalEmbeddingProvider>();
builder.Services.AddSingleton<IFileStorage,LocalFileStorage>();
builder.Services.AddSingleton<DocumentExtractor>();builder.Services.AddSingleton<DocumentChunker>();
builder.Services.AddScoped<RetrievalService>();builder.Services.AddScoped<RagContextBuilder>();
builder.Services.AddScoped<ScopeGuard>();builder.Services.AddScoped<PreparationService>();builder.Services.AddScoped<PersonalizedContext>();
builder.Services.AddSingleton(builder.Configuration.GetSection("Government").Get<GovernmentSettings>()??new());builder.Services.AddScoped<SourcePolicy>();
builder.Services.AddScoped<Spilton.Api.Government.IResearchSourceProvider,Spilton.Api.Government.ManualResearchSources>();
builder.Services.AddSingleton(TimeProvider.System);builder.Services.AddScoped<Spilton.Api.Mocks.AttemptService>();builder.Services.AddHostedService<Spilton.Api.Mocks.AttemptExpiryWorker>();
builder.Services.AddHostedService<IngestionWorker>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.Configure<PasswordHasherOptions>(options => options.IterationCount = 210_000);
builder.Services.AddSingleton<LoginTimingGuard>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<SessionService>();
var emailSettings = builder.Configuration.GetSection("Email").Get<EmailSettings>() ?? new();
builder.Services.AddSingleton(emailSettings);
builder.Services.AddSingleton<IAccountEmailSender, SmtpAccountEmailSender>();
builder.Services.AddSingleton<Spilton.Api.Security.IResourceBudgetStore, Spilton.Api.Security.LocalResourceBudgetStore>();
var models = builder.Configuration.GetSection("Models").Get<ModelSettings>() ?? new();
builder.Services.AddSingleton(models);
builder.Services.AddHttpClient("models", client => client.Timeout = Timeout.InfiniteTimeSpan)
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler { AllowAutoRedirect = false });
builder.Services.AddScoped<ModelProviderFactory>();
builder.Services.AddScoped<IModelProviderResolver>(s => s.GetRequiredService<ModelProviderFactory>());
builder.Services.AddSingleton<IProviderHealth,ProviderHealth>();
builder.Services.AddSingleton<IMultimodalProvider,UnavailableMultimodalProvider>();
builder.Services.AddSingleton<Spilton.Api.Coding.CodingWorkspacePolicy>();
builder.Services.AddSingleton<Spilton.Api.Coding.ICodingWorkspace,Spilton.Api.Coding.UnavailableCodingWorkspace>();
builder.Services.AddSingleton<Spilton.Api.Coding.ICodeExecutionService,Spilton.Api.Coding.UnavailableCodeExecutionService>();
builder.Services.AddSingleton<Spilton.Api.Security.ISecretProtector,Spilton.Api.Security.SecretProtector>();
builder.Services.AddSingleton<Spilton.Api.Security.IUploadScanner,Spilton.Api.Security.UnconfiguredUploadScanner>();
builder.Services.AddSingleton<Spilton.Api.Security.IDistributedRateLimitStore,Spilton.Api.Security.LocalRateLimitStore>();
builder.Services.AddSingleton<ContextBuilder>();
builder.Services.AddSingleton<CapabilityRouter>();
builder.Services.AddSingleton<ConversationCompressor>();
builder.Services.AddScoped<CapabilityModelRouter>();
var web=builder.Configuration.GetSection("Web").Get<WebSettings>()??new();builder.Services.AddSingleton(web);
builder.Services.AddHttpClient("web-search",c=>c.Timeout=Timeout.InfiniteTimeSpan).ConfigurePrimaryHttpMessageHandler(()=>new HttpClientHandler{AllowAutoRedirect=false,AutomaticDecompression=DecompressionMethods.None});
builder.Services.AddHttpClient("web-reader",c=>c.Timeout=Timeout.InfiniteTimeSpan).ConfigurePrimaryHttpMessageHandler(()=>new SocketsHttpHandler{AllowAutoRedirect=false,AutomaticDecompression=DecompressionMethods.None,ConnectCallback=async(ctx,ct)=>{var addresses=await Dns.GetHostAddressesAsync(ctx.DnsEndPoint.Host,ct);var address=addresses.FirstOrDefault(a=>!SafeWebPageReader.IsPrivate(a))??throw new HttpRequestException("Public address required.");var socket=new System.Net.Sockets.Socket(address.AddressFamily,System.Net.Sockets.SocketType.Stream,System.Net.Sockets.ProtocolType.Tcp);try{await socket.ConnectAsync(new IPEndPoint(address,ctx.DnsEndPoint.Port),ct);return new System.Net.Sockets.NetworkStream(socket,ownsSocket:true);}catch{socket.Dispose();throw;}}});
builder.Services.AddSingleton<IWebSearchProvider>(s=>new TavilyWebSearchProvider(web,s.GetRequiredService<IHttpClientFactory>().CreateClient("web-search"),s.GetRequiredService<ILogger<TavilyWebSearchProvider>>()));
builder.Services.AddSingleton<IWebPageReader>(s=>new SafeWebPageReader(web,s.GetRequiredService<IHttpClientFactory>().CreateClient("web-reader"),s.GetRequiredService<ILogger<SafeWebPageReader>>()));builder.Services.AddSingleton<WebResearchService>();
builder.Services.AddSingleton<ResearchPlanner>();builder.Services.AddSingleton<ResearchSourceSelector>();builder.Services.AddSingleton<ResearchOrchestrator>();
builder.Services.AddSingleton<Spilton.Api.Tools.CalculatorTool>();
builder.Services.AddSingleton<Spilton.Api.Tools.DateTimeTool>();
builder.Services.AddSingleton<WebSearchTool>();builder.Services.AddSingleton<WebPageReaderTool>();builder.Services.AddSingleton<ResearchTool>();
builder.Services.AddSingleton<Spilton.Api.Tools.ISpiltonTool>(s=>s.GetRequiredService<Spilton.Api.Tools.CalculatorTool>());
builder.Services.AddSingleton<Spilton.Api.Tools.ISpiltonTool>(s=>s.GetRequiredService<Spilton.Api.Tools.DateTimeTool>());
builder.Services.AddSingleton<Spilton.Api.Tools.ISpiltonTool>(s=>s.GetRequiredService<WebSearchTool>());builder.Services.AddSingleton<Spilton.Api.Tools.ISpiltonTool>(s=>s.GetRequiredService<WebPageReaderTool>());
builder.Services.AddSingleton<Spilton.Api.Tools.ISpiltonTool>(s=>s.GetRequiredService<ResearchTool>());
builder.Services.AddSingleton<Spilton.Api.Tools.ToolRegistry>();
builder.Services.AddSingleton<Spilton.Api.Agents.Planner>();
builder.Services.AddSingleton(builder.Configuration.GetSection("Agent").Get<Spilton.Api.Agents.AgentSettings>()??new());
builder.Services.AddSingleton<Spilton.Api.Agents.AgentVerifier>();
builder.Services.AddScoped<Spilton.Api.Agents.AgentOrchestrator>();
builder.Services.AddSingleton<GenerationControl>();
builder.Services.AddHostedService<GenerationRecovery>();
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ApiExceptionHandler>();
var origins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
if (origins.Length == 0 || origins.Contains("*")) throw new InvalidOperationException("Configure explicit CORS origins.");
builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
    policy.WithOrigins(origins).WithMethods("GET", "POST", "PATCH", "DELETE").WithHeaders("Content-Type", "Authorization")));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.MapInboundClaims = false;
    options.TokenValidationParameters = new()
    {
        ValidateIssuer = true, ValidIssuer = jwt.Issuer,
        ValidateAudience = true, ValidAudience = jwt.Audience,
        ValidateLifetime = true, ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Secret)),
        ValidAlgorithms = [SecurityAlgorithms.HmacSha256],
        ClockSkew = TimeSpan.FromSeconds(10), NameClaimType = "name", RoleClaimType = "role"
    };
    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = async context =>
        {
            if (!Guid.TryParse(context.Principal?.FindFirst("sub")?.Value, out var id)) { context.Fail("Invalid user."); return; }
            var db = context.HttpContext.RequestServices.GetRequiredService<AppDbContext>();
            if (!await db.Users.AnyAsync(u => u.Id == id && u.IsActive, context.HttpContext.RequestAborted))
                context.Fail("Inactive or missing user.");
            var sessionClaim = context.Principal?.FindFirst("sid")?.Value;
            if (sessionClaim is not null && (!Guid.TryParse(sessionClaim, out var sid) ||
                !await db.Set<AuthSession>().AnyAsync(s => s.Id == sid && s.UserId == id && s.RevokedAt == null && s.ExpiresAt > DateTimeOffset.UtcNow, context.HttpContext.RequestAborted)))
                context.Fail("Session expired or revoked.");
            if (sessionClaim is null && !builder.Environment.IsDevelopment()) context.Fail("Session required.");
        }
    };
});
builder.Services.AddAuthorization();
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = 429;
    options.AddPolicy("mockAnswers",context=>RateLimitPartition.GetFixedWindowLimiter(context.User.FindFirst("sub")?.Value??"anonymous",_=>new FixedWindowRateLimiterOptions{PermitLimit=240,Window=TimeSpan.FromMinutes(1),QueueLimit=0}));
    options.AddPolicy("library",context=>RateLimitPartition.GetFixedWindowLimiter(context.User.FindFirst("sub")?.Value??"anonymous",_=>new FixedWindowRateLimiterOptions{PermitLimit=20,Window=TimeSpan.FromMinutes(1),QueueLimit=0}));
    options.AddPolicy("documents", context=>RateLimitPartition.GetFixedWindowLimiter(context.User.FindFirst("sub")?.Value??"anonymous",
        _=>new FixedWindowRateLimiterOptions{PermitLimit=10,Window=TimeSpan.FromMinutes(1),QueueLimit=0}));
    options.AddPolicy("chat", context => RateLimitPartition.GetFixedWindowLimiter(
        context.User.FindFirst("sub")?.Value ?? "anonymous",
        _ => new FixedWindowRateLimiterOptions { PermitLimit = 30, Window = TimeSpan.FromMinutes(1), QueueLimit = 0 }));
    options.AddPolicy("auth", context => RateLimitPartition.GetFixedWindowLimiter(
        context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
        _ => new FixedWindowRateLimiterOptions { PermitLimit = 20, Window = TimeSpan.FromMinutes(1), QueueLimit = 0 }));
});
var app = builder.Build();
app.UseExceptionHandler();
app.UseStatusCodePages();
app.UseMiddleware<SecurityHeadersMiddleware>();
app.UseMiddleware<RequestAuditMiddleware>();
if (!app.Environment.IsDevelopment()) { app.UseHsts(); app.UseHttpsRedirection(); }
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<Spilton.Api.Security.ResourceBudgetMiddleware>();
app.UseRateLimiter();
if (app.Environment.IsDevelopment()) app.MapOpenApi();
app.MapControllers();
app.MapGet("/api/health", async (AppDbContext db, IModelProviderResolver providers, IProviderHealth providerHealth, CancellationToken ct) =>
{
    try
    {
        if (!await db.Database.CanConnectAsync(ct))
            return Results.Json(new { status = "degraded", database = "unavailable", errorCategory = "database" }, statusCode: 503);

        var migrationsPending = (await db.Database.GetPendingMigrationsAsync(ct)).Any();
        if (migrationsPending)
            return Results.Json(new { status = "degraded", database = "connected", migrations = "pending", provider = "not_checked" }, statusCode: 503);

        var defaultProvider = providers.Available.FirstOrDefault(model => model.Id == providers.DefaultId);
        var provider = defaultProvider is null ? "unconfigured" : providerHealth.Status(defaultProvider.Id).ToLowerInvariant();
        return provider == "healthy"
            ? Results.Ok(new { status = "healthy", database = "connected", migrations = "current", provider })
            : Results.Ok(new { status = "degraded", database = "connected", migrations = "current", provider });
    }
    catch (OperationCanceledException) when (ct.IsCancellationRequested)
    {
        return Results.Json(new { status = "degraded", database = "unknown", errorCategory = "canceled" }, statusCode: 503);
    }
    catch (Exception ex)
    {
        return Results.Json(new { status = "degraded", database = "unavailable", errorCategory = "database", errorType = ex.GetType().Name }, statusCode: 503);
    }
}).WithName("Health");
app.Run();
public partial class Program { }

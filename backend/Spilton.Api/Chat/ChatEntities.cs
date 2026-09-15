using Spilton.Api.Data;
using Spilton.Api.Documents;
using System.Text.Json;
namespace Spilton.Api.Chat;

public sealed class Conversation
{
    public Guid? SpaceId {get;set;}public Spilton.Api.Preparation.Space? Space {get;set;}
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public string Title { get; set; } = "New conversation";
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    public List<Message> Messages { get; set; } = [];
}
public sealed class Message
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ConversationId { get; set; }
    public Conversation Conversation { get; set; } = null!;
    public int Sequence { get; set; }
    public string Role { get; set; } = "USER";
    public string Content { get; set; } = "";
    public string? ModelProvider { get; set; }
    public string? ModelName { get; set; }
    public string Status { get; set; } = "completed";
    public Guid? ReplyToId { get; set; }
    public bool IsSuperseded { get; set; }
    public Guid[] DocumentIds { get; set; } = [];
    public string CitationsJson { get; set; } = "[]";
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
public sealed record MessageDto(Guid Id, int Sequence, string Role, string Content, string? ModelProvider, string? ModelName, string Status, DateTimeOffset CreatedAt, Guid[] DocumentIds, Citation[] Citations)
{
    public static MessageDto From(Message m) => new(m.Id, m.Sequence, m.Role, m.Content, m.ModelProvider, m.ModelName, m.Status, m.CreatedAt,m.DocumentIds,JsonSerializer.Deserialize<Citation[]>(m.CitationsJson)??[]);
}

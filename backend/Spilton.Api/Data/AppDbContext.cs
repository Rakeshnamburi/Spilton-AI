using Microsoft.EntityFrameworkCore;
using Spilton.Api.Chat;
using Spilton.Api.Documents;
using Spilton.Api.Preparation;
using Spilton.Api.Government;
using Spilton.Api.Mocks;
namespace Spilton.Api.Data;

public sealed class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string NormalizedEmail { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    public bool IsActive { get; set; } = true;
    public ICollection<Role> Roles { get; set; } = new List<Role>();
}
public sealed class Role
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public ICollection<User> Users { get; set; } = new List<User>();
}
public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public static readonly Guid UserRoleId = Guid.Parse("7d34e4d3-0b27-4612-a3ed-563aa5c22ae7");
    public DbSet<User> Users => Set<User>();
    public DbSet<Question> Questions=>Set<Question>();public DbSet<QuestionOption> QuestionOptions=>Set<QuestionOption>();public DbSet<MockTest> MockTests=>Set<MockTest>();public DbSet<MockSection> MockSections=>Set<MockSection>();public DbSet<MockTestQuestion> MockTestQuestions=>Set<MockTestQuestion>();public DbSet<MockAttempt> MockAttempts=>Set<MockAttempt>();public DbSet<AttemptAnswer> AttemptAnswers=>Set<AttemptAnswer>();
    public DbSet<GovernmentResource> GovernmentResources=>Set<GovernmentResource>();public DbSet<NotificationField> NotificationFields=>Set<NotificationField>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Conversation> Conversations => Set<Conversation>();
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<Document> Documents => Set<Document>();
    public DbSet<DocumentChunk> DocumentChunks => Set<DocumentChunk>();
    public DbSet<Space> Spaces=>Set<Space>();public DbSet<Exam> Exams=>Set<Exam>();public DbSet<ExamStage> ExamStages=>Set<ExamStage>();public DbSet<Subject> Subjects=>Set<Subject>();public DbSet<Topic> Topics=>Set<Topic>();
    public DbSet<UserExamProfile> UserExamProfiles=>Set<UserExamProfile>();public DbSet<UserTopicProgress> UserTopicProgress=>Set<UserTopicProgress>();public DbSet<Goal> Goals=>Set<Goal>();public DbSet<Memory> Memories=>Set<Memory>();public DbSet<StudyPlan> StudyPlans=>Set<StudyPlan>();public DbSet<StudyPlanItem> StudyPlanItems=>Set<StudyPlanItem>();
    protected override void OnModelCreating(ModelBuilder model)
    {
        Spilton.Api.Auth.SessionModel.Configure(model);
        Spilton.Api.Auth.AccountRecoveryModel.Configure(model);
        PreparationModel.Configure(model);
        MockModel.Configure(model);
        var resources=model.Entity<GovernmentResource>();resources.HasOne(r=>r.User).WithMany().HasForeignKey(r=>r.UserId).OnDelete(DeleteBehavior.Cascade);resources.HasOne(r=>r.Space).WithMany().HasForeignKey(r=>r.SpaceId).OnDelete(DeleteBehavior.Restrict);resources.HasOne(r=>r.Exam).WithMany().HasForeignKey(r=>r.ExamId).OnDelete(DeleteBehavior.Restrict);resources.HasOne(r=>r.Document).WithMany().HasForeignKey(r=>r.DocumentId).OnDelete(DeleteBehavior.Cascade);resources.HasIndex(r=>new{r.UserId,r.SpaceId,r.Kind});model.Entity<NotificationField>().HasOne(f=>f.GovernmentResource).WithMany(r=>r.Fields).HasForeignKey(f=>f.GovernmentResourceId).OnDelete(DeleteBehavior.Cascade);
        model.Entity<Conversation>().HasOne(c=>c.Space).WithMany().HasForeignKey(c=>c.SpaceId).OnDelete(DeleteBehavior.Restrict);
        model.Entity<Document>().HasOne(c=>c.Space).WithMany().HasForeignKey(c=>c.SpaceId).OnDelete(DeleteBehavior.Restrict);
        model.HasPostgresExtension("vector");
        var documents=model.Entity<Document>();
        documents.HasOne(d=>d.User).WithMany().HasForeignKey(d=>d.UserId).OnDelete(DeleteBehavior.Cascade);
        documents.HasIndex(d=>new{d.UserId,d.CreatedAt});
        documents.Property(d=>d.OriginalName).HasMaxLength(200);
        documents.Property(d=>d.StoredName).HasMaxLength(40);documents.HasIndex(d=>d.StoredName).IsUnique();
        documents.Property(d=>d.ContentType).HasMaxLength(100);documents.Property(d=>d.Category).HasMaxLength(30);
        documents.Property(d=>d.Status).HasMaxLength(16);documents.Property(d=>d.Error).HasMaxLength(300);documents.Property(d=>d.EmbeddingModel).HasMaxLength(120);
        documents.ToTable(t=>t.HasCheckConstraint("CK_Document_Status","\"Status\" IN ('UPLOADED','PROCESSING','READY','FAILED','DELETING')"));
        var chunks=model.Entity<DocumentChunk>();chunks.HasOne(c=>c.Document).WithMany(d=>d.Chunks).HasForeignKey(c=>c.DocumentId).OnDelete(DeleteBehavior.Cascade);
        chunks.HasIndex(c=>new{c.DocumentId,c.ChunkIndex}).IsUnique();chunks.Property(c=>c.Content).HasMaxLength(6000);chunks.Property(c=>c.Section).HasMaxLength(200);
        chunks.Property(c=>c.Embedding).HasColumnType("vector(384)");
        var conversations = model.Entity<Conversation>();
        conversations.Property(c => c.Title).HasMaxLength(100).IsRequired();
        conversations.HasOne(c => c.User).WithMany().HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Cascade);
        conversations.HasIndex(c => new { c.UserId, c.UpdatedAt, c.Id });
        var messages = model.Entity<Message>();
        messages.Property(m=>m.DocumentIds).HasColumnType("uuid[]");
        messages.Property(m=>m.CitationsJson).HasColumnType("jsonb");
        messages.HasOne(m => m.Conversation).WithMany(c => c.Messages).HasForeignKey(m => m.ConversationId).OnDelete(DeleteBehavior.Cascade);
        messages.HasIndex(m => new { m.ConversationId, m.Sequence }).IsUnique();
        messages.Property(m => m.Content).HasMaxLength(32000).IsRequired();
        messages.Property(m => m.Role).HasMaxLength(16).IsRequired();
        messages.Property(m => m.Status).HasMaxLength(16).IsRequired();
        messages.Property(m => m.ModelProvider).HasMaxLength(80);
        messages.Property(m => m.ModelName).HasMaxLength(120);
        messages.ToTable(t => {
            t.HasCheckConstraint("CK_Message_Role", "\"Role\" IN ('USER', 'ASSISTANT', 'SYSTEM')");
            t.HasCheckConstraint("CK_Message_Status", "\"Status\" IN ('completed', 'generating', 'cancelled', 'failed')");
        });
        var users = model.Entity<User>();
        users.ToTable("Users");
        users.HasKey(u => u.Id);
        users.Property(u => u.Name).HasMaxLength(100).IsRequired();
        users.Property(u => u.Email).HasMaxLength(254).IsRequired();
        users.Property(u => u.NormalizedEmail).HasMaxLength(254).IsRequired();
        users.HasIndex(u => u.NormalizedEmail).IsUnique();
        users.Property(u => u.PasswordHash).HasMaxLength(512).IsRequired();
        users.HasMany(u => u.Roles).WithMany(r => r.Users).UsingEntity("UserRoles");
        var roles = model.Entity<Role>();
        roles.ToTable("Roles");
        roles.HasKey(r => r.Id);
        roles.Property(r => r.Name).HasMaxLength(50).IsRequired();
        roles.HasIndex(r => r.Name).IsUnique();
        roles.HasData(new Role { Id = UserRoleId, Name = "User" });
    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<User>().Where(e => e.State == EntityState.Modified))
            entry.Entity.UpdatedAt = DateTimeOffset.UtcNow;
        return base.SaveChangesAsync(cancellationToken);
    }
}

using System.ComponentModel.DataAnnotations;
using Spilton.Api.Data;
using Spilton.Api.Documents;
using Spilton.Api.Preparation;
namespace Spilton.Api.Government;
// Shared private-library metadata avoids duplicating ownership/source fields across three modules.
public sealed class GovernmentResource
{
 public Guid Id{get;set;}=Guid.NewGuid();public Guid UserId{get;set;}public User User{get;set;}=null!;
 public Guid? SpaceId{get;set;}public Space? Space{get;set;}public Guid? ExamId{get;set;}public Exam? Exam{get;set;}
 public Guid? DocumentId{get;set;}public Document? Document{get;set;}
 [MaxLength(24)]public string Kind{get;set;}="NOTIFICATION";
 [MaxLength(200)]public string Title{get;set;}="";[MaxLength(120)]public string Organization{get;set;}="";
 [MaxLength(100)]public string NotificationNumber{get;set;}="";
 [MaxLength(2000)]public string? SourceUrl{get;set;}
 [MaxLength(24)]public string SourceType{get;set;}="USER_UPLOADED";
 [MaxLength(40)]public string Verification{get;set;}="UNVERIFIED_CONTENT";
 [MaxLength(64)]public string? SourceHash{get;set;}
 public DateTimeOffset? VerifiedAt{get;set;}
 [MaxLength(24)]public string Status{get;set;}="REGISTERED";
 public DateOnly? PublishedDate{get;set;}public int? Year{get;set;}
 [MaxLength(80)]public string Stage{get;set;}="";[MaxLength(80)]public string Subject{get;set;}="";
 [MaxLength(50)]public string Language{get;set;}="";[MaxLength(100)]public string Shift{get;set;}="";
 [MaxLength(50)]public string Category{get;set;}="OTHER";
 [MaxLength(700)]public string Summary{get;set;}="";
 public bool IsSaved{get;set;}public DateTimeOffset CreatedAt{get;set;}=DateTimeOffset.UtcNow;public DateTimeOffset UpdatedAt{get;set;}=DateTimeOffset.UtcNow;
 public ICollection<NotificationField> Fields{get;set;}=[];
}
public sealed class NotificationField
{
 public Guid Id{get;set;}=Guid.NewGuid();public Guid GovernmentResourceId{get;set;}public GovernmentResource GovernmentResource{get;set;}=null!;
 [MaxLength(40)]public string Name{get;set;}="";[MaxLength(1800)]public string Quote{get;set;}="";
 public Guid ChunkId{get;set;}public int? Page{get;set;}[MaxLength(200)]public string? Section{get;set;}
 [MaxLength(30)]public string Confidence{get;set;}="EXCERPT_REVIEW_REQUIRED";
}
public sealed class GovernmentSettings{public string VerifiedSourcesPath{get;set;}="";}
public sealed record VerifiedSource(string Url,string Sha256,string Organization,DateTimeOffset VerifiedAt,string SourceType="OFFICIAL");

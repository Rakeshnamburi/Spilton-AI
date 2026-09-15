using System.ComponentModel.DataAnnotations;
using Spilton.Api.Data;
namespace Spilton.Api.Preparation;
public sealed class Space
{
    public Guid Id {get;set;}=Guid.NewGuid();public Guid UserId {get;set;} public User User {get;set;}=null!;
    [MaxLength(100)]public string Name {get;set;}="";[MaxLength(500)]public string Description {get;set;}="";
    [MaxLength(30)]public string Type {get;set;}="GENERAL";public bool IsArchived {get;set;}
    public DateTimeOffset CreatedAt {get;set;}=DateTimeOffset.UtcNow;public DateTimeOffset UpdatedAt {get;set;}=DateTimeOffset.UtcNow;
}
public sealed class Exam {public Guid Id {get;set;}[MaxLength(100)]public string Name {get;set;}="";[MaxLength(30)]public string Family {get;set;}="";}
public sealed class ExamStage {public Guid Id {get;set;}public Guid ExamId {get;set;}public Exam Exam {get;set;}=null!;[MaxLength(100)]public string Name {get;set;}="";}
public sealed class Subject {public Guid Id {get;set;}public Guid ExamStageId {get;set;}public ExamStage ExamStage {get;set;}=null!;[MaxLength(100)]public string Name {get;set;}="";}
public sealed class Topic {public Guid Id {get;set;}public Guid SubjectId {get;set;}public Subject Subject {get;set;}=null!;[MaxLength(100)]public string Name {get;set;}="";}
public sealed class UserExamProfile
{
    public Guid Id {get;set;}=Guid.NewGuid();public Guid UserId {get;set;}public User User {get;set;}=null!;public Guid? SpaceId {get;set;}public Space? Space {get;set;}
    public Guid ExamStageId {get;set;}public ExamStage ExamStage {get;set;}=null!;
    public DateOnly? ExpectedExamDate {get;set;}public DateOnly? StartDate {get;set;}public int DailyMinutes {get;set;}=60;
    [MaxLength(20)]public string Level {get;set;}="BEGINNER";[MaxLength(30)]public string PreferredLanguage {get;set;}="English";
    public int? TargetScore {get;set;}public DateTimeOffset UpdatedAt {get;set;}=DateTimeOffset.UtcNow;
}
public sealed class UserTopicProgress
{
    public Guid Id {get;set;}=Guid.NewGuid();public Guid UserExamProfileId {get;set;}public UserExamProfile UserExamProfile {get;set;}=null!;
    public Guid TopicId {get;set;}public Topic Topic {get;set;}=null!;[MaxLength(20)]public string Status {get;set;}="NOT_STARTED";
    public int QuestionsAttempted {get;set;}public int QuestionsCorrect {get;set;}public int? ConfidenceScore {get;set;}
    public DateTimeOffset? LastPracticedAt {get;set;}public DateTimeOffset UpdatedAt {get;set;}=DateTimeOffset.UtcNow;
}
public sealed class Goal
{
    public Guid Id {get;set;}=Guid.NewGuid();public Guid UserId {get;set;}public User User {get;set;}=null!;public Guid? SpaceId {get;set;}public Space? Space {get;set;}
    [MaxLength(160)]public string Title {get;set;}="";[MaxLength(600)]public string Description {get;set;}="";
    public DateOnly? TargetDate {get;set;}[MaxLength(20)]public string Status {get;set;}="ACTIVE";public int ProgressPercent {get;set;}
    public DateTimeOffset CreatedAt {get;set;}=DateTimeOffset.UtcNow;public DateTimeOffset UpdatedAt {get;set;}=DateTimeOffset.UtcNow;
}
public sealed class Memory
{
    public Guid Id {get;set;}=Guid.NewGuid();public Guid UserId {get;set;}public User User {get;set;}=null!;public Guid? SpaceId {get;set;}public Space? Space {get;set;}
    [MaxLength(30)]public string Type {get;set;}="Preference";[MaxLength(500)]public string Content {get;set;}="";[MaxLength(30)]public string Source {get;set;}="USER_APPROVED";
    public bool IsActive {get;set;}=true;public DateTimeOffset CreatedAt {get;set;}=DateTimeOffset.UtcNow;public DateTimeOffset UpdatedAt {get;set;}=DateTimeOffset.UtcNow;
}
public sealed class StudyPlan
{
    public Guid Id {get;set;}=Guid.NewGuid();public Guid UserId {get;set;}public User User {get;set;}=null!;public Guid? SpaceId {get;set;}public Space? Space {get;set;}
    public DateOnly StartDate {get;set;}public int Days {get;set;}public DateTimeOffset CreatedAt {get;set;}=DateTimeOffset.UtcNow;
    public List<StudyPlanItem> Items {get;set;}=[];
}
public sealed class StudyPlanItem
{
    public Guid Id {get;set;}=Guid.NewGuid();public Guid StudyPlanId {get;set;}public StudyPlan StudyPlan {get;set;}=null!;public Guid TopicId {get;set;}public Topic Topic {get;set;}=null!;
    public DateOnly Date {get;set;}public int Minutes {get;set;}[MaxLength(20)]public string Status {get;set;}="PLANNED";[MaxLength(300)]public string Reason {get;set;}="";
}

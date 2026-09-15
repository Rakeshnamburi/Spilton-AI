using System.ComponentModel.DataAnnotations;
using Spilton.Api.Data;
using Spilton.Api.Preparation;
using Spilton.Api.Government;
namespace Spilton.Api.Mocks;

public sealed class Question
{
 public Guid Id{get;set;}=Guid.NewGuid();public Guid? UserId{get;set;}public User? User{get;set;}public Guid? SpaceId{get;set;}public Space? Space{get;set;}
 public Guid TopicId{get;set;}public Topic Topic{get;set;}=null!;
 [MaxLength(3000)]public string Text{get;set;}="";[MaxLength(3000)]public string Explanation{get;set;}="";
 [MaxLength(12)]public string Difficulty{get;set;}="EASY";[MaxLength(30)]public string SourceType{get;set;}="MANUAL_DEVELOPMENT";
 public Guid? SourceResourceId{get;set;}public GovernmentResource? SourceResource{get;set;}
 public string SourceMetadataJson{get;set;}="{}";public int CorrectOptionIndex{get;set;}
 public List<QuestionOption> Options{get;set;}=[];
}
public sealed class QuestionOption
{
 public Guid Id{get;set;}=Guid.NewGuid();public Guid QuestionId{get;set;}public Question Question{get;set;}=null!;public int Index{get;set;}
 [MaxLength(1000)]public string Text{get;set;}="";
}
public sealed class MockTest
{
 public Guid Id{get;set;}=Guid.NewGuid();public Guid? UserId{get;set;}public User? User{get;set;}public Guid? SpaceId{get;set;}public Space? Space{get;set;}
 public Guid ExamStageId{get;set;}public ExamStage ExamStage{get;set;}=null!;
 [MaxLength(200)]public string Title{get;set;}="";[MaxLength(800)]public string Description{get;set;}="";
 public int DurationSeconds{get;set;}=600;public bool FloorAtZero{get;set;}
 [MaxLength(20)]public string NavigationRule{get;set;}="FREE";
 public DateTimeOffset CreatedAt{get;set;}=DateTimeOffset.UtcNow;
 public List<MockSection> Sections{get;set;}=[];public List<MockTestQuestion> Questions{get;set;}=[];
}
public sealed class MockSection
{
 public Guid Id{get;set;}=Guid.NewGuid();public Guid MockTestId{get;set;}public MockTest MockTest{get;set;}=null!;
 public Guid SubjectId{get;set;}public Subject Subject{get;set;}=null!;[MaxLength(100)]public string Name{get;set;}="";public int Position{get;set;}
 public decimal MarksCorrect{get;set;}=2;public decimal NegativeMarks{get;set;}=.5m;
}
public sealed class MockTestQuestion
{
 public Guid MockTestId{get;set;}public MockTest MockTest{get;set;}=null!;public Guid QuestionId{get;set;}public Question Question{get;set;}=null!;
 public Guid MockSectionId{get;set;}public MockSection MockSection{get;set;}=null!;public int Position{get;set;}
}
public sealed class MockAttempt
{
 public Guid Id{get;set;}=Guid.NewGuid();public Guid UserId{get;set;}public User User{get;set;}=null!;public Guid? SpaceId{get;set;}public Space? Space{get;set;}
 public Guid MockTestId{get;set;}public MockTest MockTest{get;set;}=null!;public Guid StartKey{get;set;}
 public Guid? UserExamProfileId{get;set;}public UserExamProfile? UserExamProfile{get;set;}
 public DateTimeOffset StartedAt{get;set;}public DateTimeOffset ExpiresAt{get;set;}public DateTimeOffset? CompletedAt{get;set;}
 [MaxLength(16)]public string Status{get;set;}="ACTIVE";[MaxLength(16)]public string? CompletionReason{get;set;}
 public int Correct{get;set;}public int Incorrect{get;set;}public int Unattempted{get;set;}public int TimeTakenSeconds{get;set;}
 public decimal RawScore{get;set;}public decimal NegativeMarks{get;set;}public decimal FinalScore{get;set;}public decimal? AccuracyPercent{get;set;}
 public List<AttemptAnswer> Answers{get;set;}=[];
}
public sealed class AttemptAnswer
{
 public Guid MockAttemptId{get;set;}public MockAttempt MockAttempt{get;set;}=null!;public Guid QuestionId{get;set;}public Question Question{get;set;}=null!;
 public Guid? OptionId{get;set;}public QuestionOption? Option{get;set;}
 public bool Visited{get;set;}public bool MarkedForReview{get;set;}public int Version{get;set;}
 public decimal MarksCorrect{get;set;}public decimal NegativeMarks{get;set;}public bool? IsCorrect{get;set;}public decimal Score{get;set;}
}

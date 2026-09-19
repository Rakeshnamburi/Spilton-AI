using Microsoft.EntityFrameworkCore;
using Spilton.Api.Chat;
using Spilton.Api.Data;
namespace Spilton.Api.Mocks;
public sealed class MockException(int status,string message):Exception(message){public int Status{get;}=status;}
public sealed class AttemptService(AppDbContext db,TimeProvider clock)
{
 public DateTimeOffset Now=>clock.GetUtcNow();
 public Task<MockAttempt?> Load(Guid user,Guid id,CancellationToken ct)=>db.MockAttempts
  .Include(a=>a.MockTest).ThenInclude(t=>t.ExamStage).ThenInclude(s=>s.Exam)
  .Include(a=>a.MockTest).ThenInclude(t=>t.Sections)
  .Include(a=>a.MockTest).ThenInclude(t=>t.Questions)
  .Include(a=>a.Answers).ThenInclude(a=>a.Question).ThenInclude(q=>q.Options)
  .Include(a=>a.Answers).ThenInclude(a=>a.Question).ThenInclude(q=>q.Topic).ThenInclude(t=>t.Subject)
  .AsSplitQuery().SingleOrDefaultAsync(a=>a.Id==id&&a.UserId==user,ct);

 // Caller holds the attempt advisory lock. Status, scores and progress commit atomically.
 public async Task Complete(MockAttempt a,CancellationToken ct)
 {
  if(a.Status!="ACTIVE")return;
  var now=Now;var expired=now>=a.ExpiresAt;
  await using var transaction=await db.Database.BeginTransactionAsync(ct);
  foreach(var answer in a.Answers){answer.IsCorrect=answer.OptionId.HasValue?answer.Question.Options.Single(o=>o.Id==answer.OptionId).Index==answer.Question.CorrectOptionIndex:null;answer.Score=answer.IsCorrect switch{true=>answer.MarksCorrect,false=>-answer.NegativeMarks,_=>0};}
  a.Correct=a.Answers.Count(x=>x.IsCorrect==true);a.Incorrect=a.Answers.Count(x=>x.IsCorrect==false);a.Unattempted=a.Answers.Count(x=>!x.OptionId.HasValue);
  a.RawScore=a.Answers.Where(x=>x.IsCorrect==true).Sum(x=>x.MarksCorrect);a.NegativeMarks=a.Answers.Where(x=>x.IsCorrect==false).Sum(x=>x.NegativeMarks);a.FinalScore=a.MockTest.FloorAtZero?Math.Max(0,a.RawScore-a.NegativeMarks):a.RawScore-a.NegativeMarks;
  a.AccuracyPercent=a.Correct+a.Incorrect>0?Math.Round(100m*a.Correct/(a.Correct+a.Incorrect),2):null;
  a.CompletedAt=expired?a.ExpiresAt:now;a.TimeTakenSeconds=Math.Clamp((int)(a.CompletedAt.Value-a.StartedAt).TotalSeconds,0,(int)(a.ExpiresAt-a.StartedAt).TotalSeconds);a.Status="SUBMITTED";a.CompletionReason=expired?"EXPIRED":"USER_SUBMITTED";
  if(a.UserExamProfileId is {} profileId&&await db.UserExamProfiles.AnyAsync(p=>p.Id==profileId&&p.UserId==a.UserId&&p.SpaceId==a.SpaceId&&p.ExamStageId==a.MockTest.ExamStageId,ct)){
   foreach(var group in a.Answers.Where(x=>x.OptionId.HasValue).GroupBy(x=>x.Question.TopicId)){
    var topic=group.Key;var attempted=group.Count();var correct=group.Count(x=>x.IsCorrect==true);var id=Guid.NewGuid();
    await db.Database.ExecuteSqlInterpolatedAsync($"""
     INSERT INTO "UserTopicProgress" ("Id","UserExamProfileId","TopicId","Status","QuestionsAttempted","QuestionsCorrect","LastPracticedAt","UpdatedAt")
     VALUES ({id},{profileId},{topic},'PRACTICING',{attempted},{correct},{a.CompletedAt},{now})
     ON CONFLICT ("UserExamProfileId","TopicId") DO UPDATE SET
      "QuestionsAttempted"="UserTopicProgress"."QuestionsAttempted"+EXCLUDED."QuestionsAttempted",
      "QuestionsCorrect"="UserTopicProgress"."QuestionsCorrect"+EXCLUDED."QuestionsCorrect",
      "LastPracticedAt"=GREATEST("UserTopicProgress"."LastPracticedAt",EXCLUDED."LastPracticedAt"),"UpdatedAt"=EXCLUDED."UpdatedAt",
      "Status"=CASE WHEN "UserTopicProgress"."QuestionsAttempted"+EXCLUDED."QuestionsAttempted">=5 AND
       100.0*("UserTopicProgress"."QuestionsCorrect"+EXCLUDED."QuestionsCorrect")/("UserTopicProgress"."QuestionsAttempted"+EXCLUDED."QuestionsAttempted")>=80 THEN 'REVIEW' ELSE 'PRACTICING' END
     """,ct);
   }
  }
  await db.SaveChangesAsync(ct);await transaction.CommitAsync(ct);
 }
 public async Task Expire(Guid user,Guid id,CancellationToken ct){await using var gate=await ConversationLock.TryAcquire(db,id,ct);if(gate is null)return;var a=await Load(user,id,ct);if(a is {Status:"ACTIVE"}&&Now>=a.ExpiresAt)await Complete(a,ct);}
 public object View(MockAttempt a){
  var finished=a.Status=="SUBMITTED";var positions=a.MockTest.Questions.ToDictionary(q=>q.QuestionId,q=>q.Position);
  return new{a.Id,a.MockTestId,a.SpaceId,title=a.MockTest.Title,exam=a.MockTest.ExamStage.Exam.Name,stage=a.MockTest.ExamStage.Name,a.Status,a.StartedAt,a.ExpiresAt,a.CompletedAt,a.CompletionReason,serverNow=Now,
   sections=a.MockTest.Sections.OrderBy(s=>s.Position).Select(s=>new{s.Id,s.Name,s.SubjectId,s.MarksCorrect,s.NegativeMarks}),a.MockTest.NavigationRule,a.MockTest.FloorAtZero,
   questions=a.Answers.OrderBy(x=>positions[x.QuestionId]).Select(x=>new{id=x.QuestionId,position=positions[x.QuestionId],sectionId=a.MockTest.Questions.Single(l=>l.QuestionId==x.QuestionId).MockSectionId,text=x.Question.Text,topic=x.Question.Topic.Name,subject=x.Question.Topic.Subject.Name,x.Question.Difficulty,x.Question.SourceType,
    options=x.Question.Options.OrderBy(o=>o.Index).Select(o=>new{o.Id,o.Text}),answer=new{x.OptionId,x.Visited,x.MarkedForReview,x.Version},review=finished?new{correctOptionId=x.Question.Options.Single(o=>o.Index==x.Question.CorrectOptionIndex).Id,x.Question.Explanation,x.IsCorrect,x.Score,source=System.Text.Json.JsonSerializer.Deserialize<object>(x.Question.SourceMetadataJson)}:null}),
   result=finished?new{a.Correct,a.Incorrect,a.Unattempted,a.RawScore,a.NegativeMarks,a.FinalScore,a.AccuracyPercent,a.TimeTakenSeconds,
    subjects=a.Answers.GroupBy(x=>x.Question.Topic.Subject.Name).Select(g=>Group(g.Key,g)),topics=a.Answers.GroupBy(x=>x.Question.Topic.Name).Select(g=>Group(g.Key,g)),progressNotice="Only answered questions from this completed attempt count toward the matching profile. Repeated submission cannot add progress twice. Topic confidence requires at least five recorded answers."}:null};
 }
 public static string Classification(int attempted,int correct)=>attempted<5?"Limited evidence":100.0*correct/attempted>=80?"Strong":100.0*correct/attempted>=60?"Improving":"Needs Work";
 static object Group(string name,IEnumerable<AttemptAnswer> answers){var list=answers.ToArray();var n=list.Count(x=>x.OptionId.HasValue);var correct=list.Count(x=>x.IsCorrect==true);return new{name,total=list.Length,attempted=n,correct,accuracyPercent=n>0?Math.Round(100m*correct/n,2):(decimal?)null,score=list.Sum(x=>x.Score),classification=Classification(n,correct)};}
}
public sealed class AttemptExpiryWorker(IServiceScopeFactory scopes,ILogger<AttemptExpiryWorker> logger):BackgroundService
{
 protected override async Task ExecuteAsync(CancellationToken stoppingToken){while(!stoppingToken.IsCancellationRequested){try{using var scope=scopes.CreateScope();var db=scope.ServiceProvider.GetRequiredService<AppDbContext>();var service=scope.ServiceProvider.GetRequiredService<AttemptService>();var expired=await db.MockAttempts.AsNoTracking().Where(a=>a.Status=="ACTIVE"&&a.ExpiresAt<=service.Now).OrderBy(a=>a.ExpiresAt).Take(100).Select(a=>new{a.UserId,a.Id}).ToListAsync(stoppingToken);foreach(var a in expired)await service.Expire(a.UserId,a.Id,stoppingToken);}catch(OperationCanceledException)when(stoppingToken.IsCancellationRequested){break;}catch(Exception e){logger.LogWarning("Attempt expiry temporarily unavailable; retrying. ErrorCategory={ErrorCategory} ErrorType={ErrorType}",e is DbUpdateException?"database":"worker",e.GetType().Name);}try{await Task.Delay(5000,stoppingToken);}catch(OperationCanceledException){break;}}}
}

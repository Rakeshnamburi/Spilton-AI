using Microsoft.EntityFrameworkCore;
using Spilton.Api.Data;
namespace Spilton.Api.Preparation;
public sealed record TopicView(Guid Id,string Name,string Subject,string Status,int QuestionsAttempted,int QuestionsCorrect,double? AccuracyPercent,int? ConfidenceScore,DateTimeOffset? LastPracticedAt,DateTimeOffset? UpdatedAt);
public sealed record Focus(Guid TopicId,string Topic,string Subject,string Reason,double Priority);
public sealed class PreparationService(AppDbContext db)
{
    public Task<UserExamProfile?> Profile(Guid user,Guid? space,CancellationToken ct)=>db.UserExamProfiles.Include(p=>p.ExamStage).ThenInclude(s=>s.Exam).SingleOrDefaultAsync(p=>p.UserId==user&&p.SpaceId==space,ct);
    public async Task<List<TopicView>> Topics(UserExamProfile? profile,CancellationToken ct)
    {
        if(profile is null)return [];
        var topics=await db.Topics.AsNoTracking().Include(t=>t.Subject).Where(t=>t.Subject.ExamStageId==profile.ExamStageId).OrderBy(t=>t.Subject.Name).ThenBy(t=>t.Name).ToListAsync(ct);
        var progress=await db.UserTopicProgress.AsNoTracking().Where(p=>p.UserExamProfileId==profile.Id).ToDictionaryAsync(p=>p.TopicId,ct);
        return topics.Select(t=>{progress.TryGetValue(t.Id,out var p);return new TopicView(t.Id,t.Name,t.Subject.Name,p?.Status??"NOT_STARTED",p?.QuestionsAttempted??0,p?.QuestionsCorrect??0,p is {QuestionsAttempted:>0}?Math.Round(100.0*p.QuestionsCorrect/p.QuestionsAttempted,1):null,p?.ConfidenceScore,p?.LastPracticedAt,p?.UpdatedAt);}).ToList();
    }
    public static List<Focus> Recommend(IEnumerable<TopicView> topics,IEnumerable<Goal> goals,DateTimeOffset now)
    {
        return topics.Select(t=>{
            var days=t.LastPracticedAt is null?double.PositiveInfinity:(now-t.LastPracticedAt.Value).TotalDays;
            var (score,reason)=t.QuestionsAttempted>=10&&t.AccuracyPercent<60?(120-t.AccuracyPercent.Value,$"Reported accuracy {t.AccuracyPercent:0.#}% across {t.QuestionsAttempted} questions; revisit the basics."):
                t.Status is "REVIEW" or "MASTERED"&&days>=7?(55.0,"Review is due: no practice recorded in the last seven days."):
                t.Status=="NOT_STARTED"?(45.0,"Not started in your reported progress."):
                t.Status=="LEARNING"?(50.0,"Continue learning before moving to timed practice."):(30.0,"Build consistency with focused practice.");
            if(goals.Any(g=>g.Status=="ACTIVE"&&g.Title.Contains(t.Name,StringComparison.OrdinalIgnoreCase)&&g.TargetDate is {} due&&due<=DateOnly.FromDateTime(now.LocalDateTime).AddDays(14))){score+=20;reason+=" A matching goal is due within two weeks.";}
            return new Focus(t.Id,t.Name,t.Subject,reason,score);
        }).OrderByDescending(f=>f.Priority).ThenBy(f=>f.Topic).ToList();
    }
    public static double Indicator(IEnumerable<TopicView> topics){var items=topics.ToArray();if(items.Length==0)return 0;return Math.Round(items.Average(t=>t.Status switch{"LEARNING"=>25,"PRACTICING"=>50,"REVIEW"=>75,"MASTERED"=>100,_=>0}),1);}
}

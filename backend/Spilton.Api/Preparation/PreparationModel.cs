using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Spilton.Api.Data;
namespace Spilton.Api.Preparation;
public static class PreparationModel
{
    public static void Configure(ModelBuilder model)
    {
        var spaces=model.Entity<Space>();spaces.HasOne(s=>s.User).WithMany().HasForeignKey(s=>s.UserId).OnDelete(DeleteBehavior.Restrict);spaces.HasIndex(s=>new{s.UserId,s.IsArchived});
        model.Entity<Exam>().HasIndex(e=>e.Name).IsUnique();
        model.Entity<ExamStage>().HasOne(s=>s.Exam).WithMany().HasForeignKey(s=>s.ExamId).OnDelete(DeleteBehavior.Restrict);
        model.Entity<Subject>().HasOne(s=>s.ExamStage).WithMany().HasForeignKey(s=>s.ExamStageId).OnDelete(DeleteBehavior.Restrict);
        model.Entity<Topic>().HasOne(t=>t.Subject).WithMany().HasForeignKey(t=>t.SubjectId).OnDelete(DeleteBehavior.Restrict);
        foreach(var type in new[]{typeof(UserExamProfile),typeof(Goal),typeof(Memory),typeof(StudyPlan)}){
            model.Entity(type).HasOne(typeof(User),"User").WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.Restrict);
            model.Entity(type).HasOne(typeof(Space),"Space").WithMany().HasForeignKey("SpaceId").OnDelete(DeleteBehavior.Restrict);
            model.Entity(type).HasIndex("UserId","SpaceId");
        }
        var profiles=model.Entity<UserExamProfile>();profiles.HasOne(p=>p.ExamStage).WithMany().HasForeignKey(p=>p.ExamStageId).OnDelete(DeleteBehavior.Restrict);
        profiles.HasIndex(p=>new{p.UserId,p.SpaceId}).IsUnique();profiles.HasIndex(p=>p.UserId).IsUnique().HasFilter("\"SpaceId\" IS NULL");
        var progress=model.Entity<UserTopicProgress>();progress.HasOne(p=>p.UserExamProfile).WithMany().HasForeignKey(p=>p.UserExamProfileId).OnDelete(DeleteBehavior.Cascade);progress.HasOne(p=>p.Topic).WithMany().HasForeignKey(p=>p.TopicId).OnDelete(DeleteBehavior.Restrict);progress.HasIndex(p=>new{p.UserExamProfileId,p.TopicId}).IsUnique();
        progress.ToTable(t=>t.HasCheckConstraint("CK_Progress_Counts","\"QuestionsAttempted\" >= 0 AND \"QuestionsCorrect\" >= 0 AND \"QuestionsCorrect\" <= \"QuestionsAttempted\""));
        var items=model.Entity<StudyPlanItem>();items.HasOne(i=>i.StudyPlan).WithMany(p=>p.Items).HasForeignKey(i=>i.StudyPlanId).OnDelete(DeleteBehavior.Cascade);items.HasOne(i=>i.Topic).WithMany().HasForeignKey(i=>i.TopicId).OnDelete(DeleteBehavior.Restrict);items.HasIndex(i=>new{i.StudyPlanId,i.Date});
        // Curated starter taxonomy, not an official or complete current syllabus.
        foreach(var (name,family) in new[]{("SSC CGL","SSC"),("SSC CHSL","SSC"),("RRB NTPC","RRB"),("Banking Preparation","Banking"),("APPSC Preparation","APPSC"),("UPSC Preparation","UPSC"),
            ("SSC MTS","SSC Other"),("SSC GD","SSC Other"),("SSC CPO","SSC Other"),("SSC JE","SSC Other"),
            ("RRB Group D","RRB"),("RRB ALP","RRB"),("RRB JE","RRB"),
            ("IBPS PO","Banking"),("IBPS Clerk","Banking"),("IBPS RRB","Banking"),("SBI PO","Banking"),("SBI Clerk","Banking"),("RBI Grade B","Banking"),
            ("UPSC Civil Services","UPSC"),("UPSC NDA","Defence"),("UPSC CDS","Defence"),("UPSC CAPF","Defence"),("AFCAT","Defence"),
            ("APPSC Group 1","State PSC"),("APPSC Group 2","State PSC"),("Telangana Group 1","State PSC"),("Telangana Group 2","State PSC"),
            ("TNPSC","State PSC"),("Karnataka PSC","State PSC"),("Kerala PSC","State PSC"),("MPSC","State PSC"),("UPPSC","State PSC"),("BPSC","State PSC"),("MPPSC","State PSC"),("RPSC","State PSC"),
            ("WBPSC","State PSC"),("OPSC","State PSC"),("GPSC","State PSC"),("Punjab PSC","State PSC"),("Haryana PSC","State PSC"),("Assam PSC","State PSC"),
            ("CTET","Teaching"),("UGC NET","Teaching"),("CSIR NET","Teaching"),("State TET","Teaching")}){
            var exam=Id(name);model.Entity<Exam>().HasData(new Exam{Id=exam,Name=name,Family=family});
            foreach(var stageName in family=="SSC"?new[]{"Tier 1","Tier 2"}:new[]{"General preparation"}){
                var stage=Id(name+stageName);model.Entity<ExamStage>().HasData(new ExamStage{Id=stage,ExamId=exam,Name=stageName});
                foreach(var (subjectName,topics) in new[]{("Quantitative Aptitude",new[]{"Percentage","Profit and Loss","Geometry"}),("Reasoning",new[]{"Coding-Decoding","Series","Logical Reasoning"}),("English",new[]{"Error Detection","Reading Comprehension","Vocabulary"}),("General Awareness",new[]{"Static GK","History","Geography"})}){
                    var subject=Id(name+stageName+subjectName);model.Entity<Subject>().HasData(new Subject{Id=subject,ExamStageId=stage,Name=subjectName});
                    foreach(var topic in topics)model.Entity<Topic>().HasData(new Topic{Id=Id(name+stageName+subjectName+topic),SubjectId=subject,Name=topic});
                }
            }
        }
    }
    public static Guid Id(string value)=>new(SHA256.HashData(Encoding.UTF8.GetBytes("spilton-catalog:"+value)).AsSpan(0,16));
}

using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Spilton.Api.Preparation;
namespace Spilton.Api.Mocks;
public static class MockSeed
{
 public static Guid Id(string key)=>new(SHA256.HashData(Encoding.UTF8.GetBytes("spilton-mocks:"+key)).AsSpan(0,16));
 public static void Configure(ModelBuilder m){
  const string prefix="SSC CGLTier 1";var test=Id("starter");
  m.Entity<MockTest>().HasData(new MockTest{Id=test,ExamStageId=PreparationModel.Id(prefix),Title="SSC CGL starter · 16 original practice questions",Description="Manually authored development practice, not an official paper or a complete current exam pattern. Free section navigation. Stored sample scoring: +2 correct, −0.5 incorrect, 0 unattempted.",DurationSeconds=600,CreatedAt=new DateTimeOffset(2026,9,10,0,0,0,TimeSpan.Zero)});
  var subjects=new[]{"Quantitative Aptitude","Reasoning","English","General Awareness"};for(var s=0;s<subjects.Length;s++)m.Entity<MockSection>().HasData(new MockSection{Id=Id("section"+s),MockTestId=test,SubjectId=PreparationModel.Id(prefix+subjects[s]),Name=subjects[s],Position=s,MarksCorrect=2,NegativeMarks=.5m});
  (int Subject,string Topic,string Text,string[] Options,int Correct,string Explanation)[] bank=[
   (0,"Percentage","What is 25% of 480?",["100","120","140","160"],1,"25% is one quarter. 480 ÷ 4 = 120."),
   (0,"Percentage","What is 20% of 250?",["50","25","75","100"],0,"20/100 × 250 = 50."),
   (0,"Percentage","What is 15% of 200?",["15","20","30","40"],2,"15/100 × 200 = 30."),
   (0,"Percentage","What is 10% of 750?",["7.5","75","150","750"],1,"Ten percent is one tenth: 750 ÷ 10 = 75."),
   (0,"Percentage","What is 40% of 150?",["15","30","45","60"],3,"40/100 × 150 = 60."),
   (0,"Percentage","What is 12.5% of 80?",["10","12","8","16"],0,"12.5% equals one eighth. 80 ÷ 8 = 10."),
   (0,"Profit and Loss","An item costs ₹100 and sells for ₹120. What is the profit percentage on cost?",["10%","20%","25%","120%"],1,"Profit is ₹20. Profit percentage is 20/100 × 100 = 20%."),
   (0,"Profit and Loss","An item costs ₹200 and sells for ₹180. What is the loss percentage on cost?",["5%","20%","10%","90%"],2,"Loss is ₹20. Loss percentage is 20/200 × 100 = 10%."),
   (1,"Series","What comes next: 2, 4, 8, 16, ...?",["20","24","32","64"],2,"Each term is twice the preceding term. 16 × 2 = 32."),
   (1,"Series","What comes next: 3, 6, 9, ...?",["10","12","15","18"],1,"Add 3 to each term; 9 + 3 = 12."),
   (1,"Coding-Decoding","If A=1, B=2, ..., Z=26, what is the sum of the letter values in CAT?",["21","23","24","26"],2,"C=3, A=1, T=20; the sum is 24."),
   (1,"Coding-Decoding","Each letter is replaced by the next alphabet letter. How is DOG encoded?",["EPH","EPG","ENH","FPH"],0,"D→E, O→P, G→H, so DOG becomes EPH."),
   (2,"Error Detection","Choose the correct word: She ___ to school every day.",["go","goes","going","gone"],1,"A singular third-person subject uses 'goes' in the simple present."),
   (2,"Vocabulary","Which word is closest in meaning to 'rapid'?",["slow","late","fast","weak"],2,"Rapid means fast or quick."),
   (3,"Static GK","What is the chemical formula of water?",["CO2","O2","H2O","NaCl"],2,"A water molecule contains two hydrogen atoms and one oxygen atom: H2O."),
   (3,"Static GK","What is the capital of India?",["Mumbai","New Delhi","Chennai","Kolkata"],1,"New Delhi is the capital of India.")
  ];
  for(var i=0;i<bank.Length;i++){var b=bank[i];var id=Id("question"+i);m.Entity<Question>().HasData(new Question{Id=id,TopicId=PreparationModel.Id(prefix+subjects[b.Subject]+b.Topic),Text=b.Text,CorrectOptionIndex=b.Correct,Explanation=b.Explanation,SourceType="MANUAL_DEVELOPMENT",SourceMetadataJson="{\"label\":\"Original development question; not PYQ\"}"});for(var n=0;n<4;n++)m.Entity<QuestionOption>().HasData(new QuestionOption{Id=Id($"question{i}-option{n}"),QuestionId=id,Index=n,Text=b.Options[n]});m.Entity<MockTestQuestion>().HasData(new MockTestQuestion{MockTestId=test,QuestionId=id,MockSectionId=Id("section"+b.Subject),Position=i});}
 }
}

using Spilton.Api.Chat;
namespace Spilton.Api.Tests;
public sealed class CapabilityTests
{
    [Theory]
    [InlineData("Explain cloud computing simply.",Capability.TUTOR)]
    [InlineData("Write Python palindrome program.",Capability.CODING)]
    [InlineData("Build a responsive login page using HTML and CSS.",Capability.CODING)]
    [InlineData("Create a React counter component.",Capability.CODING)]
    [InlineData("Explain Java inheritance with code.",Capability.CODING)]
    [InlineData("Create a simple ASP.NET Core API example.",Capability.CODING)]
    [InlineData("Explain this TypeScript generic.",Capability.CODING)]
    [InlineData("Create a Next.js server component.",Capability.CODING)]
    [InlineData("Explain SQL joins with examples.",Capability.CODING)]
    [InlineData("Teach recursion like I'm a beginner.",Capability.TUTOR)]
    [InlineData("Why does this code throw an exception?",Capability.CODING)]
    [InlineData("Teach SSC CGL percentages.",Capability.EXAM)]
    [InlineData("Teach percentage",Capability.EXAM)]
    [InlineData("Build percentage calculator in JavaScript",Capability.CODING)]
    [InlineData("Latest .NET changes",Capability.RESEARCH)]
    public void Routes_without_exam_contamination(string input,Capability expected)
    {
        var route=new CapabilityRouter().Route(input,[],false,true,"quick");
        Assert.Equal(expected,route.Capability);Assert.Equal(expected==Capability.EXAM,route.UseExamContext);
    }
    [Fact] public void Selected_documents_and_followups_are_preserved()
    {
        var router=new CapabilityRouter();
        Assert.Equal(Capability.DOCUMENT_RAG,router.Route("Explain Java",[],true,true,"quick").Capability);
        Assert.Equal(Capability.CODING,router.Route("Make it responsive",[new Message{Role="USER",Content="Build a login page"}],false,true,"quick").Capability);
        Assert.Equal(Capability.CODING,router.Route("Now convert the previous HTML page to React",[],false,true,"quick").Capability);
        Assert.Equal(Capability.TUTOR,router.Route("Explain photosynthesis",[],false,true,"quick").Capability);
    }
    [Fact] public void Bounded_context_retains_latest_code_and_explicit_full_code_instruction()
    {
        var history=Enumerable.Range(0,40).Select(i=>new Message{Role="USER",Content=new string('x',900),Status="completed"}).ToList();
        history.Add(new Message{Role="USER",Content="Give full code",Status="completed"});
        var result=new ContextBuilder().Build(history,new(Capability.CODING,false,"test"),"think");
        Assert.True(result.Count<=22);Assert.Equal("Give full code",result.Last().Content);
        Assert.Contains("explicit full-code",result[0].Content);Assert.Contains("cannot execute",result[0].Content);
    }
    [Fact] public void Compression_is_extractive_bounded_and_skips_assistant_claims()
    {
        var messages=Enumerable.Range(0,12).Select(i=>new Message{Role="USER",Content=$"Project fact {i}",Status="completed"}).ToList();
        messages.Add(new Message{Role="ASSISTANT",Content="Invented assistant claim",Status="completed"});
        var result=new ConversationCompressor().Compress(messages,80);
        Assert.True(result.Summary.Length<=80);Assert.DoesNotContain("Invented",result.Summary);Assert.Contains("Project fact",result.Summary);
    }
}

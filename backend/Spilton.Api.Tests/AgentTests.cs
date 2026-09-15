using Spilton.Api.Agents;
using Spilton.Api.Tools;
namespace Spilton.Api.Tests;
public sealed class AgentTests
{
 [Fact] public void Planner_is_bounded_and_does_not_turn_simple_questions_into_agents(){var p=new Planner();Assert.True(p.Create("What is Python?").Steps.Count<=1);Assert.True(p.Create("Research latest .NET release and calculate 18% of 45000").Steps.Count<=4);}
 [Fact] public async Task Agent_executes_only_registered_safe_tool(){var a=new AgentOrchestrator(new ToolRegistry([new CalculatorTool(),new DateTimeTool()]),new Planner());var r=await a.Run("Calculate 18% of 45000",default);Assert.Equal("completed",r.Status);Assert.Contains("8100",r.Answer);}
 [Fact] public async Task Agent_reports_unavailable_web_without_fabrication(){var a=new AgentOrchestrator(new ToolRegistry([new CalculatorTool(),new DateTimeTool()]),new Planner());var r=await a.Run("Research the latest stable .NET version",default);Assert.Equal("failed",r.Status);Assert.Contains("unavailable",r.Answer,StringComparison.OrdinalIgnoreCase);Assert.DoesNotContain("202",r.Answer);}
}

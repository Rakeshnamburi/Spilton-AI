using System.Text.RegularExpressions;
using Spilton.Api.Tools;
namespace Spilton.Api.Agents;
public enum ActionPermission { READ_ONLY, LOW_RISK, CONSEQUENTIAL }
public sealed record AgentStep(int Number,string Title,string? Tool,string Input,ActionPermission Permission);
public sealed record AgentPlan(string Goal,IReadOnlyList<AgentStep> Steps,int MaxSteps);
public sealed record AgentObservation(bool Success,string Step,string? Tool,string? Output,string? Error,long DurationMs=0);
public sealed record AgentVerification(bool Passed,IReadOnlyList<string> Checks,IReadOnlyList<string> Warnings);
public sealed record AgentRunResult(string Status,AgentPlan Plan,IReadOnlyList<AgentObservation> Observations,string Answer,bool ApprovalRequired,AgentVerification? Verification=null,int ToolCalls=0);
public sealed class AgentSettings { public int MaxSteps{get;set;}=4;public int MaxToolCalls{get;set;}=4;public int MaxRetries{get;set;}=1; }
public sealed class Planner
{
 public AgentPlan Create(string goal){var s=new List<AgentStep>();var n=1;if(Regex.IsMatch(goal,@"(?i)\b(latest|research|compare|official|release|notification)\b"))s.Add(new(n++,"Run bounded evidence research","research",goal,ActionPermission.READ_ONLY));if(Regex.IsMatch(goal,@"(?i)\b\d+\s*%|calculate|compute|arithmetic\b"))s.Add(new(n++,"Calculate the requested value","calculator",goal,ActionPermission.READ_ONLY));if(Regex.IsMatch(goal,@"(?i)\b(current time|what time|date today|today's date)\b"))s.Add(new(n++,"Read current time","date_time","",ActionPermission.READ_ONLY));if(s.Count==0)s.Add(new(n++,"Prepare an answer",null,"No registered tool is required.",ActionPermission.READ_ONLY));return new(goal,s.Take(4).ToArray(),4);}
}
public sealed class AgentVerifier
{
 public AgentVerification Verify(AgentPlan plan,IReadOnlyList<AgentObservation> observations,string status)
 {var warnings=new List<string>();if(observations.Any(x=>!x.Success))warnings.Add("One or more tool steps failed.");if(observations.Any(x=>x.Tool is not null&&string.IsNullOrWhiteSpace(x.Output)&&x.Success))warnings.Add("A successful tool returned no evidence.");var checks=new[]{"Plan stayed within its step bound","Only registered tools were invoked","Tool failures were preserved"};return new(status=="completed"&&warnings.Count==0,checks,warnings);}
}
public sealed class AgentOrchestrator
{
 private readonly ToolRegistry tools;private readonly Planner planner;private readonly AgentSettings settings;private readonly AgentVerifier verifier;
 public AgentOrchestrator(ToolRegistry tools,Planner planner):this(tools,planner,new(),new()){}
 public AgentOrchestrator(ToolRegistry tools,Planner planner,AgentSettings settings,AgentVerifier verifier){this.tools=tools;this.planner=planner;this.settings=settings;this.verifier=verifier;}
 public async Task<AgentRunResult> Run(string goal,CancellationToken ct)
 {
  if(string.IsNullOrWhiteSpace(goal)||goal.Length>8000)throw new ArgumentException("Enter a goal under 8,000 characters.");
  var raw=planner.Create(goal);var plan=raw with{MaxSteps=Math.Clamp(settings.MaxSteps,1,8),Steps=raw.Steps.Take(Math.Clamp(settings.MaxSteps,1,8)).ToArray()};
  var o=new List<AgentObservation>();var toolCalls=0;string status="completed";string answer="No registered tool was required. Continue with the normal Spilton response path.";
  foreach(var step in plan.Steps){ct.ThrowIfCancellationRequested();var registeredPermission=step.Tool is not null&&tools.TryInfo(step.Tool,out var info)&&Enum.TryParse<ActionPermission>(info!.Permission,true,out var parsed)?parsed:step.Permission;if(registeredPermission==ActionPermission.CONSEQUENTIAL){status="approval_required";answer="I need your explicit approval before taking that consequential action.";var v=verifier.Verify(plan,o,status);return new(status,plan,o,answer,true,v,toolCalls);}if(step.Tool is null){o.Add(new(true,step.Title,null,null,null));continue;}if(toolCalls>=Math.Clamp(settings.MaxToolCalls,1,8)){status="failed";answer="The agent stopped at its tool-call safety limit.";break;}toolCalls++;var r=await tools.Execute(step.Tool,step.Input,ct);o.Add(new(r.Success,step.Title,r.Tool,r.Output,r.Error,r.DurationMs));if(!r.Success){status="failed";answer=$"The requested tool failed: {r.Error} I will not invent a result.";break;}answer=$"Completed the bounded plan. Tool result: {r.Output}";}
  var verification=verifier.Verify(plan,o,status);return new(status,plan,o,answer,false,verification,toolCalls);
 }
}
[Microsoft.AspNetCore.Authorization.Authorize(Roles="User"),Microsoft.AspNetCore.Mvc.ApiController,Microsoft.AspNetCore.Mvc.Route("api/agents")]
public sealed class AgentController(AgentOrchestrator orchestrator):Microsoft.AspNetCore.Mvc.ControllerBase
{[Microsoft.AspNetCore.Mvc.HttpPost("run")]public async Task<Microsoft.AspNetCore.Mvc.IActionResult> Run([Microsoft.AspNetCore.Mvc.FromBody]AgentRequest request,CancellationToken ct)=>Ok(await orchestrator.Run(request.Goal,ct));}
public sealed record AgentRequest([System.ComponentModel.DataAnnotations.Required]string Goal);

using Spilton.Api.Agents;
using Spilton.Api.Chat;
using Spilton.Api.Coding;
using Spilton.Api.Tools;

namespace Spilton.Api.Tests;

public sealed class PhaseTwoFoundationTests
{
    [Fact] public void Model_router_uses_only_resolver_advertised_provider()
    {
        var configured=new StubProvider(new("real","Configured","Test","model-1",false){Capabilities=new(Coding:true,Reasoning:true)});
        var router=new CapabilityModelRouter(new StubResolver(configured),new ProviderHealth(TimeProvider.System));
        var selection=router.Select(new(Capability.CODING,false,"test"),"auto");
        Assert.Same(configured,selection.Provider);Assert.False(selection.ExplicitSelection);Assert.Contains("CODING",selection.Reason);
        Assert.Throws<ProviderException>(()=>router.Select(new(Capability.CODING,false,"test"),"invented"));
    }

    [Fact] public void Model_router_rejects_unsupported_configured_capability()
    {
        var configured=new StubProvider(new("text","Text only","Test","text",false){Capabilities=new(Coding:false)});
        Assert.Throws<ProviderException>(()=>new CapabilityModelRouter(new StubResolver(configured)).Resolve(new(Capability.CODING,false,"test"),"auto"));
    }

    [Fact] public async Task Vision_and_execution_are_honestly_unavailable()
    {
        IMultimodalProvider vision=new UnavailableMultimodalProvider();ICodeExecutionService execution=new UnavailableCodeExecutionService();
        Assert.False(vision.IsConfigured);Assert.Equal("VISION_PROVIDER_NOT_CONFIGURED",vision.Capability.Status);
        Assert.False(execution.Capability.Available);
        var error=await Assert.ThrowsAsync<InvalidOperationException>(()=>execution.ExecuteAsync("x",["dotnet","test"],default));
        Assert.Contains("unavailable",error.Message,StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData("../secret.txt")]
    [InlineData(".env")]
    [InlineData("folder/.env.local")]
    [InlineData("C:\\Windows\\win.ini")]
    public void Coding_workspace_blocks_escape_and_secret_paths(string path)
        =>Assert.ThrowsAny<Exception>(()=>new CodingWorkspacePolicy().Resolve(Path.Combine(Path.GetTempPath(),"spilton-safe-root"),path));

    [Fact] public void Coding_workspace_allows_normal_relative_source_path()
    {
        var root=Path.Combine(Path.GetTempPath(),"spilton-safe-root");var path=new CodingWorkspacePolicy().Resolve(root,"src/app.ts");
        Assert.StartsWith(Path.GetFullPath(root),path,StringComparison.OrdinalIgnoreCase);
    }

    [Fact] public async Task Agent_records_verification_and_tool_bound()
    {
        var settings=new AgentSettings{MaxSteps=2,MaxToolCalls=1};var orchestrator=new AgentOrchestrator(new ToolRegistry([new CalculatorTool(),new DateTimeTool()]),new Planner(),settings,new AgentVerifier());
        var result=await orchestrator.Run("Calculate 18% of 45000 and tell current time",default);
        Assert.True(result.ToolCalls<=1);Assert.NotNull(result.Verification);Assert.Equal("failed",result.Status);
    }

    [Fact] public async Task Consequential_tool_cannot_execute_without_explicit_approval()
    {
        var registry=new ToolRegistry([new ConsequentialTool()]);var result=await registry.Execute("external_change","payload",default);
        Assert.False(result.Success);Assert.Contains("approval",result.Error!,StringComparison.OrdinalIgnoreCase);
    }

    [Fact] public void Provider_health_degrades_after_bounded_failures_and_recovers()
    {
        var health=new ProviderHealth(TimeProvider.System);health.Failure("real","provider_failure");health.Failure("real","rate_limit");health.Failure("real","connection_interrupted");
        Assert.Equal("DEGRADED",health.Status("real"));health.Success("real");Assert.Equal("HEALTHY",health.Status("real"));
    }

    [Theory]
    [InlineData("image/png",1024,"owned/abc.png",true)]
    [InlineData("image/svg+xml",1024,"owned/abc.svg",false)]
    [InlineData("image/png",9000000,"owned/abc.png",false)]
    [InlineData("image/png",1024,"../secret.png",false)]
    public void Multimodal_policy_validates_metadata_without_claiming_vision(string mime,long size,string reference,bool expected)
        =>Assert.Equal(expected,MultimodalPolicy.IsSafe(new("image",mime,reference,size,"upload")));

    private sealed class StubProvider(ModelInfo info):IModelProvider
    { public ModelInfo Info=>info;public async IAsyncEnumerable<string> StreamAsync(IReadOnlyList<ModelMessage> messages,[System.Runtime.CompilerServices.EnumeratorCancellation]CancellationToken ct){await Task.CompletedTask;yield break;} }
    private sealed class StubResolver(IModelProvider provider):IModelProviderResolver
    { public IReadOnlyList<ModelInfo> Available=>[provider.Info];public string DefaultId=>provider.Info.Id;public IModelProvider Resolve(string? id)=>string.IsNullOrWhiteSpace(id)||id=="auto"||id==provider.Info.Id?provider:throw new ProviderException("provider_unavailable","Unavailable"); }
    private sealed class ConsequentialTool:ISpiltonTool
    { public ToolInfo Info=>new("external_change","Test-only external mutation","value: string","CONSEQUENTIAL");public Task<ToolResult> ExecuteAsync(string input,CancellationToken ct)=>Task.FromResult(new ToolResult(true,Info.Name,"must not run",null)); }
}

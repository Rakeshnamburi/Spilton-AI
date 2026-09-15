using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;
namespace Spilton.Api.Tools;

public sealed record ToolInfo(string Name,string Description,string InputSchema,string Permission);
public sealed record ToolResult(bool Success,string Tool,string? Output,string? Error,long DurationMs=0);
public interface ISpiltonTool
{
    ToolInfo Info { get; }
    Task<ToolResult> ExecuteAsync(string input,CancellationToken ct);
}

public sealed class CalculatorTool : ISpiltonTool
{
    public ToolInfo Info=>new("calculator","Evaluate basic arithmetic locally; no model or web call.","expression: string","READ_ONLY");
    public Task<ToolResult> ExecuteAsync(string input,CancellationToken ct)
    {
        if(string.IsNullOrWhiteSpace(input)||input.Length>200)return Task.FromResult(new ToolResult(false,Info.Name,null,"Enter a short arithmetic expression."));
        try {
            var expression=input.Trim();
            var percent=Regex.Match(expression,@"(?i)([-+]?\d+(?:\.\d+)?)\s*%\s*(?:of|x|\*)\s*([-+]?\d+(?:\.\d+)?)");
            if(percent.Success)expression=$"({percent.Groups[1].Value}/100)*{percent.Groups[2].Value}";
            else expression=Regex.Replace(expression,@"(?i)^\s*(calculate|compute|what is)\s+","").Trim().TrimEnd('?');
            var value=new Parser(expression).Parse(); return Task.FromResult(new ToolResult(true,Info.Name,value.ToString("0.##########",CultureInfo.InvariantCulture),null));
        }
        catch { return Task.FromResult(new ToolResult(false,Info.Name,null,"Only numbers, parentheses and +, -, *, /, % are supported.")); }
    }
    private sealed class Parser(string text)
    {
        int p;
        public decimal Parse(){var x=Add();Skip();if(p!=text.Length)throw new();return x;}
        decimal Add(){var x=Mul();while(true){Skip();if(Match('+'))x+=Mul();else if(Match('-'))x-=Mul();else return x;}}
        decimal Mul(){var x=Unary();while(true){Skip();if(Match('*'))x*=Unary();else if(Match('/')){var y=Unary();if(y==0)throw new();x/=y;}else if(Match('%')){var y=Unary();if(y==0)throw new();x%=y;}else return x;}}
        decimal Unary(){Skip();if(Match('+'))return Unary();if(Match('-'))return -Unary();if(Match('(')){var x=Add();if(!Match(')'))throw new();return x;}var start=p;while(p<text.Length&&(char.IsDigit(text[p])||text[p]=='.'))p++;if(start==p||!decimal.TryParse(text[start..p],NumberStyles.Number,CultureInfo.InvariantCulture,out var n))throw new();return n;}
        void Skip(){while(p<text.Length&&char.IsWhiteSpace(text[p]))p++;} bool Match(char c){Skip();if(p<text.Length&&text[p]==c){p++;return true;}return false;}
    }
}
public sealed class DateTimeTool : ISpiltonTool
{
    public ToolInfo Info=>new("date_time","Return the current UTC time or a requested IANA/Windows time zone.","timeZone: optional string","READ_ONLY");
    public Task<ToolResult> ExecuteAsync(string input,CancellationToken ct)
    { try { var zone=string.IsNullOrWhiteSpace(input)?TimeZoneInfo.Utc:TimeZoneInfo.FindSystemTimeZoneById(input.Trim());var now=TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow,zone);return Task.FromResult(new ToolResult(true,Info.Name,$"{now:yyyy-MM-dd HH:mm:ss zzz} ({zone.Id})",null)); } catch { return Task.FromResult(new ToolResult(false,Info.Name,null,"That time zone is not available on this server.")); } }
}
public sealed class ToolRegistry(IEnumerable<ISpiltonTool> tools)
{
    private readonly IReadOnlyDictionary<string,ISpiltonTool> all=tools.ToDictionary(x=>x.Info.Name,StringComparer.OrdinalIgnoreCase);
    public IReadOnlyList<ToolInfo> Available=>all.Values.Select(x=>x.Info).ToArray();
    public bool TryInfo(string name,out ToolInfo? info){if(all.TryGetValue(name,out var tool)){info=tool.Info;return true;}info=null;return false;}
    public async Task<ToolResult> Execute(string name,string input,CancellationToken ct,bool approved=false){if(!all.TryGetValue(name,out var tool))return new(false,name,null,"This tool is unavailable.");if(tool.Info.Permission=="CONSEQUENTIAL"&&!approved)return new(false,name,null,"Explicit user approval is required for this tool.");var clock=System.Diagnostics.Stopwatch.StartNew();try{return (await tool.ExecuteAsync(input,ct)) with {DurationMs=clock.ElapsedMilliseconds};}catch(OperationCanceledException){return new(false,name,null,"Tool execution was cancelled.",clock.ElapsedMilliseconds);}catch{return new(false,name,null,"Tool execution failed safely.",clock.ElapsedMilliseconds);}}
}
 [Microsoft.AspNetCore.Authorization.Authorize(Roles="User"),Microsoft.AspNetCore.Mvc.ApiController,Microsoft.AspNetCore.Mvc.Route("api/tools")]
public sealed class ToolController(ToolRegistry tools):Microsoft.AspNetCore.Mvc.ControllerBase
{
    [Microsoft.AspNetCore.Mvc.HttpGet] public Microsoft.AspNetCore.Mvc.IActionResult List()=>Ok(new{tools.Available});
    [Microsoft.AspNetCore.Mvc.HttpPost("{name}")] public async Task<Microsoft.AspNetCore.Mvc.IActionResult> Run(string name,[Microsoft.AspNetCore.Mvc.FromBody] ToolRequest request,CancellationToken ct){var result=await tools.Execute(name,request.Input??"",ct);return result.Success?Ok(result):Problem(statusCode:400,title:result.Error);}
}
public sealed record ToolRequest([StringLength(200)] string? Input);

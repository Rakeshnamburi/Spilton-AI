using Spilton.Api.Tools;
namespace Spilton.Api.Tests;
public sealed class ToolTests
{
 [Theory]
 [InlineData("18% of 45000",8100)]
 [InlineData("(12 + 8) * 3",60)]
 [InlineData("100 / 4 + 2",27)]
 public async Task Calculator_is_deterministic(string input,decimal expected){var r=await new CalculatorTool().ExecuteAsync(input,default);Assert.True(r.Success);Assert.Equal(expected.ToString(),r.Output);}
 [Fact] public async Task Calculator_rejects_code_or_division_by_zero(){var t=new CalculatorTool();Assert.False((await t.ExecuteAsync("1/0",default)).Success);Assert.False((await t.ExecuteAsync("System.IO.File.Delete('x')",default)).Success);}
 [Fact] public async Task Registry_exposes_only_safe_initial_tools(){var c=new CalculatorTool();var d=new DateTimeTool();var r=new ToolRegistry([c,d]);Assert.Equal(new[]{"calculator","date_time"},r.Available.Select(x=>x.Name).Order());Assert.False((await r.Execute("web_search","latest",default)).Success);}
}

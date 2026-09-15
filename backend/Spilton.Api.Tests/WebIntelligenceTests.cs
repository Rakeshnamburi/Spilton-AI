using System.Net;
using System.Text;
using Microsoft.Extensions.Logging.Abstractions;
using Spilton.Api.Web;
namespace Spilton.Api.Tests;
public sealed class WebIntelligenceTests
{
 [Theory][InlineData("0.0.0.0")][InlineData("127.0.0.1")][InlineData("::ffff:127.0.0.1")][InlineData("10.0.0.1")][InlineData("::ffff:10.0.0.1")][InlineData("169.254.169.254")][InlineData("::ffff:169.254.169.254")][InlineData("192.168.1.2")][InlineData("::ffff:192.168.1.2")][InlineData("172.16.0.1")][InlineData("224.0.0.1")][InlineData("::1")][InlineData("fe80::1")][InlineData("fd00::1")][InlineData("ff02::1")][InlineData("::")]
 public void Private_and_metadata_addresses_are_blocked(string value)=>Assert.True(SafeWebPageReader.IsPrivate(IPAddress.Parse(value)));
 [Theory][InlineData("1.1.1.1")][InlineData("8.8.8.8")][InlineData("2001:4860:4860::8888")]
 public void Legitimate_public_addresses_remain_allowed(string value)=>Assert.False(SafeWebPageReader.IsPrivate(IPAddress.Parse(value)));
 [Fact] public async Task Reader_rejects_localhost_and_unsupported_scheme(){var r=Reader(new StubHandler("text/plain","unused"));await Assert.ThrowsAsync<Spilton.Api.Web.WebException>(()=>r.ReadAsync("http://localhost/test",default));await Assert.ThrowsAsync<Spilton.Api.Web.WebException>(()=>r.ReadAsync("file:///secret",default));}
 [Theory][InlineData("https://user:password@1.1.1.1/test")][InlineData("https://1.1.1.1:444/test")]
 public async Task Reader_rejects_credentials_and_unsafe_ports(string url)=>await Assert.ThrowsAsync<Spilton.Api.Web.WebException>(()=>Reader(new StubHandler("text/plain","unused")).ReadAsync(url,default));
 [Fact] public async Task Reader_revalidates_and_blocks_private_redirect_targets()=>await Assert.ThrowsAsync<Spilton.Api.Web.WebException>(()=>Reader(new RedirectHandler()).ReadAsync("https://1.1.1.1/start",default));
 [Fact] public async Task Search_returns_structured_provenance(){var json="""{"results":[{"title":"Official docs","url":"https://example.com/docs","content":"Current release notes"}]}""";var p=new TavilyWebSearchProvider(new(){TavilyApiKey="configured-test-key"},new HttpClient(new StubHandler("application/json",json)),NullLogger<TavilyWebSearchProvider>.Instance);var x=Assert.Single(await p.SearchAsync("latest release",3,default));Assert.Equal("Tavily",x.Provider);Assert.Equal(1,x.Rank);Assert.Equal("https://example.com/docs",x.Url);}
 [Fact] public async Task Empty_search_is_not_fabricated(){var p=new TavilyWebSearchProvider(new(){TavilyApiKey="configured-test-key"},new HttpClient(new StubHandler("application/json","{\"results\":[]}")),NullLogger<TavilyWebSearchProvider>.Instance);Assert.Empty(await p.SearchAsync("nothing",3,default));}
 [Fact] public async Task Rate_limit_is_honest(){var p=new TavilyWebSearchProvider(new(){TavilyApiKey="configured-test-key"},new HttpClient(new StubHandler("application/json","{}",HttpStatusCode.TooManyRequests)),NullLogger<TavilyWebSearchProvider>.Instance);var e=await Assert.ThrowsAsync<Spilton.Api.Web.WebException>(()=>p.SearchAsync("query",3,default));Assert.Contains("rate limited",e.Message);}
 [Fact] public async Task Provider_failure_is_honest(){var p=new TavilyWebSearchProvider(new(){TavilyApiKey="configured-test-key"},new HttpClient(new StubHandler("application/json","{}",HttpStatusCode.BadGateway)),NullLogger<TavilyWebSearchProvider>.Instance);var e=await Assert.ThrowsAsync<Spilton.Api.Web.WebException>(()=>p.SearchAsync("query",3,default));Assert.Contains("could not complete",e.Message);}
 [Fact] public void Router_uses_web_only_for_freshness(){var r=new Spilton.Api.Chat.CapabilityRouter();Assert.Equal(Spilton.Api.Chat.Capability.RESEARCH,r.Route("latest stable .NET release",[],false,false,"quick").Capability);Assert.Equal(Spilton.Api.Chat.Capability.CODING,r.Route("write a Python palindrome",[],false,false,"quick").Capability);}
 static SafeWebPageReader Reader(HttpMessageHandler h)=>new(new(),new HttpClient(h),NullLogger<SafeWebPageReader>.Instance);
 sealed class StubHandler(string type,string body,HttpStatusCode status=HttpStatusCode.OK):HttpMessageHandler{protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,CancellationToken cancellationToken)=>Task.FromResult(new HttpResponseMessage(status){Content=new StringContent(body,Encoding.UTF8,type)});}
 sealed class RedirectHandler:HttpMessageHandler{protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,CancellationToken cancellationToken){var response=new HttpResponseMessage(HttpStatusCode.Redirect);response.Headers.Location=new Uri("http://127.0.0.1/private");return Task.FromResult(response);}}
}

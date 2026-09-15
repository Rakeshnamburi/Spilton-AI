using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Spilton.Api.Data;
using Spilton.Api.Mocks;
namespace Spilton.Api.Tests;
public sealed class TestClock:TimeProvider{long ticks=DateTimeOffset.UtcNow.UtcTicks;public override DateTimeOffset GetUtcNow()=>new(Interlocked.Read(ref ticks),TimeSpan.Zero);public void Advance(int seconds)=>Interlocked.Add(ref ticks,TimeSpan.FromSeconds(seconds).Ticks);}
public sealed class MockFactory:WebApplicationFactory<Program>
{
 public TestClock Clock{get;}=new();protected override void ConfigureWebHost(IWebHostBuilder b){b.UseEnvironment("Development");b.ConfigureServices(s=>{s.RemoveAll<TimeProvider>();s.AddSingleton<TimeProvider>(Clock);s.RemoveAll<IHostedService>();});}
}
public class MockTests:IClassFixture<MockFactory>
{
 readonly MockFactory factory;public MockTests(MockFactory factory)=>this.factory=factory;
 async Task<HttpClient> User(){var c=factory.CreateClient();var r=await c.PostAsJsonAsync("/api/auth/register",new{name="Mock integration",email=$"mock-{Guid.NewGuid():N}@example.test",password=Guid.NewGuid().ToString("N")});Assert.Equal(HttpStatusCode.Created,r.StatusCode);c.DefaultRequestHeaders.Authorization=new("Bearer",(await r.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("accessToken").GetString());return c;}
 static async Task<JsonElement> Json(HttpResponseMessage r){Assert.True(r.IsSuccessStatusCode,await r.Content.ReadAsStringAsync());return await r.Content.ReadFromJsonAsync<JsonElement>();}
 async Task<Guid> Start(HttpClient c,Guid? test=null){var r=await Json(await c.PostAsJsonAsync($"/api/mocks/tests/{test??MockSeed.Id("starter")}/attempts",new{startKey=Guid.NewGuid()}));return r.GetProperty("id").GetGuid();}
 [Fact]public async Task Scoring_ownership_answer_key_protection_and_idempotency(){using var a=await User();using var b=await User();var id=await Start(a);var path=$"/api/mocks/attempts/{id}";var before=await Json(await a.GetAsync(path));Assert.Equal("ACTIVE",before.GetProperty("status").GetString());Assert.Equal(JsonValueKind.Null,before.GetProperty("result").ValueKind);Assert.All(before.GetProperty("questions").EnumerateArray(),q=>Assert.Equal(JsonValueKind.Null,q.GetProperty("review").ValueKind));
 Assert.Equal(HttpStatusCode.NotFound,(await b.GetAsync(path)).StatusCode);Assert.Equal(HttpStatusCode.NotFound,(await b.PostAsJsonAsync(path+"/submit",new{})).StatusCode);
 var first=MockSeed.Id("question0");var second=MockSeed.Id("question1");
 Assert.Equal(HttpStatusCode.BadRequest,(await a.PatchAsJsonAsync(path+$"/answers/{first}",new{optionId=MockSeed.Id("question1-option0"),version=0})).StatusCode);
 Assert.Equal(HttpStatusCode.BadRequest,(await a.PatchAsJsonAsync(path+$"/answers/{Guid.NewGuid()}",new{optionId=Guid.NewGuid(),version=0})).StatusCode);
 Assert.Equal(HttpStatusCode.NotFound,(await b.PatchAsJsonAsync(path+$"/answers/{first}",new{optionId=MockSeed.Id("question0-option1"),version=0})).StatusCode);
 await Json(await a.PatchAsJsonAsync(path+$"/answers/{first}",new{optionId=MockSeed.Id("question0-option1"),version=0,markedForReview=true,score=9999}));await Json(await a.PatchAsJsonAsync(path+$"/answers/{second}",new{optionId=MockSeed.Id("question1-option1"),version=0}));
 Assert.Equal(HttpStatusCode.Conflict,(await a.PatchAsJsonAsync(path+$"/answers/{first}",new{optionId=MockSeed.Id("question0-option0"),version=0})).StatusCode);
 var reload=await Json(await a.GetAsync(path));Assert.Equal(before.GetProperty("expiresAt").GetString(),reload.GetProperty("expiresAt").GetString());Assert.True(reload.GetProperty("questions")[0].GetProperty("answer").GetProperty("markedForReview").GetBoolean());
 var result=await Json(await a.PostAsJsonAsync(path+"/submit",new{finalScore=99999,correct=16}));var score=result.GetProperty("result");Assert.Equal(1,score.GetProperty("correct").GetInt32());Assert.Equal(1,score.GetProperty("incorrect").GetInt32());Assert.Equal(14,score.GetProperty("unattempted").GetInt32());Assert.Equal(1.5m,score.GetProperty("finalScore").GetDecimal());Assert.Equal(50,score.GetProperty("accuracyPercent").GetDecimal());
 Assert.Equal(HttpStatusCode.Conflict,(await a.PatchAsJsonAsync(path+$"/answers/{first}",new{version=1})).StatusCode);var again=await Json(await a.PostAsJsonAsync(path+"/submit",new{}));Assert.Equal(result.GetProperty("completedAt").GetDateTimeOffset().ToUnixTimeMilliseconds(),again.GetProperty("completedAt").GetDateTimeOffset().ToUnixTimeMilliseconds());
 using var scope=factory.Services.CreateScope();var db=scope.ServiceProvider.GetRequiredService<AppDbContext>();var attempt=await db.MockAttempts.SingleAsync(x=>x.Id==id);var progress=await db.UserTopicProgress.SingleAsync(x=>x.UserExamProfileId==attempt.UserExamProfileId&&x.TopicId==Spilton.Api.Preparation.PreparationModel.Id("SSC CGLTier 1Quantitative AptitudePercentage"));Assert.Equal(2,progress.QuestionsAttempted);Assert.Equal(1,progress.QuestionsCorrect);
 var tutor=await Json(await a.PostAsJsonAsync(path+"/tutor",new{questionId=first,style="telugu"}));Assert.Contains("Telugu",tutor.GetProperty("prompt").GetString());Assert.Contains("120",tutor.GetProperty("prompt").GetString());
 }
 [Fact]public async Task Expiration_is_server_enforced_without_waiting_or_resetting_timer(){using var a=await User();var cat=await Json(await a.GetAsync("/api/mocks/catalog"));var stage=cat.GetProperty("tests")[0].GetProperty("examStageId").GetGuid();var custom=await Json(await a.PostAsJsonAsync("/api/mocks/practice",new{examStageId=stage,count=1,durationSeconds=15,marksCorrect=3,negativeMarks=1}));var id=await Start(a,custom.GetProperty("id").GetGuid());var path=$"/api/mocks/attempts/{id}";var initial=await Json(await a.GetAsync(path));factory.Clock.Advance(16);var expired=await Json(await a.GetAsync(path));Assert.Equal("SUBMITTED",expired.GetProperty("status").GetString());Assert.Equal("EXPIRED",expired.GetProperty("completionReason").GetString());Assert.Equal(initial.GetProperty("expiresAt").GetString(),expired.GetProperty("completedAt").GetString());Assert.Equal(15,expired.GetProperty("result").GetProperty("timeTakenSeconds").GetInt32());Assert.Equal(HttpStatusCode.Conflict,(await a.PatchAsJsonAsync(path+"/answers/"+initial.GetProperty("questions")[0].GetProperty("id").GetGuid(),new{version=0})).StatusCode);}
 [Fact]public async Task Counts_and_start_retries_use_real_configuration(){using var a=await User();var cat=await Json(await a.GetAsync("/api/mocks/catalog"));var stage=cat.GetProperty("tests")[0].GetProperty("examStageId").GetGuid();Assert.Equal(HttpStatusCode.BadRequest,(await a.PostAsJsonAsync("/api/mocks/practice",new{examStageId=stage,count=50})).StatusCode);var key=Guid.NewGuid();var path=$"/api/mocks/tests/{MockSeed.Id("starter")}/attempts";var first=await Json(await a.PostAsJsonAsync(path,new{startKey=key}));var second=await Json(await a.PostAsJsonAsync(path,new{startKey=key}));Assert.Equal(first.GetProperty("id").GetGuid(),second.GetProperty("id").GetGuid());var id=first.GetProperty("id").GetGuid();await Json(await a.PostAsJsonAsync($"/api/mocks/attempts/{id}/submit",new{}));}
}

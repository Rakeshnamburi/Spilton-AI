using Microsoft.Extensions.DependencyInjection;
using Spilton.Api.Data;
using Spilton.Api.Preparation;
namespace Spilton.Api.Tests;
public class PreparationTests:IClassFixture<ChatFactory>
{
 readonly ChatFactory factory;public PreparationTests(ChatFactory factory)=>this.factory=factory;
 [Fact]public async Task Context_is_bounded_owned_scoped_and_excludes_deactivated_memory(){
 using var scope=factory.Services.CreateScope();var db=scope.ServiceProvider.GetRequiredService<AppDbContext>();
 var a=new User{Name="Context test",Email=Guid.NewGuid()+"@example.test",NormalizedEmail=Guid.NewGuid().ToString(),PasswordHash="TEST_ONLY_UNUSABLE_HASH"};
 var b=new User{Name="Other test",Email=Guid.NewGuid()+"@example.test",NormalizedEmail=Guid.NewGuid().ToString(),PasswordHash="TEST_ONLY_UNUSABLE_HASH"};
 var s=new Space{User=a,Name="SSC"};var p=new Space{User=a,Name="Python"};
 db.AddRange(a,b,s,p);var memory=new Memory{User=a,Space=s,Type="WeakArea",Content="SSC_PERCENTAGE_PRIVATE"};db.AddRange(memory,new Memory{User=a,Space=p,Content="PYTHON_DECORATORS"},new Memory{User=b,Content="OTHER_USER_PRIVATE"},new Memory{User=a,Type="Preference",Content="Prefer Telugu"});await db.SaveChangesAsync();
 var builder=scope.ServiceProvider.GetRequiredService<PersonalizedContext>();
 var first=(await builder.Build(a.Id,s.Id,default))!.Content;Assert.Contains("SSC_PERCENTAGE_PRIVATE",first);Assert.Contains("Prefer Telugu",first);Assert.DoesNotContain("PYTHON_DECORATORS",first);Assert.DoesNotContain("OTHER_USER_PRIVATE",first);Assert.True(first.Length<9000);
 var second=(await builder.Build(a.Id,p.Id,default))!.Content;Assert.Contains("PYTHON_DECORATORS",second);Assert.DoesNotContain("SSC_PERCENTAGE_PRIVATE",second);
 memory.IsActive=false;await db.SaveChangesAsync();Assert.DoesNotContain("SSC_PERCENTAGE_PRIVATE",(await builder.Build(a.Id,s.Id,default))!.Content);
 db.Memories.RemoveRange(db.Memories.Where(m=>m.UserId==a.Id||m.UserId==b.Id));db.Spaces.RemoveRange(s,p);db.Users.RemoveRange(a,b);await db.SaveChangesAsync();
 }
}

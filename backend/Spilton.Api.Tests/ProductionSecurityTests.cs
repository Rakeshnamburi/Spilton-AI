using Spilton.Api.Security;
using Spilton.Api.Documents;

namespace Spilton.Api.Tests;
public sealed class ProductionSecurityTests
{
    [Theory][InlineData("notes.exe.pdf")][InlineData("../notes.pdf")][InlineData("notes.txt:secret")][InlineData("notes.pdf.")]
    public void Unsafe_upload_names_fail(string name) => Assert.False(UploadPolicy.SafeName(name));
    [Fact] public void Ordinary_versioned_document_name_is_allowed() => Assert.True(UploadPolicy.SafeName("notes.v2.pdf"));
    [Fact] public async Task Budget_is_atomic_and_partitions_users()
    {
        var store=new LocalResourceBudgetStore(TimeProvider.System);
        var results=await Task.WhenAll(Enumerable.Range(0,30).Select(async _=>await store.Acquire("a:generation",10,TimeSpan.FromMinutes(1),default)));
        Assert.Equal(10,results.Count(r=>r.Allowed));
        Assert.True((await store.Acquire("b:generation",10,TimeSpan.FromMinutes(1),default)).Allowed);
    }
    [Fact] public async Task Existing_file_is_not_deleted_on_create_collision()
    {
        var root=Path.Combine(Path.GetTempPath(),"spilton-storage-"+Guid.NewGuid().ToString("N"));
        var name=Guid.NewGuid().ToString("N")+".txt";
        var storage=new LocalFileStorage(new(){StoragePath=root});
        await storage.SaveAsync(name,new MemoryStream([65]),default);
        await Assert.ThrowsAsync<IOException>(()=>storage.SaveAsync(name,new MemoryStream([66]),default));
        using(var stream=storage.Open(name))Assert.Equal(65,stream.ReadByte());
        storage.Delete(name);Directory.Delete(root);
    }
}

using Spilton.Api.Documents;
namespace Spilton.Api.Tests;
public class DocumentTests
{
    private static RagSettings Settings()=>new(){ModelPath=Environment.GetEnvironmentVariable("Rag__ModelPath")??throw new Exception("Load scripts/dev-environment.ps1 first.")};
    [Fact] public async Task Real_local_embeddings_capture_semantic_similarity()
    {
        var settings=Settings();using var provider=new LocalEmbeddingProvider(settings,new MiniLmTokenizer(settings));
        var vectors=await provider.EmbedAsync(["A person is programming in Python.","A developer writes Python code.","Tomatoes need water and sunlight."],CancellationToken.None);
        Assert.All(vectors,v=>{Assert.Equal(384,v.Length);Assert.InRange(v.Sum(x=>x*x),.999f,1.001f);});
        double Dot(float[] a,float[] b)=>a.Zip(b).Sum(x=>(double)x.First*x.Second);
        Assert.True(Dot(vectors[0],vectors[1])>Dot(vectors[0],vectors[2])+.15);
    }
    [Fact] public void Tokenizer_uses_published_uncased_vocabulary()
    {var tokenizer=new MiniLmTokenizer(Settings());Assert.Equal(new[]{7592,1010,2088,999},tokenizer.Tokens("Héllo, WORLD!"));Assert.Equal(tokenizer.Tokens("hello world"),tokenizer.Tokens("hello\u0000 world"));}
    [Fact]public void Chunks_bound_tokens_preserve_pages_and_overlap()
    {var settings=Settings();settings.ChunkTokens=60;settings.OverlapTokens=10;var tokenizer=new MiniLmTokenizer(settings);var chunker=new DocumentChunker(settings,tokenizer);
        var result=chunker.Split(new([new(string.Join(" ",Enumerable.Repeat("Scholarship applicants need a degree and must submit their application before the deadline.",50)),1,"Eligibility"),new("The fee is 250 rupees.",2,"Fees")],2));
        Assert.True(result.Count>2);Assert.All(result,c=>Assert.InRange(c.Tokens,1,60));Assert.Contains(result,c=>c.Page==2&&c.Section=="Fees"&&c.Content.Contains("250"));}
    [Fact]public async Task Storage_rejects_traversal_without_writing()
    {var storage=new LocalFileStorage(new(){StoragePath=Path.Combine(Path.GetTempPath(),"spilton-storage-test")});await Assert.ThrowsAsync<DocumentException>(()=>storage.SaveAsync("../escape.txt",new MemoryStream([65]),CancellationToken.None));}
    [Fact]public void Binary_text_is_not_processed_as_success()
    {var extractor=new DocumentExtractor();Assert.Throws<DocumentException>(()=>extractor.Extract(new MemoryStream([65,0,66]),".txt",CancellationToken.None));}
}

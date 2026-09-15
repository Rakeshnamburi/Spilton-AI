using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
namespace Spilton.Api.Documents;

public interface IEmbeddingProvider
{
    string Model { get; }
    int Dimensions { get; }
    Task<float[][]> EmbedAsync(IReadOnlyList<string> texts, CancellationToken ct);
}
// BERT uncased basic tokenizer + greedy WordPiece using the model's pinned vocabulary.
public sealed class MiniLmTokenizer(RagSettings settings)
{
    private readonly Lazy<Dictionary<string,int>> vocabulary = new(() => File.ReadAllLines(Path.Combine(settings.ModelPath,"vocab.txt")).Select((s,i)=>(s,i)).ToDictionary(x=>x.s,x=>x.i));
    public int[] Tokens(string text)
    {
        var clean=new StringBuilder();
        foreach(var rune in text.Normalize(NormalizationForm.FormD).EnumerateRunes()) {
            var category=Rune.GetUnicodeCategory(rune);
            if(category==UnicodeCategory.NonSpacingMark || (Rune.IsControl(rune)&&!Rune.IsWhiteSpace(rune)) || rune.Value==0xfffd)continue;
            if(Rune.IsWhiteSpace(rune)){clean.Append(' ');continue;}
            var punct=Rune.IsPunctuation(rune)||(rune.Value is >=33 and <=47 or >=58 and <=64 or >=91 and <=96 or >=123 and <=126);
            var cjk=rune.Value is >=0x4e00 and <=0x9fff or >=0x3400 and <=0x4dbf or >=0x20000 and <=0x2fa1f;
            if(punct||cjk)clean.Append(' ');clean.Append(Rune.ToLowerInvariant(rune));if(punct||cjk)clean.Append(' ');
        }
        var result=new List<int>(); var vocab=vocabulary.Value;
        foreach(var word in clean.ToString().Split(' ',StringSplitOptions.RemoveEmptyEntries)) {
            if(word.Length>100){result.Add(100);continue;}
            var pieces=new List<int>();var start=0;
            while(start<word.Length){var end=word.Length;int found=-1;while(end>start){if(vocab.TryGetValue((start==0?"":"##")+word[start..end],out found))break;found=-1;end--;}
                if(found<0){pieces.Clear();pieces.Add(100);break;}pieces.Add(found);start=end;}
            result.AddRange(pieces);
        }
        return result.ToArray();
    }
}
public sealed class LocalEmbeddingProvider(RagSettings settings, MiniLmTokenizer tokenizer) : IEmbeddingProvider, IDisposable
{
    public string Model => "sentence-transformers/all-MiniLM-L6-v2@1110a243";
    public int Dimensions => 384;
    private readonly SemaphoreSlim gate=new(1,1);
    private InferenceSession? session;
    public async Task<float[][]> EmbedAsync(IReadOnlyList<string> texts,CancellationToken ct)
    {
        await gate.WaitAsync(ct);
        try {
            if(session is null){var options=new Microsoft.ML.OnnxRuntime.SessionOptions{IntraOpNumThreads=2,InterOpNumThreads=1};session=new InferenceSession(Path.Combine(settings.ModelPath,"model.onnx"),options);options.Dispose();}
            var all=new List<float[]>();
            foreach(var batch in texts.Chunk(4)){
                ct.ThrowIfCancellationRequested();
                var ids=batch.Select(t=>new[]{101}.Concat(tokenizer.Tokens(t).Take(254)).Append(102).Select(i=>(long)i).ToArray()).ToArray();
                var length=ids.Max(x=>x.Length);var input=new DenseTensor<long>(new[]{batch.Length,length});var mask=new DenseTensor<long>(new[]{batch.Length,length});var types=new DenseTensor<long>(new[]{batch.Length,length});
                for(var b=0;b<batch.Length;b++)for(var j=0;j<ids[b].Length;j++){input[b,j]=ids[b][j];mask[b,j]=1;}
                using var output=session.Run(new[]{NamedOnnxValue.CreateFromTensor("input_ids",input),NamedOnnxValue.CreateFromTensor("attention_mask",mask),NamedOnnxValue.CreateFromTensor("token_type_ids",types)});
                var tensor=output.First().AsTensor<float>();
                if(tensor.Dimensions.Length!=3||tensor.Dimensions[2]!=Dimensions)throw new DocumentException("The embedding model has an unexpected output shape.");
                for(var b=0;b<batch.Length;b++){var vector=new float[Dimensions];for(var j=0;j<ids[b].Length;j++)for(var d=0;d<Dimensions;d++)vector[d]+=tensor[b,j,d]/ids[b].Length;
                    var norm=Math.Sqrt(vector.Sum(x=>(double)x*x));if(norm<1e-10)throw new DocumentException("The embedding model returned an empty vector.");for(var d=0;d<Dimensions;d++)vector[d]/=(float)norm;all.Add(vector);}
            }
            return all.ToArray();
        } finally {gate.Release();}
    }
    public void Dispose(){session?.Dispose();gate.Dispose();}
}

using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using UglyToad.PdfPig;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;
namespace Spilton.Api.Documents;
public sealed record TextPart(string Content,int? Page,string? Section);
public sealed record ExtractionResult(List<TextPart> Parts,int? PageCount);
public sealed class DocumentExtractor
{
    public ExtractionResult Extract(Stream stream,string extension,CancellationToken ct)
    {
        var parts=new List<TextPart>();int? pages=null;
        if(extension==".pdf") {
            using var pdf=PdfDocument.Open(stream);pages=pdf.NumberOfPages;if(pages>100)throw new DocumentException("PDFs are limited to 100 pages.");
            foreach(var page in pdf.GetPages()){ct.ThrowIfCancellationRequested();parts.Add(new(ContentOrderTextExtractor.GetText(page),page.Number,null));}
        } else if(extension==".docx") {
            using var zip=new ZipArchive(stream,ZipArchiveMode.Read,true);
            if(zip.Entries.Count>1000||zip.Entries.Sum(e=>e.Length)>10*1024*1024)throw new DocumentException("The DOCX archive expands beyond the safe processing limit.");
            if(zip.Entries.Any(e=>e.FullName.EndsWith("vbaProject.bin",StringComparison.OrdinalIgnoreCase)))throw new DocumentException("Macro-enabled documents are not supported.");
            var entry=zip.GetEntry("word/document.xml")??throw new DocumentException("This is not a valid DOCX document.");
            using var xml=XmlReader.Create(entry.Open(),new XmlReaderSettings{DtdProcessing=DtdProcessing.Prohibit,XmlResolver=null,MaxCharactersInDocument=10*1024*1024});
            var doc=XDocument.Load(xml);XNamespace w="http://schemas.openxmlformats.org/wordprocessingml/2006/main";string? heading=null;
            foreach(var p in doc.Descendants(w+"p")){ct.ThrowIfCancellationRequested();var content=string.Concat(p.Descendants(w+"t").Select(t=>t.Value));var style=p.Descendants(w+"pStyle").FirstOrDefault()?.Attribute(w+"val")?.Value;
                if(style?.StartsWith("Heading",StringComparison.OrdinalIgnoreCase)==true)heading=content[..Math.Min(content.Length,200)];parts.Add(new(content,null,heading));}
        } else {
            using var reader=new StreamReader(stream,new UTF8Encoding(false,true),true,4096,true);var text=reader.ReadToEnd();if(text.Contains('\0'))throw new DocumentException("TXT files must contain readable text.");parts.Add(new(text,null,null));
        }
        if(parts.Sum(p=>p.Content.Length)>250000)throw new DocumentException("The document exceeds 250,000 extracted characters.");
        parts=parts.Select(p=>p with{Content=Clean(p.Content)}).Where(p=>!string.IsNullOrWhiteSpace(p.Content)).ToList();
        if(parts.Count==0)throw new DocumentException("No extractable text was found. Scanned PDFs need OCR, which is not available yet.");
        return new(parts,pages);
    }
    public static string Clean(string text)=>Regex.Replace(Regex.Replace(text.Replace("\r\n","\n").Replace('\r','\n').Replace("\u00ad",""),@"[^\S\n]+"," "),@"\n[ \t]*\n(?:[ \t]*\n)+","\n\n").Trim();
}
public sealed record ChunkText(string Content,int? Page,string? Section,int Tokens);
public sealed class DocumentChunker(RagSettings settings,MiniLmTokenizer tokenizer)
{
    public List<ChunkText> Split(ExtractionResult extraction)
    {
        var max=Math.Clamp(settings.ChunkTokens,40,240);var overlap=Math.Clamp(settings.OverlapTokens,0,max/3);var output=new List<ChunkText>();
        foreach(var group in extraction.Parts.GroupBy(p=>(p.Page,p.Section))) {
            var buffer="";
            void Emit(){if(buffer.Length>0)output.Add(new(buffer,group.Key.Page,group.Key.Section,tokenizer.Tokens(buffer).Length));}
            foreach(var paragraph in group.SelectMany(p=>Regex.Split(p.Content,@"\n\s*\n"))) {
                // Paragraphs first, then sentence boundaries; word-level fallback only for long sentences.
                foreach(var sentence in Regex.Split(paragraph,@"(?<=[.!?])\s+|\n")) {
                    foreach(var piece in Pieces(sentence,max)) {
                        var joined=buffer.Length==0?piece:buffer+"\n"+piece;
                        if(tokenizer.Tokens(joined).Length>max){Emit();var words=buffer.Split(' ',StringSplitOptions.RemoveEmptyEntries);var tail="";for(var i=words.Length-1;i>=0;i--){var candidate=words[i]+(tail.Length==0?"":" "+tail);if(tokenizer.Tokens(candidate).Length>overlap)break;tail=candidate;}buffer=tail;
                            if(tokenizer.Tokens(buffer+"\n"+piece).Length>max)buffer="";}
                        buffer=buffer.Length==0?piece:buffer+"\n"+piece;
                        if(output.Count>RagSettings.MaxChunks)throw new DocumentException("The document exceeds the 500-chunk limit.");
                    }
                }
            } Emit();
        }
        if(output.Count is 0 or >RagSettings.MaxChunks)throw new DocumentException("The document has no usable chunks or exceeds the 500-chunk limit.");
        if(output.Any(c=>c.Content.Length>6000||c.Tokens>max))throw new DocumentException("The document contains a text segment too large to process safely.");return output;
    }
    private IEnumerable<string> Pieces(string text,int max){var buffer="";foreach(var word in text.Split(' ',StringSplitOptions.RemoveEmptyEntries)){if(tokenizer.Tokens(buffer+" "+word).Length>max&&buffer.Length>0){yield return buffer;buffer="";}buffer+= (buffer.Length==0?"":" ")+word;}if(buffer.Length>0)yield return buffer;}
}

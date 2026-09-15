namespace Spilton.Api.Documents;
public interface IFileStorage
{
    Task SaveAsync(string name, Stream input,CancellationToken ct);
    Stream Open(string name);
    void Delete(string name);
}
public sealed class LocalFileStorage(RagSettings settings) : IFileStorage
{
    private string Resolve(string name){if(!System.Text.RegularExpressions.Regex.IsMatch(name,@"^[a-f0-9]{32}\.(pdf|txt|docx)$"))throw new DocumentException("Invalid storage identifier.");var root=Path.GetFullPath(settings.StoragePath);for(var d=new DirectoryInfo(root);d is not null;d=d.Parent)if(d.Exists&&(d.Attributes&FileAttributes.ReparsePoint)!=0)throw new DocumentException("Storage links are not allowed.");var path=Path.Combine(root,name);if(File.Exists(path)&&(File.GetAttributes(path)&FileAttributes.ReparsePoint)!=0)throw new DocumentException("Storage links are not allowed.");return path;}
    public async Task SaveAsync(string name,Stream input,CancellationToken ct){Directory.CreateDirectory(settings.StoragePath);var path=Resolve(name);var created=false;try{await using var output=new FileStream(path,FileMode.CreateNew,FileAccess.Write,FileShare.None,81920,true);created=true;var bytes=new byte[81920];long total=0;int count;while((count=await input.ReadAsync(bytes,ct))>0){total+=count;if(total>RagSettings.MaxFileBytes)throw new DocumentException("Files must be 5 MB or smaller.");await output.WriteAsync(bytes.AsMemory(0,count),ct);}}catch{if(created)File.Delete(path);throw;}}
    public Stream Open(string name)=>File.OpenRead(Resolve(name));
    public void Delete(string name)=>File.Delete(Resolve(name));
}

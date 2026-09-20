using System.Net;
using Amazon.S3;
using Amazon.S3.Model;

namespace Spilton.Api.Documents;
public interface IFileStorage
{
    string Provider { get; }
    Task SaveAsync(string name, Stream input,CancellationToken ct);
    Task<Stream> OpenReadAsync(string name,CancellationToken ct);
    Task DeleteAsync(string name,CancellationToken ct);
}
public sealed class LocalFileStorage(RagSettings settings) : IFileStorage
{
    public string Provider => "Local";
    private string Resolve(string name){if(!System.Text.RegularExpressions.Regex.IsMatch(name,@"^[a-f0-9]{32}\.(pdf|txt|docx)$"))throw new DocumentException("Invalid storage identifier.");var root=Path.GetFullPath(settings.StoragePath);for(var d=new DirectoryInfo(root);d is not null;d=d.Parent)if(d.Exists&&(d.Attributes&FileAttributes.ReparsePoint)!=0)throw new DocumentException("Storage links are not allowed.");var path=Path.Combine(root,name);if(File.Exists(path)&&(File.GetAttributes(path)&FileAttributes.ReparsePoint)!=0)throw new DocumentException("Storage links are not allowed.");return path;}
    public async Task SaveAsync(string name,Stream input,CancellationToken ct){Directory.CreateDirectory(settings.StoragePath);var path=Resolve(name);var created=false;try{await using var output=new FileStream(path,FileMode.CreateNew,FileAccess.Write,FileShare.None,81920,true);created=true;var bytes=new byte[81920];long total=0;int count;while((count=await input.ReadAsync(bytes,ct))>0){total+=count;if(total>RagSettings.MaxFileBytes)throw new DocumentException("Files must be 5 MB or smaller.");await output.WriteAsync(bytes.AsMemory(0,count),ct);}}catch{if(created)File.Delete(path);throw;}}
    public Task<Stream> OpenReadAsync(string name,CancellationToken ct)=>Task.FromResult<Stream>(File.OpenRead(Resolve(name)));
    public Task DeleteAsync(string name,CancellationToken ct){File.Delete(Resolve(name));return Task.CompletedTask;}
}

public sealed class S3FileStorage(IAmazonS3 s3,StorageSettings settings) : IFileStorage
{
    public string Provider => "S3";
    private string Key(string name)
    {
        if(!System.Text.RegularExpressions.Regex.IsMatch(name,@"^[a-f0-9]{32}\.(pdf|txt|docx)$"))throw new DocumentException("Invalid storage identifier.");
        var prefix=settings.KeyPrefix.Trim().Trim('/');
        if(prefix.Length>120||prefix.Contains("..",StringComparison.Ordinal)||prefix.Contains('\\'))throw new DocumentException("Invalid storage prefix.");
        return string.IsNullOrEmpty(prefix)?name:$"{prefix}/{name}";
    }
    public async Task SaveAsync(string name,Stream input,CancellationToken ct)
    {
        using var limited=new MemoryStream();var buffer=new byte[81920];long total=0;int read;
        while((read=await input.ReadAsync(buffer,ct))>0){total+=read;if(total>RagSettings.MaxFileBytes)throw new DocumentException("Files must be 5 MB or smaller.");await limited.WriteAsync(buffer.AsMemory(0,read),ct);}
        limited.Position=0;
        try{await s3.PutObjectAsync(new PutObjectRequest{BucketName=settings.Bucket,Key=Key(name),InputStream=limited,AutoCloseStream=false,IfNoneMatch="*"},ct);}
        catch(AmazonS3Exception ex) when(ex.StatusCode is HttpStatusCode.PreconditionFailed or HttpStatusCode.Conflict){throw new IOException("The storage identifier already exists.",ex);}
    }
    public async Task<Stream> OpenReadAsync(string name,CancellationToken ct)
    {
        using var response=await s3.GetObjectAsync(new GetObjectRequest{BucketName=settings.Bucket,Key=Key(name)},ct);
        if(response.ContentLength>RagSettings.MaxFileBytes)throw new DocumentException("Stored file exceeds the allowed size.");
        var output=new MemoryStream((int)Math.Max(0,response.ContentLength));var buffer=new byte[81920];long total=0;int read;
        while((read=await response.ResponseStream.ReadAsync(buffer,ct))>0){total+=read;if(total>RagSettings.MaxFileBytes){output.Dispose();throw new DocumentException("Stored file exceeds the allowed size.");}await output.WriteAsync(buffer.AsMemory(0,read),ct);}
        output.Position=0;return output;
    }
    public async Task DeleteAsync(string name,CancellationToken ct)
    {
        try{await s3.DeleteObjectAsync(new DeleteObjectRequest{BucketName=settings.Bucket,Key=Key(name)},ct);}
        catch(AmazonS3Exception ex){throw new IOException("Object storage deletion failed.",ex);}
    }
}

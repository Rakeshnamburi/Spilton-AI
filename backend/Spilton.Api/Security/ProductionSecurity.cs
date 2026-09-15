using System.Security.Cryptography;
using System.Text;

namespace Spilton.Api.Security;

public interface ISecretProtector
{
    string Fingerprint(string value);
}
public sealed class SecretProtector:ISecretProtector
{
    public string Fingerprint(string value)=>Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(value)))[..16];
}
public sealed record SecurityCapability(string Name,string Status,string Detail);
public interface IUploadScanner
{
    string Status { get; }
    Task<bool> IsSafe(Stream content,string fileName,CancellationToken ct);
}
// The extension point fails closed when production asks for malware scanning.
// Local document validation remains in the existing ingestion pipeline.
public sealed class UnconfiguredUploadScanner:IUploadScanner
{
    public string Status=>"MALWARE_SCANNER_NOT_CONFIGURED";
    public Task<bool> IsSafe(Stream content,string fileName,CancellationToken ct)=>Task.FromResult(false);
}
public interface IDistributedRateLimitStore { string Status { get; } }
public sealed class LocalRateLimitStore:IDistributedRateLimitStore { public string Status=>"PROCESS_LOCAL_ONLY"; }


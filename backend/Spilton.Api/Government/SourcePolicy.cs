using System.Net;
using System.Security.Cryptography;
using System.Text.Json;
using Spilton.Api.Documents;
namespace Spilton.Api.Government;
public sealed class SourcePolicy(GovernmentSettings settings,IFileStorage storage)
{
 public static bool SafeUrl(string? value){if(string.IsNullOrWhiteSpace(value))return true;if(!Uri.TryCreate(value,UriKind.Absolute,out var u)||u.Scheme!="https"||u.Port!=443||u.UserInfo.Length>0||u.HostNameType!=UriHostNameType.Dns)return false;var h=u.IdnHost.ToLowerInvariant();return h.Contains('.')&&!h.EndsWith(".localhost")&&!h.EndsWith(".local")&&!h.EndsWith(".internal")&&!IPAddress.TryParse(h,out _)&&h.Length<=253;}
 public async Task Apply(GovernmentResource r,Document? doc,CancellationToken ct){
  r.SourceType=doc is null?"UNVERIFIED":"USER_UPLOADED";r.Verification="UNVERIFIED_CONTENT";
  if(doc is null)return;
  await using var file=storage.Open(doc.StoredName);r.SourceHash=Convert.ToHexString(await SHA256.HashDataAsync(file,ct));
  if(string.IsNullOrEmpty(settings.VerifiedSourcesPath)||!File.Exists(settings.VerifiedSourcesPath))return;
  var entries=JsonSerializer.Deserialize<VerifiedSource[]>(await File.ReadAllTextAsync(settings.VerifiedSourcesPath,ct),new JsonSerializerOptions{PropertyNameCaseInsensitive=true})??[];
  var verified=entries.FirstOrDefault(e=>e.Url==r.SourceUrl&&e.Sha256.Equals(r.SourceHash,StringComparison.OrdinalIgnoreCase)&&e.SourceType is "OFFICIAL" or "TRUSTED_SECONDARY");
  if(verified is null)return;r.SourceType=verified.SourceType;r.Organization=verified.Organization;r.VerifiedAt=verified.VerifiedAt;r.Verification="SOURCE_BYTES_VERIFIED";
 }
 public static int Rank(string type)=>type switch{"OFFICIAL"=>100,"TRUSTED_SECONDARY"=>70,"USER_UPLOADED"=>40,_=>10};
}

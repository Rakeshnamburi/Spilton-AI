param([Parameter(Mandatory=$true)][string]$Url,[Parameter(Mandatory=$true)][string]$Organization,[Parameter(Mandatory=$true)][string]$ReviewedOfficialHost)
$ErrorActionPreference='Stop'
# Local administrator workflow only. Review the official portal/link before invoking.
$sourceUri=[Uri]$Url
if($sourceUri.Scheme -ne 'https' -or $sourceUri.Port -ne 443 -or $sourceUri.UserInfo -or $sourceUri.Host -ne $ReviewedOfficialHost -or $sourceUri.HostNameType -ne 'Dns'){throw 'Use the exact reviewed official HTTPS hostname without redirects or credentials.'}
$root=Split-Path $PSScriptRoot -Parent
$directory=Join-Path $root '.local/official-sources'
New-Item -ItemType Directory -Force -Path $directory | Out-Null
$handler=[Net.Http.HttpClientHandler]::new();$handler.AllowAutoRedirect=$false
$client=[Net.Http.HttpClient]::new($handler);$client.Timeout=[TimeSpan]::FromSeconds(60)
try {
 $response=$client.GetAsync($sourceUri,[Net.Http.HttpCompletionOption]::ResponseHeadersRead).GetAwaiter().GetResult()
 if([int]$response.StatusCode -ne 200){throw 'The official source did not return HTTP 200. Redirects are not followed.'}
 if($response.Content.Headers.ContentLength -gt 5242880){throw 'Official PDF exceeds the 5 MB development limit.'}
 $stream=$response.Content.ReadAsStream();$memory=[IO.MemoryStream]::new();$buffer=New-Object byte[] 81920
 while(($count=$stream.Read($buffer,0,$buffer.Length)) -gt 0){if($memory.Length+$count -gt 5242880){throw 'File exceeds 5 MB.'};$memory.Write($buffer,0,$count)}
 $bytes=$memory.ToArray();if($bytes.Length -lt 5 -or [Text.Encoding]::ASCII.GetString($bytes,0,5) -ne '%PDF-'){throw 'Source is not a PDF.'}
 $hash=[Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($bytes));$file=Join-Path $directory ($hash+'.pdf');[IO.File]::WriteAllBytes($file,$bytes)
 $registry=Join-Path $root '.local/verified-sources.json';$entries=@();if(Test-Path -LiteralPath $registry){$entries=@(Get-Content -LiteralPath $registry -Raw | ConvertFrom-Json)}
 $entries=@($entries | Where-Object { $_.Url -ne $Url })+@{Url=$Url;Sha256=$hash;Organization=$Organization;VerifiedAt=[DateTimeOffset]::UtcNow.ToString('o');SourceType='OFFICIAL'}
 ConvertTo-Json -InputObject $entries -Depth 4 | Set-Content -LiteralPath $registry -Encoding utf8
 Write-Output "Verified source snapshot saved: $file"
 Write-Output 'Upload these exact bytes and register the same URL. This confirms origin at retrieval time, not current validity or absence of corrigenda.'
} finally {$client.Dispose();$handler.Dispose()}

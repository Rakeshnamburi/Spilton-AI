$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot 'dev-environment.ps1')
$backupRoot = Join-Path $projectRoot '.local/backups'
New-Item -ItemType Directory -Force -Path $backupRoot | Out-Null
$stamp = Get-Date -Format 'yyyyMMdd-HHmmss'
$target = Join-Path $backupRoot "spilton-$stamp.dump"
& pg_dump --format=custom --file=$target $env:ConnectionStrings__DefaultConnection
if ($LASTEXITCODE -ne 0) { Remove-Item -LiteralPath $target -Force -ErrorAction SilentlyContinue; throw 'pg_dump failed.' }
Write-Output "Backup created at $target"

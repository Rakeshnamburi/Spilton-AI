$ErrorActionPreference = 'Stop'
$projectRoot = Split-Path $PSScriptRoot -Parent
Set-Location $projectRoot
Write-Host 'SPILTON local verification (read-only checks; no database reset)'
if (!(Get-Command dotnet -ErrorAction SilentlyContinue)) { throw 'dotnet is not on PATH. Install .NET 10 SDK.' }
if (-not ((dotnet --list-sdks) -match '^10\.')) { throw '.NET 10 SDK is required.' }
if (!(Get-Command npm.cmd -ErrorAction SilentlyContinue)) { throw 'Node/npm is required.' }
Write-Host " .NET: $(dotnet --version)"
Write-Host " Node: $(node --version)"
Write-Host " npm:  $(npm.cmd --version)"
& (Join-Path $PSScriptRoot 'check-postgres.ps1')
if (!(Test-Path -LiteralPath '.env')) { throw 'Missing .env.' }
if (!(Test-Path -LiteralPath 'frontend/.env.local')) { throw 'Missing frontend/.env.local.' }
. (Join-Path $PSScriptRoot 'dev-environment.ps1')
Write-Host 'Building backend...'
dotnet build backend/Spilton.Api --no-restore --nologo
if ($LASTEXITCODE -ne 0) { throw 'Backend build failed.' }
Write-Host 'Checking backend health (start the backend separately first)...'
try { $health = Invoke-RestMethod 'http://localhost:5081/api/health' -TimeoutSec 10; $health | ConvertTo-Json -Compress } catch { throw 'Backend health check failed. Start scripts/start-backend.ps1 and rerun.' }
Write-Host 'Running backend tests...'
dotnet test backend/Spilton.Api.Tests --no-restore --nologo
if ($LASTEXITCODE -ne 0) { throw 'Backend tests failed.' }
Push-Location frontend
try {
    npm.cmd run lint
    if ($LASTEXITCODE -ne 0) { throw 'Frontend lint failed.' }
    npx.cmd tsc --noEmit
    if ($LASTEXITCODE -ne 0) { throw 'TypeScript check failed.' }
    npm.cmd run build
    if ($LASTEXITCODE -ne 0) { throw 'Frontend production build failed.' }
} finally { Pop-Location }
Write-Host 'Static and database-backed verification completed. Run browser tests separately with the backend and frontend running:'
Write-Host 'npx.cmd playwright test tests/auth.spec.ts tests/chat.spec.ts tests/agent.spec.ts tests/documents.spec.ts tests/preparation.spec.ts tests/government.spec.ts'

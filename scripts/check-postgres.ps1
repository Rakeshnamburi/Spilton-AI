$ErrorActionPreference = 'Stop'
$projectRoot = Split-Path $PSScriptRoot -Parent
$envPath = Join-Path $projectRoot '.env'
if (!(Test-Path -LiteralPath $envPath)) { throw 'Missing .env. Run scripts/setup-local.ps1 first.' }
$values = @{}
foreach ($line in Get-Content -LiteralPath $envPath) {
    if ($line -match '^\s*([^#=]+)=(.*)$') { $values[$Matches[1].Trim()] = $Matches[2].Trim() }
}
$port = if ($values.POSTGRES_PORT) { $values.POSTGRES_PORT } else { '5432' }
$pgBin = Join-Path $projectRoot '.tools/postgresql/pgsql/bin'
$isReady = Join-Path $pgBin 'pg_isready.exe'
if (!(Test-Path -LiteralPath $isReady)) { throw "PostgreSQL tools not found at $pgBin" }
Write-Host "Checking PostgreSQL on 127.0.0.1:$port..."
& $isReady -h 127.0.0.1 -p $port
if ($LASTEXITCODE -ne 0) { throw 'PostgreSQL is stopped or unavailable. Start the existing server, then run this script again.' }
$env:PGPASSWORD = $values.POSTGRES_PASSWORD
$psql = Join-Path $pgBin 'psql.exe'
try {
    & $psql -h 127.0.0.1 -p $port -U $values.POSTGRES_USER -d $values.POSTGRES_DB -v ON_ERROR_STOP=1 -Atc "SELECT current_database(); SELECT extversion FROM pg_extension WHERE extname='vector';"
    if ($LASTEXITCODE -ne 0) { throw 'The application database or credentials are unavailable.' }
} finally { Remove-Item Env:PGPASSWORD -ErrorAction SilentlyContinue }
Write-Host 'PostgreSQL is healthy and the pgvector check completed.'

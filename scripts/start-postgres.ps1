$ErrorActionPreference = 'Stop'
. "$PSScriptRoot/dev-environment.ps1"
$pgBin = Join-Path $projectRoot '.tools/postgresql/pgsql/bin'
$pgData = Join-Path $projectRoot '.local/postgres-data'
$pgLog = Join-Path $projectRoot '.local/postgres.log'
if (!(Test-Path -LiteralPath "$pgBin/pg_ctl.exe")) { throw 'PostgreSQL binaries missing. See README local PostgreSQL installation.' }
& "$pgBin/pg_ctl.exe" -D $pgData status 2>$null
if ($LASTEXITCODE -eq 0) { Write-Host 'Project PostgreSQL is already running.'; exit 0 }
$firstRun = !(Test-Path -LiteralPath "$pgData/PG_VERSION")
$adminPasswordFile = Join-Path $projectRoot '.local/postgres-admin.password'
if ($firstRun) {
    New-Item -ItemType Directory -Force -Path (Join-Path $projectRoot '.local') | Out-Null
    $bytes = New-Object byte[] 48
    $rng = [System.Security.Cryptography.RandomNumberGenerator]::Create()
    try { $rng.GetBytes($bytes) } finally { $rng.Dispose() }
    [Convert]::ToBase64String($bytes) | Set-Content -LiteralPath $adminPasswordFile
    & "$pgBin/initdb.exe" -D $pgData -U spilton_admin --pwfile=$adminPasswordFile --auth=scram-sha-256 --encoding=UTF8 --locale=C
    if ($LASTEXITCODE -ne 0) { throw 'PostgreSQL initialization failed.' }
}
& "$pgBin/pg_ctl.exe" -D $pgData -l $pgLog -o "-h 127.0.0.1 -p $($settings.POSTGRES_PORT)" -w start
if ($LASTEXITCODE -ne 0) { throw 'PostgreSQL startup failed. Inspect .local/postgres.log.' }
# Bootstrap only the project database. Application role has no superuser or role-creation privileges.
$env:PGPASSWORD = (Get-Content -LiteralPath $adminPasswordFile -Raw).Trim()
try {
    $role = $settings.POSTGRES_USER.Replace('"', '""')
    $roleLiteral = $settings.POSTGRES_USER.Replace("'", "''")
    $database = $settings.POSTGRES_DB.Replace('"', '""')
    $dbLiteral = $settings.POSTGRES_DB.Replace("'", "''")
    $password = $settings.POSTGRES_PASSWORD.Replace("'", "''")
    $sql = @"
SELECT 'CREATE ROLE ""$role"" LOGIN NOSUPERUSER NOCREATEDB NOCREATEROLE PASSWORD ' || quote_literal('$password') WHERE NOT EXISTS (SELECT FROM pg_roles WHERE rolname = '$roleLiteral')
\gexec
SELECT 'CREATE DATABASE ""$database"" OWNER ""$role""' WHERE NOT EXISTS (SELECT FROM pg_database WHERE datname = '$dbLiteral')
\gexec
"@
    # PowerShell here-strings preserve quotes literally.
    $sql = $sql.Replace('""', '"')
    $sql | & "$pgBin/psql.exe" -h 127.0.0.1 -p $settings.POSTGRES_PORT -U spilton_admin -d postgres -v ON_ERROR_STOP=1
    if ($LASTEXITCODE -ne 0) { throw 'Project database bootstrap failed.' }
} finally { Remove-Item Env:PGPASSWORD -ErrorAction SilentlyContinue }
& "$pgBin/pg_isready.exe" -h 127.0.0.1 -p $settings.POSTGRES_PORT

$ErrorActionPreference = 'Stop'
$projectRoot = Split-Path $PSScriptRoot -Parent
function New-Secret {
    $bytes = New-Object byte[] 48
    $rng = [System.Security.Cryptography.RandomNumberGenerator]::Create()
    try { $rng.GetBytes($bytes) } finally { $rng.Dispose() }
    return [Convert]::ToBase64String($bytes)
}
$envFile = Join-Path $projectRoot '.env'
if (!(Test-Path -LiteralPath $envFile)) {
    @("POSTGRES_DB=spilton", "POSTGRES_USER=spilton", "POSTGRES_PASSWORD=$(New-Secret)", "POSTGRES_PORT=5432", "JWT_SECRET=$(New-Secret)") | Set-Content -LiteralPath $envFile
}
$frontendEnv = Join-Path $projectRoot 'frontend/.env.local'
if (!(Test-Path -LiteralPath $frontendEnv)) {
    @('API_BASE_URL=http://localhost:5081', 'APP_ORIGIN=http://localhost:3000', 'AUTH_COOKIE_SECURE=false') | Set-Content -LiteralPath $frontendEnv
}
Write-Host 'Local configuration is ready. Existing secrets were preserved.'

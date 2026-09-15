. "$PSScriptRoot/dev-environment.ps1"
Push-Location $projectRoot
try {
    dotnet tool restore
    if ($LASTEXITCODE -ne 0) { throw 'EF tool restore failed.' }
    dotnet ef database update --project backend/Spilton.Api
    if ($LASTEXITCODE -ne 0) { throw 'Database migration failed.' }
} finally { Pop-Location }

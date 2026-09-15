. "$PSScriptRoot/dev-environment.ps1"
Push-Location $projectRoot
try { dotnet run --project backend/Spilton.Api --no-launch-profile } finally { Pop-Location }

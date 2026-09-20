$ErrorActionPreference = 'Stop'
$projectRoot = Split-Path $PSScriptRoot -Parent
$envFile = Join-Path $projectRoot '.env'
if (!(Test-Path -LiteralPath $envFile)) { throw 'Run .\scripts\setup-local.ps1 first.' }
$settings = @{}
foreach ($line in Get-Content -LiteralPath $envFile) {
    if ($line -match '^([^#=]+)=(.*)$') { $settings[$Matches[1].Trim()] = $Matches[2] }
}
$env:ConnectionStrings__DefaultConnection = "Host=localhost;Port=$($settings.POSTGRES_PORT);Database=$($settings.POSTGRES_DB);Username=$($settings.POSTGRES_USER);Password=$($settings.POSTGRES_PASSWORD)"
$env:Jwt__Secret = $settings.JWT_SECRET
# Optional model configuration remains server-side. Only explicitly supplied keys override the environment.
foreach ($entry in @{ MODEL_DEFAULT='Models__Default'; MODEL_BASE_URL='Models__BaseUrl'; MODEL_API_KEY='Models__ApiKey'; MODEL_NAME='Models__Model'; MODEL_PROVIDER_LABEL='Models__ProviderLabel'; MODEL_TIMEOUT_SECONDS='Models__TimeoutSeconds'; MODEL_DEVELOPMENT_ENABLED='Models__DevelopmentEnabled' }.GetEnumerator()) {
    if ($settings.ContainsKey($entry.Key)) { [Environment]::SetEnvironmentVariable($entry.Value, $settings[$entry.Key], 'Process') }
}
if ($settings.ContainsKey('WEB_TAVILY_API_KEY')) { $env:Web__TavilyApiKey = $settings.WEB_TAVILY_API_KEY }
foreach ($entry in @{EMAIL_PROVIDER='Email__Provider';EMAIL_SMTP_HOST='Email__SmtpHost';EMAIL_SMTP_PORT='Email__SmtpPort';EMAIL_SMTP_USERNAME='Email__SmtpUsername';EMAIL_SMTP_PASSWORD='Email__SmtpPassword';EMAIL_FROM_ADDRESS='Email__FromAddress';EMAIL_FROM_NAME='Email__FromName';EMAIL_ENABLE_SSL='Email__EnableSsl'}.GetEnumerator()) {
    if ($settings.ContainsKey($entry.Key)) { [Environment]::SetEnvironmentVariable($entry.Value,$settings[$entry.Key],'Process') }
}
$env:ASPNETCORE_ENVIRONMENT = 'Development'
$env:ASPNETCORE_URLS = 'http://localhost:5081'
$env:DOTNET_CLI_HOME = Join-Path $projectRoot '.local/dotnet-home'
$env:DOTNET_CLI_TELEMETRY_OPTOUT = '1'
$env:Rag__StoragePath = Join-Path $projectRoot '.local/documents'
$env:Rag__ModelPath = Join-Path $projectRoot '.local/models/minilm'
$env:Government__VerifiedSourcesPath = Join-Path $projectRoot '.local/verified-sources.json'
foreach ($entry in @{RAG_CHUNK_TOKENS='Rag__ChunkTokens';RAG_OVERLAP_TOKENS='Rag__OverlapTokens';RAG_TOP_K='Rag__TopK';RAG_MIN_SIMILARITY='Rag__MinimumSimilarity';RAG_DIAGNOSTICS='Rag__Diagnostics'}.GetEnumerator()) {
    if ($settings.ContainsKey($entry.Key)) { [Environment]::SetEnvironmentVariable($entry.Value,$settings[$entry.Key],'Process') }
}
$localDotnet = Join-Path $projectRoot '.tools/dotnet'
if (Test-Path -LiteralPath (Join-Path $localDotnet 'dotnet.exe')) {
    $env:DOTNET_ROOT = $localDotnet
    $env:PATH = "$localDotnet;$env:PATH"
}

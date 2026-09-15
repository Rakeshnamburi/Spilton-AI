$ErrorActionPreference='Stop'
. "$PSScriptRoot/dev-environment.ps1"
$pgRoot=Join-Path $projectRoot '.tools/postgresql/pgsql'
$source=Join-Path $projectRoot '.tools/pgvector/extracted'
if (!(Test-Path "$pgRoot/share/extension/vector.control")) {
    $archive=Join-Path $projectRoot '.tools/pgvector/pg17.zip'
    if (!(Test-Path $archive)) { throw 'Download the pinned PostgreSQL 17 extension archive documented in docs/rag.md first.' }
    if ((Get-FileHash $archive).Hash -ne '420388e9e9f05d92f06d6967ce8772483629b27a66ca9255925fa0fdd445438e') { throw 'pgvector checksum mismatch.' }
    if (!(Test-Path $source)) { Expand-Archive -LiteralPath $archive -DestinationPath $source }
    Copy-Item -LiteralPath "$source/lib/vector.dll" -Destination "$pgRoot/lib/vector.dll"
    Get-ChildItem "$source/share/extension" -File | Where-Object {$_.Name -match '^vector(--[0-9.]+(--[0-9.]+)?)?\.(sql|control)$'} | Copy-Item -Destination "$pgRoot/share/extension"
}
$env:PGPASSWORD=(Get-Content -LiteralPath (Join-Path $projectRoot '.local/postgres-admin.password') -Raw).Trim()
try {
    'CREATE EXTENSION IF NOT EXISTS vector; SELECT extversion FROM pg_extension WHERE extname = ''vector'';' | & "$pgRoot/bin/psql.exe" -h 127.0.0.1 -p $settings.POSTGRES_PORT -U spilton_admin -d $settings.POSTGRES_DB -v ON_ERROR_STOP=1
    if($LASTEXITCODE -ne 0){throw 'Could not enable pgvector.'}
}finally{Remove-Item Env:PGPASSWORD -ErrorAction SilentlyContinue}

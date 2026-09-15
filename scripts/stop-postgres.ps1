$projectRoot = Split-Path $PSScriptRoot -Parent
& "$projectRoot/.tools/postgresql/pgsql/bin/pg_ctl.exe" -D "$projectRoot/.local/postgres-data" -m fast -w stop
if ($LASTEXITCODE -ne 0) { throw 'PostgreSQL stop failed.' }

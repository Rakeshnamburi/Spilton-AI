# Spilton AI — general intelligence foundation

Next.js + React + TypeScript + Tailwind frontend, .NET 10 ASP.NET Core API, PostgreSQL, and the existing JWT/HTTP-only-cookie authentication. Spilton supports general questions, coding assistance, technical tutoring, bounded multi-turn coding context, document-grounded RAG, and government-exam intelligence in one chat. See [general and coding intelligence](docs/general-intelligence.md), [AI design/configuration](docs/ai-system.md), and the historical daily reports.

**Real Groq chat, coding responses, and document-grounded answers have been tested.** The configured default is Groq `openai/gpt-oss-20b`; the separately labelled development provider remains available for repeatable tests. PDF/TXT/DOCX uploads, local MiniLM embeddings, PostgreSQL/pgvector retrieval, source excerpts, Spaces, preparation, notifications, eligibility, PYQ foundations, and mock-test infrastructure are preserved. Bounded local-tool Agent mode works; unbounded autonomous or external actions are unavailable.

The main experience is general-purpose chat. Government Exam Intelligence is an optional specialization and is excluded from unrelated coding/general context. Quick, Think, real-Web/document Research, and bounded local-tool Agent modes share the same conversation interface. See the honest [capability matrix](docs/capability-matrix.md). Vision, audio and arbitrary code execution remain unavailable. See [Web Intelligence](docs/web-intelligence.md) and [Advanced Research](docs/advanced-research.md).

MVP operational guidance is in [release readiness](docs/release-readiness.md) and the infrastructure-neutral [deployment plan](docs/deployment.md). Phase 2 boundaries are documented in [Phase 2 architecture](docs/phase-2-architecture.md), [model routing](docs/model-routing.md), [multimodal](docs/multimodal.md), [coding workspace](docs/coding-workspace.md), and [production security](docs/production-security.md). Use the [final local verification checklist](docs/FINAL_LOCAL_VERIFICATION_CHECKLIST.md) after installing the complete .NET 10 SDK/runtime. No cloud or paid service was activated.

## Run again on this computer

Use PowerShell. PostgreSQL and .NET 10 were installed inside this project; the scripts select the local SDK automatically. Existing local secrets and database contents are preserved.

Terminal 1 — database, migrations, backend:

```powershell
Set-Location 'D:\Spilton AI\spilton-ai'
.\scripts\setup-local.ps1
.\scripts\start-postgres.ps1
.\scripts\migrate.ps1
.\scripts\start-backend.ps1
```

Terminal 2 — frontend:

```powershell
Set-Location 'D:\Spilton AI\spilton-ai\frontend'
npm.cmd run dev -- --hostname 127.0.0.1 --port 3000
```

Use **http://127.0.0.1:3000** (or `http://localhost:3000`). Both loopback origins are allowed by the supplied local frontend configuration. Register your own account; there are no seeded user credentials. Browser and API tests create disposable accounts under `example.test` with random passwords.

Existing tool installations and PostgreSQL data are reused. No reinstall/reset is needed for daily startup. On a **fresh checkout** only, install the required tools below and run `npm.cmd ci` inside `frontend` to install the locked package dependencies. A temporary local demo account created separately on Day 1 remains in the existing database; its credentials are not in source control.

Verify the API:

```powershell
Invoke-RestMethod 'http://localhost:5081/api/health'
```

Expected: `status = healthy`, `database = connected`. Development OpenAPI JSON: http://localhost:5081/openapi/v1.json (no Swagger UI installed).

Stop frontend/backend with Ctrl+C in their terminals. Stop the project database without deleting data:

```powershell
Set-Location 'D:\Spilton AI\spilton-ai'
.\scripts\stop-postgres.ps1
```

## Environment inspection and installation

The workspace was empty at the start of Day 1. Day 2 extended that existing codebase and reused its tools and database.

| Tool | Observed state | Verification |
| --- | --- | --- |
| Node.js | Installed, v25.6.1; build/browser checks passed | `node --version` |
| npm | Installed, 11.9.0 | `npm.cmd --version` |
| .NET SDK | System had only 8.0.423; installed local 10.0.400 | `& '.\.tools\dotnet\dotnet.exe' --version` |
| PostgreSQL | Initially absent; installed local 17.11, running on 5432 | `& '.\.tools\postgresql\pgsql\bin\psql.exe' --version` |
| Python | Launcher exists, no Python runtime installed; not needed today | `py --list`; after installation: `py -3 --version` |
| Git | Installed, 2.43.0.windows.1; local repository initialized | `git --version` |
| Docker | CLI 29.6.1 installed; Desktop engine crashes on startup | `docker info` must succeed before using Compose |

### .NET 10 (required)

The .NET 8 SDK cannot build this `net10.0` API. On a fresh checkout, either install the **SDK**, not just the runtime, from [Microsoft's .NET 10 page](https://dotnet.microsoft.com/en-us/download/dotnet/10.0), or repeat the official project-local installation:

```powershell
Set-Location 'D:\Spilton AI\spilton-ai'
New-Item -ItemType Directory -Force .tools | Out-Null
Invoke-WebRequest 'https://dot.net/v1/dotnet-install.ps1' -OutFile '.tools/dotnet-install.ps1'
& '.tools/dotnet-install.ps1' -Channel 10.0 -InstallDir "$PWD/.tools/dotnet" -NoPath
& '.tools/dotnet/dotnet.exe' --list-sdks
```

Expected: a 10.0 SDK. `global.json` allows stable .NET 10 feature bands. [Official installation-script documentation](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-install-script).

### PostgreSQL (required)

Authentication needs persistent storage. The tested path uses the Windows binaries linked by [PostgreSQL's official Windows download page](https://www.postgresql.org/download/windows/) and supplied by [EDB](https://www.enterprisedb.com/download-postgresql-binaries). No Windows service installation is necessary.

On a fresh checkout:

```powershell
New-Item -ItemType Directory -Force .tools | Out-Null
# Official EDB PostgreSQL 17.11 Windows x64 archive, verified September 8, 2026.
Invoke-WebRequest 'https://sbp.enterprisedb.com/getfile.jsp?fileid=1260491' -OutFile '.tools/postgresql.zip'
Expand-Archive -LiteralPath '.tools/postgresql.zip' -DestinationPath '.tools/postgresql'
.\scripts\setup-local.ps1
.\scripts\start-postgres.ps1
& '.tools/postgresql/pgsql/bin/pg_isready.exe' -h 127.0.0.1 -p 5432
```

Expected: `accepting connections`. If the archive link changes, select PostgreSQL 17 Windows x64 on EDB's page and extract it so `.tools/postgresql/pgsql/bin/pg_ctl.exe` exists. Alternatively, use the official Windows installer and configure `.env` for a database/user created there; do not run the local startup script against an independently installed server.

The local initializer generates a separate ignored administrator password, then creates a non-superuser `spilton` role that owns only the application database. SCRAM password authentication is required; the server binds to `127.0.0.1` only. Data lives in `.local/postgres-data`.

### Optional Docker alternative — not runtime-verified on this machine

Docker Desktop failed while initializing its `dockerInference` socket. It was not reset or reconfigured. [Docker's Windows setup guide](https://docs.docker.com/desktop/setup/install/windows-install/). Once the engine is healthy, use Compose instead of the local PostgreSQL server:

```powershell
.\scripts\stop-postgres.ps1
docker info
docker compose up -d --wait postgres
.\scripts\migrate.ps1
```

Do not run both databases on port 5432. The Compose volume is a **separate database** from `.local/postgres-data`; existing local accounts do not automatically transfer. `docker compose stop postgres` preserves the volume. Compose syntax is included, but container execution could not be tested because the engine is unavailable.

### Python (deferred)

Python is absent and is not required to run Day 1. No Python service is implemented. When needed, install a supported Python release using the installer or install manager from [python.org's Windows downloads](https://www.python.org/downloads/windows/). Verify `py -3 --version` and `py -3 -m pip --version` before building a Python service.

Node/npm and Git were already available. Fresh machines can use [the official Node.js download](https://nodejs.org/en/download) and [Git for Windows](https://git-scm.com/downloads/win). Verify using the commands in the table before proceeding.

## Configuration

Run `scripts/setup-local.ps1` once. It generates fresh random secrets and preserves existing files on subsequent runs. **Do not delete `.env` while keeping an existing database**: regenerating its password does not rotate the database role's actual password.

Root `.env` (see `.env.example`):

| Variable | Purpose |
| --- | --- |
| `POSTGRES_DB` | Application database, default `spilton` |
| `POSTGRES_USER` | Application role, default `spilton` |
| `POSTGRES_PASSWORD` | Random password, never committed |
| `POSTGRES_PORT` | Local PostgreSQL port, default `5432` |
| `JWT_SECRET` | Random signing key, at least 32 bytes; generated as 48 random bytes encoded as Base64 |
| `WEB_TAVILY_API_KEY` | Optional private Tavily key enabling real public-web search; server-side only |

`scripts/dev-environment.ps1` maps these to ASP.NET's `ConnectionStrings__DefaultConnection` and `Jwt__Secret`; it also sets the Development environment, API URL, local SDK path, and ignored SDK home. ASP.NET does not load `.env` directly. For a different launcher, supply those environment variables yourself.

Nonsecret backend settings in `appsettings.json`: `Jwt:Issuer`, `Jwt:Audience`, `Jwt:ExpiryMinutes` (30), `Cors:AllowedOrigins` (`http://localhost:3000`), and `AllowedHosts`. Environment overrides use `__`, for example `Cors__AllowedOrigins__0`.

Frontend `frontend/.env.local` (see `frontend/.env.example`):

| Variable | Value/purpose |
| --- | --- |
| `API_BASE_URL` | `http://localhost:5081`; server-only backend URL |
| `APP_ORIGIN` | `http://localhost:3000,http://127.0.0.1:3000`; comma-separated allowed browser mutation origins |
| `AUTH_COOKIE_SECURE` | `false` for local HTTP only; `true` for HTTPS |

Restart the relevant server after configuration changes. Never put credentials or JWT secrets in `NEXT_PUBLIC_` variables. Local development uses HTTP on loopback; deployment/HTTPS configuration was not part of Day 1.

## Tests and builds

With database, backend, and frontend running:

```powershell
Set-Location 'D:\Spilton AI\spilton-ai'
. .\scripts\dev-environment.ps1
dotnet build backend/Spilton.Api --no-restore
dotnet ef migrations has-pending-model-changes --project backend/Spilton.Api
node --test scripts/test-api.mjs
node --test scripts/test-chat.mjs
dotnet test backend/Spilton.Api.Tests
Set-Location frontend
npm.cmd run lint
npm.cmd run build
npx.cmd playwright test tests/auth.spec.ts tests/chat.spec.ts tests/agent.spec.ts tests/documents.spec.ts tests/preparation.spec.ts tests/government.spec.ts
```

Playwright uses installed Chrome (`channel: chrome`). The optional downloaded test-browser attempt timed out, so no browser download is needed on this computer. Tests use random accounts; they do not log generated passwords or JWTs. Screenshots are saved under ignored `.local/`.

The backend tests use the existing migrated PostgreSQL database and uniquely named test users. Their simulated provider faults exist only in the test project. They delete only their own test conversations; they do not reset tables or other users' data. If rebuilding on Windows reports a locked API executable, stop the API terminal, run `dotnet test`, and restart the backend. The tests host their own in-process API.

## Model configuration

The checked-in Development default requires no secret. The current machine's private `.env` overrides it with the user-created Groq credential. Production never exposes the development provider. Keep Groq on its Free plan; do not add a payment method or upgrade. Real-call tests are opt-in. Account billing is not inferred from an API key.

To connect a provider for which you have valid access, edit the ignored root `.env` locally (never paste a key into source or this README):

```dotenv
MODEL_DEFAULT=compatible
MODEL_DEVELOPMENT_ENABLED=false
MODEL_BASE_URL=https://your-provider-api.example/v1
MODEL_API_KEY=YOUR_PRIVATE_KEY_FROM_THE_PROVIDER
MODEL_NAME=YOUR_EXACT_ACCESSIBLE_CHAT_MODEL_ID
MODEL_PROVIDER_LABEL=Your provider name
MODEL_TIMEOUT_SECONDS=90
```

The example hostname and values are placeholders, not usable credentials. Use the provider's documented API prefix and an actual accessible model that supports streaming Chat Completions, system messages, and `max_tokens`. Restart the backend. Open **Settings** to see configured models, then send a test message. An entry being configured does not prove the credentials work; the first successful real request is the verification step. Provider error text is sanitized.

Model variables map to ASP.NET `Models__Default`, `Models__DevelopmentEnabled`, `Models__BaseUrl`, `Models__ApiKey`, `Models__Model`, `Models__ProviderLabel`, and `Models__TimeoutSeconds`. Keys stay server-side. The model selector only lists the development provider when enabled or a compatible transport when URL, key and model are configured. Spilton Auto applies capability requirements and selects only a configured provider/model. This machine has one real configured chat model, so routing remains intentionally single-model and exposes no fake alternatives.

See [AI system documentation](docs/ai-system.md) for streaming limits, provider extension points, stop behavior and real-provider verification. For the configured free-development path use `MODEL_BASE_URL=https://api.groq.com/openai/v1`, `MODEL_NAME=openai/gpt-oss-20b`, `MODEL_PROVIDER_LABEL=Groq`; store the real key only in the ignored root `.env` as `MODEL_API_KEY`. The existing key must never be copied into this README.

## Day 3 local dependencies and tests

This machine already has pgvector and the embedding model. Do not download them again for daily startup. On a fresh setup, follow [the pgvector instructions](docs/rag.md), run `scripts/enable-pgvector.ps1`, `scripts/setup-embeddings.ps1`, then `scripts/migrate.ps1`. The model is about 90 MB and CPU-only; no Python service is required.

Open **Documents** in the sidebar, upload a PDF/TXT/DOCX of at most 5 MB, wait for **Ready**, select it, and ask a question. The attachment button also uploads files. Selected excerpts are sent to the chosen chat provider. Source buttons open the actual owned chunk; a deleted source becomes unavailable.

Additional tests (existing services running; diagnostics enabled in the backend only for API retrieval tests):

```powershell
Set-Location 'D:\Spilton AI\spilton-ai'
node scripts/test-documents.mjs
Set-Location frontend
npx.cmd playwright test tests/auth.spec.ts tests/chat.spec.ts tests/documents.spec.ts
```

These use local real embeddings and default to mock generation. To explicitly repeat the small real RAG evaluation after confirming your provider organization is on Free:

```powershell
Set-Location 'D:\Spilton AI\spilton-ai'
$env:TEST_REAL_RAG = '1'
node scripts/test-documents.mjs
Remove-Item Env:TEST_REAL_RAG
```

Set `RAG_DIAGNOSTICS=true` in the ignored root environment file and restart the backend for the diagnostic API tests; restore it to `false` afterward. Optional `RAG_CHUNK_TOKENS`, `RAG_OVERLAP_TOKENS`, `RAG_TOP_K`, `RAG_MIN_SIMILARITY` are described in [docs/rag.md](docs/rag.md). Existing database and JWT values must be preserved.

To repeat the real outage test, stop **only the API** with Ctrl+C in its terminal, keep frontend running, then:

```powershell
Set-Location 'D:\Spilton AI\spilton-ai\frontend'
$env:TEST_OUTAGE = '1'
npx.cmd playwright test tests/outage.spec.ts
Remove-Item Env:TEST_OUTAGE
```

Restart the backend afterward. The default suite skips this deliberately disruptive test unless `TEST_OUTAGE=1` is set.

## Project layout

```text
spilton-ai/
  frontend/              Next.js routes, UI, same-origin auth handlers, browser tests
  backend/Spilton.Api/   API, EF model, migration, auth and errors
  ai-service/            Documented placeholder only
  docs/                  Architecture, database, AI design, and daily reports
  scripts/               Local setup/start/stop/migration/API test commands
  compose.yaml           Optional PostgreSQL container setup
  global.json            .NET 10 SDK selection
  dotnet-tools.json      Local EF CLI version
  .env.example           Configuration template; no real secrets
```

`.tools`, `.local`, real environment files, package dependencies, build output, and browser output are ignored. No Git commit or remote publication was made. If Git reports ownership mismatch between the automation account and your login, use `git -c safe.directory='D:/Spilton AI/spilton-ai' status` for this checkout.

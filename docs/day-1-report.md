# Spilton AI — Day 1 report

Date: September 8, 2026. The workspace was empty at inspection. Work was restricted to the foundation.

## COMPLETED

- Inspected Node/npm, .NET, Python, PostgreSQL, Git and Docker.
- Created frontend, backend, ai-service placeholder, docs and development scripts.
- Installed project-local .NET 10.0.400 and PostgreSQL 17.11 using official distribution sources.
- Built the Next.js/React/TypeScript/Tailwind frontend and .NET 10 API.
- Created/applied the EF migration; database read/write tested through the real API and browser.
- Implemented password hashing, JWT authentication, roles, protected profile endpoint and protected workspace.
- Added validation, development CORS, origin checks, rate limiting, structured errors, environment templates and Git ignores.
- Wrote startup, architecture, database and test documentation.

## WORKING

Frontend on http://localhost:3000; backend on http://localhost:5080; local PostgreSQL on 127.0.0.1:5432. Registration, login, invalid-login handling, duplicate-account handling, protected-page redirects, refresh persistence, logout, mobile layout and backend-unavailable handling passed browser tests. API and database recovery after database restart passed.

## PARTIALLY WORKING

Docker CLI is installed, but Docker Desktop's engine crashes. An optional Compose definition exists; container execution is not verified. The tested Day 1 database runs directly from project-local PostgreSQL binaries, so Docker does not block this foundation.

## NOT STARTED

RAG, agents, research mode, advanced memory, AWS, AI model integration, Python service, actual messaging and persisted chat history. The new-chat button and message composer are explicit placeholders. Python is absent and intentionally not installed for a documentation-only service.

## FILES/FOLDERS CREATED

- `frontend/`: App Router pages, auth handlers, centralized clients, auth/workspace components, styles, configuration, dependency lock and Playwright tests.
- `backend/Spilton.Api/`: host, auth controller, password/JWT services, data model, exception handler, settings, OpenAPI setup, EF migration and snapshot.
- `ai-service/README.md`: documented placeholder.
- `docs/architecture.md`, `docs/database.md`, `docs/day-1-report.md`.
- `README.md`, `.env.example`, `.gitignore`, `compose.yaml`, `global.json`, `dotnet-tools.json`.
- `scripts/`: setup, environment, PostgreSQL start/stop, backend start, migration and API integration test.
- Ignored local `.tools/`, `.local/`, `.env`, `frontend/.env.local`; no credentials committed. Repository initialized; no commit or remote publication made.

## DATABASE TABLES

`Users`, `Roles`, `UserRoles`, `__EFMigrationsHistory`. Applied migration: `20260908172950_InitialAuth`. `User` role seeded; no preset user accounts. Test accounts use random passwords and `example.test` addresses. Application role in the tested native database is not a superuser.

## API ENDPOINTS

| Method/path | Behavior |
| --- | --- |
| GET `/api/health` | 200 and PostgreSQL connected; 503 when unavailable |
| POST `/api/auth/register` | Validates and stores user, assigns User role, returns JWT/profile; 201/400/409 |
| POST `/api/auth/login` | Validates persisted password; 200 JWT/profile or 401 |
| GET `/api/auth/me` | Protected profile; valid JWT + active user + User role required |
| GET `/openapi/v1.json` | Development OpenAPI document |

Frontend same-origin handlers: POST `/api/auth/register`, `/api/auth/login`, `/api/auth/logout`; GET `/api/auth/session`. Browser auth responses omit JWT; the token is set in an HTTP-only cookie.

## TEST RESULTS

| Verification | Actual result |
| --- | --- |
| Backend compilation | PASS, zero warnings/errors |
| Frontend lint | PASS, zero warnings/errors after fixes |
| Frontend production build/TypeScript | PASS |
| EF migration generation/application | PASS on PostgreSQL 17.11 |
| Pending model changes | None |
| Reapply migration after restart | PASS, already up to date |
| API integration | 13 child checks passed (Node reports 14 including parent test) |
| Health, OpenAPI | PASS |
| Register, input validation, duplicate email | PASS |
| Login, invalid password, unknown email | PASS |
| Protected endpoint: missing/malformed/tampered JWT | 401, PASS |
| Protected endpoint: valid JWT | 200, PASS |
| Development CORS origin allow/deny | PASS |
| Real browser registration/login/logout + errors + refresh | PASS |
| HTTP-only cookie, no JWT in localStorage/document.cookie | PASS |
| Protected frontend redirect after logout | PASS |
| Mobile overflow and desktop/mobile visual inspection | PASS, 390px and 1366px widths |
| Loading state | PASS using controlled delayed browser response |
| Cross-origin cookie mutation rejection | 403, PASS |
| Browser suite | 4 tests passed, including combined auth flow |
| Separate real backend-outage browser test | 1 passed; API actually stopped, then restarted |
| Real database outage | Health/login 503; database restart and health recovery PASS |
| Direct PostgreSQL inspection | Four tables; populated hashes; ordered timestamps; non-superuser role |
| Git ignore inspection | Real env files, tools, DB data/admin password excluded |
| Optional Compose syntax | PASS (`docker compose config --quiet`); engine/container runtime still unavailable |

Browser tests caught premature form/button interactions before hydration. Forms now use POST and remain disabled until interactive; workspace action buttons have the same guard. All affected browser checks were rerun successfully. Screenshots: `.local/workspace-desktop.png`, `.local/workspace-mobile.png`.

## KNOWN ISSUES

- Docker Desktop startup fails on its local `dockerInference` socket. No reset, data deletion or settings change was attempted. Native PostgreSQL is the tested alternative.
- Python runtime absent; install/verification instructions are documented for when it is needed.
- The optional Playwright browser download timed out; tests succeeded with installed Chrome.
- Local .NET/PostgreSQL tools are ignored, not global installs. Use the documented scripts or install the tools separately on a new checkout.
- Logout clears the current browser's token. A separately copied JWT remains valid until its 30-minute expiry; no refresh/revocation/session system was added.
- Auth rate limiting is per backend connection IP; browser traffic through Next.js shares that limit in development.
- Local HTTP is tested. HTTPS deployment, password reset, email verification and production operations are outside Day 1.
- Cross-account file ownership can cause Git's safe-directory warning; README documents a per-command checkout exception.

## EXACT COMMANDS TO RUN THE PROJECT AGAIN

PowerShell terminal 1:

```powershell
Set-Location 'D:\Spilton AI\spilton-ai'
.\scripts\setup-local.ps1
.\scripts\start-postgres.ps1
.\scripts\migrate.ps1
.\scripts\start-backend.ps1
```

PowerShell terminal 2:

```powershell
Set-Location 'D:\Spilton AI\spilton-ai\frontend'
npm.cmd ci
npm.cmd run dev -- --hostname 127.0.0.1 --port 3000
```

Open http://localhost:3000 and create your own account. If the servers are already running, use them; do not start a second copy on the same ports. Full installation, shutdown and test commands are in the root README.

## DAY 2 STARTING POINT

Begin from this tested local foundation. Run the startup commands and smoke checks, then agree on the next small feature. No Day 2 functionality has been started.

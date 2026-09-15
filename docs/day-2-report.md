# Day 2 report — September 9, 2026

**Day 2 is complete using the clearly labelled development provider. Real AI provider integration is NOT TESTED: no external credential was configured and no real model request was made.** Existing authentication and PostgreSQL data were preserved. No Day 3 functionality was started.

## Day 1 regression status

Passed before Day 2 implementation: the original live authentication API suite and four browser authentication tests. Passed again after implementation: registration, login, HTTP-only JWT cookie persistence, protected page/profile, logout, invalid credentials, unauthorized access, and backend-unavailable feedback. Only browser selectors were adapted to the new interface; the authentication architecture remains intact.

## Completed and working

- Authenticated conversation creation, listing, opening, continuation, deterministic first-message titles, rename and confirmed deletion.
- User and assistant messages persisted in PostgreSQL; history survives refresh and logout/login.
- Progressive HTTP/SSE streaming, generating state, stop, disconnect cancellation, regeneration and saved partial/error status.
- Safe Markdown including headings, emphasis, lists, tables, links, inline code and fenced code with language labels, horizontal scrolling and copy controls.
- Original Spilton desktop/tablet/mobile interface, welcome state, composer, sidebar history, profile/logout and settings.
- Quick mode, configured-model selector and Spilton Auto selecting the configured default.
- Provider abstraction, bounded recent context, input/output limits, generation rate limiting, ownership checks and sanitized errors.

New Chat starts an unsaved draft in the UI. Sending the first message creates the conversation; the API also supports explicit empty conversation creation.

## Partially working

The optional compatible HTTPS provider transport is implemented and tested against simulated protocol responses. Its real network credentials, model availability, latency and output remain unverified. Think has a future architectural boundary but is disabled; no private chain-of-thought is displayed.

## Not started

RAG, files, document ingestion, embeddings, pgvector, grounded document answers/citations, agents, Research mode, exam mocks, PYQ analysis, advanced memory, AWS and a Python service implementation. Their UI entries are disabled or clearly marked Coming Soon.

## Database migrations and tables

Applied additive migration `20260909040101_AddConversationsAndMessages`. Existing `20260908172950_InitialAuth` remains applied. PostgreSQL was queried directly to verify both migrations, all six tables and intact relationships. No reset or destructive migration was used.

| Table | Purpose |
| --- | --- |
| Users | Existing profile, active state and password hashes |
| Roles | Existing authorization roles |
| UserRoles | Existing user/role relationship |
| Conversations | User-owned titles and timestamps |
| Messages | Ordered roles/content, provider/model, generation status and regeneration metadata |
| __EFMigrationsHistory | EF migration records |

Relationships are Users → Conversations → Messages. Ownership is checked in the API for every conversation operation. Schema details are in [database.md](database.md).

## API endpoints created

All require authentication:

| Method | Endpoint | Purpose |
| --- | --- | --- |
| GET | /api/models | Configured model metadata and available modes |
| POST | /api/conversations | Create conversation |
| GET | /api/conversations | Paginated owned history |
| GET | /api/conversations/{id} | Owned conversation and paginated messages |
| PATCH | /api/conversations/{id} | Rename |
| DELETE | /api/conversations/{id} | Delete owned conversation and messages |
| POST | /api/conversations/{id}/messages | Persist user message and stream response |
| POST | /api/conversations/{id}/regenerate | Stream replacement for latest user message |
| POST | /api/conversations/{id}/stop | Cancel active generation |

Existing health and register/login/me endpoints remain. The frontend same-origin `/api/chat/...` handler forwards allowed routes using the server-held JWT cookie.

## Model providers implemented

- `IModelProvider`, resolver/factory, context builder and generation coordination separate chat orchestration from provider transport.
- Development provider: deterministic, progressively streamed demonstrations explicitly labelled **not real AI**; available only in Development when enabled.
- Compatible provider: optional configured HTTPS streaming Chat Completions transport. It is advertised only with a valid configuration shape; successful authentication is established only by a real provider request.

**Real AI provider test status: NOT TESTED.** No key was fabricated, committed or sent to frontend JavaScript. See [ai-system.md](ai-system.md) for secure local configuration and the future Python gateway boundary.

## Streaming and security tests

Passed actual progressive development-provider streaming over HTTP and browser rendering. Passed persistence of complete/partial responses, explicit stop, client disconnect, concurrency rejection and regeneration. Simulated-provider tests passed for failure, timeout, empty response, partial failure, output limits and failed-regeneration recovery. Transport fixtures verified visible-text parsing, sanitized errors and unexpected stream termination; they were not real model calls.

Unauthorized requests and expired JWTs were rejected. Cross-user read, mutation, generation and stop attempts were rejected; another user's conversations were absent from history. Cross-origin frontend writes were rejected. Unsafe HTML and JavaScript Markdown links were tested. Input limits, unavailable configuration without accidental persistence, and per-user generation limits are implemented. Real secrets, local database files and generated outputs remain Git-ignored; no commit or publication was made.

## Test results

| Check | Final result |
| --- | --- |
| Combined live Day 1 + chat API tests | 27 child checks passed; Node reports 29 including two parent suites |
| .NET provider/context/error/security tests using PostgreSQL | 13 passed |
| Browser auth + chat tests, installed Chrome | 9 passed |
| Separate browser outage test with backend actually stopped | 1 passed; backend restarted afterward |
| Frontend lint | Passed |
| Frontend production build and TypeScript | Passed |
| Backend build | Passed, zero warnings/errors |
| EF pending model changes | None |
| Direct PostgreSQL schema/relationship verification | Passed |
| Real external LLM request | NOT TESTED |

Browser checks included registration, login, refresh, new chat, streaming, history reopening/continuation, rename, delete confirmation, response/code copy, regeneration, stop, safe Markdown, error feedback and desktop/tablet/mobile layouts. Screenshots are stored in ignored `.local/day2-*.png` files. Test users use random credentials and remain isolated local test data; test cleanup targets only its own conversations.

## Files created/changed

- `backend/Spilton.Api/Chat/`: entities, provider abstraction/transports, context, generation coordination and controllers.
- `backend/Spilton.Api/Migrations/`: chat migration and updated model snapshot.
- Backend `AppDbContext`, `Program`, Development configuration and package references; new `Spilton.slnx`.
- `backend/Spilton.Api.Tests/`: failure, context, transport and security tests.
- `frontend/src/app/api/chat/[...path]/route.ts`: authenticated streaming proxy.
- `frontend/src/lib/chat-client.ts`, updated backend helper, workspace, Markdown renderer and chat styles.
- Frontend package manifest/lock, chat browser tests and authentication selector adjustments.
- `scripts/test-chat.mjs`, environment mapping, `.env.example` and `.gitignore`.
- Root/frontend READMEs, architecture/database docs, `docs/ai-system.md`, this report and AI-service placeholder documentation.

## Known issues and practical limits

- Development output is scripted; it cannot answer arbitrary questions as a real LLM would.
- Compatible-provider behavior must be verified with your actual accessible model after configuration.
- Context is bounded by characters/messages, not model-specific token counting.
- Stop registry and rate limiting currently assume one API process. Interrupted orphan generations are marked failed by recovery after approximately three minutes.
- Logout clears the browser cookie; an already copied access JWT remains valid until its expiry (existing Day 1 behavior).
- Docker's existing engine issue remains untouched; the tested startup uses native project-local PostgreSQL. Python is not required for Day 2.

## Exact commands to run again

PowerShell terminal 1 — reuse existing database and tools:

```powershell
Set-Location 'D:\Spilton AI\spilton-ai'
.\scripts\start-postgres.ps1
.\scripts\migrate.ps1
.\scripts\start-backend.ps1
```

PowerShell terminal 2:

```powershell
Set-Location 'D:\Spilton AI\spilton-ai\frontend'
npm.cmd run dev -- --hostname 127.0.0.1 --port 3000
```

Open http://localhost:3000. Health: http://localhost:5080/api/health. If these services are already running, use their existing instances rather than starting duplicates.

## Environment variables required

Existing ignored root `.env`: `POSTGRES_DB`, `POSTGRES_USER`, `POSTGRES_PASSWORD`, `POSTGRES_PORT`, `JWT_SECRET`. Existing ignored frontend `.env.local`: `API_BASE_URL`, `APP_ORIGIN`, `AUTH_COOKIE_SECURE`. Examples contain placeholders only.

Development generation needs no new secret. Optional overrides: `MODEL_DEFAULT`, `MODEL_DEVELOPMENT_ENABLED`, `MODEL_TIMEOUT_SECONDS`. A real compatible provider additionally needs server-side `MODEL_BASE_URL`, `MODEL_API_KEY`, `MODEL_NAME` and optional `MODEL_PROVIDER_LABEL`; select `MODEL_DEFAULT=compatible` and restart the backend. Exact configuration instructions are in the root README and AI-system documentation.

## Day 3 starting point

The authenticated ownership boundary, persistent conversations, streamed responses and provider gateway are ready for a separate Day 3 implementation of files, ingestion, embeddings, pgvector, RAG, grounded answers and citations. None of those features has been started.

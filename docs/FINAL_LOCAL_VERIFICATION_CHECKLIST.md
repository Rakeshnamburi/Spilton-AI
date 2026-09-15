# Final local verification record

Executed on 2026-09-14 after installing .NET SDK 10.0.401, without resetting the existing database.

1. `docker compose up -d postgres`
2. `dotnet --info` and confirm a compatible .NET 10 SDK/runtime.
3. `dotnet restore Spilton.slnx --locked-mode`
4. `dotnet test Spilton.slnx --configuration Release`
5. Start the API with private environment configuration and verify `/api/health`.
6. In `frontend`: `npm ci`, `npm run lint`, `npx tsc --noEmit --incremental false`, `npm run build`.
7. Start frontend/API and run `npx playwright test`.
8. Re-run mapped-address SSRF cases: metadata/link-local, RFC1918, loopback and their `::ffff:` representations; verify a public HTTPS page remains readable.
9. With Tavily free quota available, run one current technical Research request and verify retrieved URLs, read evidence and persisted citations.
10. Verify Research cancellation, provider failure/rate-limit behavior and no fabricated answer.
11. Verify the model list contains only configured providers; vision and code execution report unavailable.
12. Verify Agent step/tool limits, stop, safe failure and the consequential-action approval boundary.
13. Verify cross-user conversation/document/memory/Space/government-resource ownership.
14. Verify General/Coding/Tutor/RAG while an Exam Space is active; no unrelated exam context may appear.

Results: Release backend build PASS; backend tests 120/120 PASS (including 13 DB/API integration tests); SSRF regression PASS; ESLint PASS; TypeScript PASS; production frontend build PASS; real Groq PASS; real Tavily/page-reader/citations PASS. Browser: 14/16 PASS, one outage-only case intentionally skipped, and one system-Chrome government-upload journey remains a runner flake. During that failure the upload returned 202, the document was read back with 200, government APIs stayed healthy, and Chrome alone replaced the tab with `chrome-error://chromewebdata`.

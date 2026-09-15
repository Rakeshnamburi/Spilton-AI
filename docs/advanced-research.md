# Phase 2.2 Advanced Research Intelligence

Status: implemented; final local verification remains pending.

Research is a general SPILTON capability. The deterministic intent detector separates normal questions, current-information requests, light research and deeper comparison/research. Normal questions remain on the cheaper chat path.

The bounded `ResearchOrchestrator` creates at most two deduplicated queries, runs at most two search rounds, reads at most four pages, keeps at most ten combined web/document evidence items, injects at most 22,000 evidence characters and uses one synthesis model call. A second Tavily search is performed only for deeper research when independent evidence or a requested comparison side remains missing.

Search results are deduplicated and ranked using relevance, search rank and a transparent source classification: `OFFICIAL`, `PRIMARY`, `ACADEMIC`, `REPUTABLE_SECONDARY`, `COMMUNITY`, or `UNKNOWN`. User-owned RAG evidence is kept separately as `USER_DOCUMENT`; memory and personalization remain context and are never treated as evidence.

Only relevant excerpts are injected. Every excerpt retains its server-assigned source number, title, URL or document identifiers, retrieval time, page/section metadata and source type. The server rejects citations that refer to an unknown source number or unsafe URL. Numeric/version conflicts with equivalent claim wording are flagged for synthesis. This detector is deliberately conservative and does not claim that the absence of a detected conflict proves agreement.

Webpages and document chunks remain untrusted data. Source text cannot change system policy, reveal secrets, grant permissions, invoke tools or alter research limits. The P2.1 public-address validation, DNS pinning and per-redirect SSRF validation remain in force, including normalization of IPv4-mapped IPv6 addresses.

Research progress is streamed as concise states: planning, searching, optional gap search, reading, comparing, and verifying. The existing stop endpoint cancels the linked research, page-read and model tokens. Agent mode uses the same bounded Research Orchestrator through a read-only tool; it does not start a nested autonomous loop.

No database migration is required. Citation JSON gained an optional source-type field, which remains backward compatible. Tavily Pay As You Go is not enabled and the key remains server-side.

Local verification after the environment issue is resolved:

```powershell
. .\scripts\dev-environment.ps1
dotnet test .\backend\Spilton.Api.Tests\Spilton.Api.Tests.csproj
cd .\frontend
npm run lint
npm run build
```

Then run one real Research-mode request and confirm that search/page logs, progressive UI states, source links and citations match the retrieved evidence.

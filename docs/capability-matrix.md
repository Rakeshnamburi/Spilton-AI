# Spilton MVP v1 capability matrix

| Capability | Status | Evidence / boundary |
| --- | --- | --- |
| Authentication and PostgreSQL | WORKING | Backend integration suite and browser auth flow pass. |
| General AI chat | WORKING | Persistent SSE chat, configured provider abstraction and development-provider browser tests pass. |
| Coding assistance | WORKING | Capability routing, bounded coding context, Markdown/code rendering and multi-turn chat are covered; code execution is unavailable. |
| Think / reasoning | WORKING FOUNDATION | Same configured model with a careful-answer instruction; no hidden reasoning is displayed. |
| Documents and RAG | WORKING | PDF/TXT/DOCX ingestion, local embeddings, pgvector retrieval, ownership, grounded answers and citations pass. |
| Memory and Spaces | WORKING | Explicit memory, relevant scoped retrieval, profiles, plans and cross-Space separation pass. |
| Calculator and date/time tools | WORKING | Registered read-only tools; calculator supports natural percentage expressions. |
| Agent mode | WORKING FOUNDATION | Main chat runs a bounded local-tool plan, streams concise progress, persists results, supports stop, and fails honestly for unavailable tools. |
| Saved-source research | WORKING | Selected owned documents only, with citations and freshness disclosure. |
| Live Web Search / Page Reader | WORKING | Tavily basic search plus an SSRF-restricted public page reader; real search/read and citation persistence verified. |
| Advanced Research | WORKING | Bounded planning, source selection/classification, evidence extraction, conflict/gap handling, web + owned-document context, progress and citation validation pass; real Tavily + Groq research retrieved official .NET sources. |
| Government Exam specialization | WORKING FOUNDATION | Profiles, progress, plans, notification/PYQ/current-affairs library, eligibility and mock backend/browser paths pass. |
| Multimodal vision/audio | PARTIAL | Safe request/media and capability abstractions exist; real vision/audio is `VISION_PROVIDER_NOT_CONFIGURED`. |
| Model routing | WORKING FOUNDATION | Configured-only capability metadata, selection, health and safe unsupported handling pass. Groq is the only real configured provider, so cross-provider fallback is unavailable. |
| Controlled coding workspace | PARTIAL | Canonical path, protected-file, patch and execution boundaries exist. Per-user workspace and isolated execution are unavailable. |
| Production security | PARTIAL | MVP controls plus HSTS/security headers and integration boundaries; refresh rotation, MFA, malware scanning, distributed limits and managed secrets remain deployment work. |
| CI/CD and deployment | PARTIAL | CI, backend/frontend Dockerfiles and an inert production Compose example are statically reviewed; no cloud infrastructure has been activated and no hosted CI run was performed. |
| Observability | WORKING FOUNDATION | Structured capability/provider/model/routing and generation latency, output estimate, citation count, agent step and tool duration logs; no reliable provider token/cost telemetry. |

The original workspace's generated `obj` folder is not writable from the controlled runner identity. Backend verification used an isolated temporary source copy; the production frontend build passed under the normal local account. See [release readiness](release-readiness.md) for evidence and [deployment](deployment.md) for the production architecture.

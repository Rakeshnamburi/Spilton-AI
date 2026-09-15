# Day 3 checkpoint — awaiting user-created free-provider credential

Historical checkpoint: work paused at the user's explicit Task 2 instruction to STOP when manual credential creation was required. The user subsequently configured Groq and authorized continuation. Real model and RAG tests have since passed; see [rag.md](rag.md) and the final Day 3 report for current status. The remainder records the earlier checkpoint.

## Regression results

Existing README, architecture, database and AI documentation inspected. Existing services reused. Health API reports healthy/database connected; frontend login returns HTTP 200.

- Live Day 1 + Day 2 API checks: 27 child checks passed (29 including parent tests).
- Backend tests: 13 passed.
- Browser authentication/chat tests: 9 passed.
- These cover registration/login, session persistence, ownership, streaming, storage, history, rename/delete, stop/regenerate and logout. Generation tests used the clearly labelled mock provider.

No application source, schema, existing user data or configuration was replaced. Regression tests created their normal isolated test accounts and cleaned up their own test conversations.

## Free hosted provider candidate

Groq Free plan, using the existing compatible provider adapter. Official sources checked September 9, 2026:

- [Billing FAQ](https://console.groq.com/docs/billing-faqs): Free tier is distinct from the paid Developer upgrade, which requires a payment method.
- [Rate limits](https://console.groq.com/docs/rate-limits): organization-wide request/minute, request/day and token quotas; actual account limits are shown in its console. Exceeding limits returns 429. Public table/plan labelling is ambiguous, so the displayed numbers are not treated as a guaranteed free quota.
- [Compatible API](https://console.groq.com/docs/openai): endpoint prefix `https://api.groq.com/openai/v1`.
- [Model catalog](https://console.groq.com/docs/models): candidate `openai/gpt-oss-20b`; real account availability still needs verification. Older Llama chat entries are now marked Enterprise, so do not rely on old free-tier tutorials.

Use a Free organization only. Do not add a payment method, buy credits or upgrade. If the account demands payment, stop and report that restriction without providing any secret. No real provider request has been made.

## Safe manual setup

1. Sign in at [Groq API keys](https://console.groq.com/keys), confirm the organization is on Free, and create a key.
2. Edit the existing ignored `D:\Spilton AI\spilton-ai\.env` locally. Preserve every PostgreSQL/JWT setting. Add or update these model settings (replace the key placeholder only in your local editor):

```dotenv
MODEL_DEFAULT=compatible
MODEL_DEVELOPMENT_ENABLED=true
MODEL_BASE_URL=https://api.groq.com/openai/v1
MODEL_API_KEY=PASTE_YOUR_KEY_HERE_LOCALLY_ONLY
MODEL_NAME=openai/gpt-oss-20b
MODEL_PROVIDER_LABEL=Groq
MODEL_TIMEOUT_SECONDS=90
```

3. Do not put this key in chat, source code, `.env.example`, frontend files or `NEXT_PUBLIC_` variables. The existing startup script maps `MODEL_API_KEY` to the server-only `Models__ApiKey` configuration. Git ignore was verified.
4. Restart only the backend using `scripts/start-backend.ps1` after stopping its existing process. Alternatively, tell the assistant configuration is saved so it can perform the controlled restart and tests.
5. Settings should show Groq, but that only verifies configuration shape. A real streamed response with Groq provenance and successful persistence verifies actual access. Real errors, empty responses and rate limits must also be checked. No automatic paid fallback is permitted.

The development provider remains separately labelled for repeatable regressions; Spilton Auto selects compatible when configured. `MODEL_DEVELOPMENT_ENABLED=false` can hide the demo outside these tests.

## Hardware and pending infrastructure

Observed: 15.7 GB usable RAM, Intel i5-1135G7 (4 cores/8 logical processors), Intel Iris Xe integrated graphics, approximately 2.6 GB free RAM during regression testing. No local model/runtime was downloaded. Local embedding model size, specifications, dimensions and memory requirements have not yet been selected or verified.

The project PostgreSQL installation has no `vector.control` extension file. pgvector installation, document schema, extraction, chunking, embeddings, retrieval, RAG, citations and document UI are all pending. No new migrations were created. No Day 4 features were started.

Cost this checkpoint: no provider calls, purchases, paid provisioning or model downloads. Existing local software and database were reused. Continue after the credential is configured, or after explicit user direction to proceed with mock generation and independent RAG infrastructure.

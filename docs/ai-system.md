# AI gateway through Day 3

## Current truth

The configured default now uses **real Groq `openai/gpt-oss-20b`** with a user-created server-side credential. Real streaming and persisted responses passed verification. The separately labelled Development demo remains available for repeatable tests and is never presented as real AI.

**Real model integration: TESTED.** Real RAG evaluation covered direct facts, multiple PDF pages, missing answers and wrong-document selection. Protocol/error fixtures remain separate from actual model calls. Local embeddings use the real 384-dimensional MiniLM model through ONNX Runtime. No payment method, upgrade or paid infrastructure was activated; the user's Groq account must remain on Free.

## Provider contracts

`IModelProvider` supplies safe model metadata and `IAsyncEnumerable<string> StreamAsync(context, cancellationToken)`. It produces visible text only. `IModelProviderResolver` isolates controllers from concrete providers, and `ModelProviderFactory` implements configuration-based selection. Spilton Auto maps to the configured default ID; it is not yet intelligent routing.

Implemented:

- `DevelopmentProvider`: enabled only in ASP.NET Development when configured; scripted output with a count of prior user messages to demonstrate context plumbing.
- `CompatibleProvider`: server-configured HTTPS Chat Completions transport; uses HttpClient, bearer authentication and streamed `choices[].delta.content`. Real Groq calls were verified on Day 3. It ignores private reasoning fields and tools. It requires streaming support, a valid accessible model, system messages and `max_tokens` support. The protocol follows the [official Chat Completions streaming reference](https://developers.openai.com/api/reference/resources/chat/subresources/completions/streaming-events).

To add a future Gemini, DeepSeek, local-model or Python-gateway implementation, implement the provider contract, validate server configuration, and register it in the resolver. Do not add vendor request types to controllers or the frontend. Local HTTP transport is deliberately not enabled in the current compatible adapter; it accepts HTTPS configuration only.

## Configure a real provider later

1. Obtain legitimate access and the provider's documented API prefix/model ID.
2. Edit the ignored root `.env` on the server. Use `MODEL_DEFAULT=compatible`, `MODEL_BASE_URL`, `MODEL_API_KEY`, `MODEL_NAME`, `MODEL_PROVIDER_LABEL`; optionally disable the demo with `MODEL_DEVELOPMENT_ENABLED=false`.
3. `scripts/dev-environment.ps1` maps these values to `Models__...` ASP.NET configuration. Environment variables can also be set directly by your launcher. Do not put any provider key in `NEXT_PUBLIC_` variables or browser storage.
4. Restart the backend. Settings lists only transports with complete configuration. This does not verify the key's validity or quota.
5. Send a short Quick-mode message. Confirm progressive text, a completed saved assistant record, correct provider/model provenance and persistence after refresh. Only then record a real-provider test as passed.

The default `.env.example` contains only placeholders. Never commit `.env`, real keys or a copied token. Provider URL, key and raw provider error bodies are not exposed through `/api/models`.

`MODEL_TIMEOUT_SECONDS` defaults to 90 and is clamped to 5–120 seconds. Input is at most 8000 UTF-16 characters, output at most 32000, and compatible output requests at most 2048 tokens. Context uses at most 20 recent completed visible messages and 16000 characters, plus a fixed system instruction. This is a character-based guard, not a tokenizer; choose a real model with adequate context capacity and validate its limits before enabling it. A future provider-specific token-budget implementation belongs behind `ContextBuilder`.

## Streaming lifecycle

1. Verify JWT, active account, ownership, mode/model, input size, rate limit and conversation lock.
2. Persist the user message and pending assistant row. Derive a title from the first ten whitespace-separated words, capped at 72 characters, with no extra model request.
3. Send `start` with stable message IDs, title and optional replacement ID.
4. Send `delta` events as visible text arrives. Flush each event. Save checkpoints roughly once per second.
5. Finalize content/status. On success, hide the previous regenerated version while retaining its stored row.
6. Send sanitized `error` when relevant and `done` with final message/status and persistence result.

The Next.js proxy forwards the response body directly. The browser uses fetch + a streaming reader to support authenticated POST and cancellation. There is no SignalR, WebSocket server or background agent process.

Stop posts to the owned conversation's `/stop` endpoint and cancels the provider token. Browser disconnection and timeouts also propagate cancellation. Already consumed provider work may still be billable; stopping the local request is not a promise of vendor-side cancellation. In-process stop coordination is appropriate for today's single API process. API restarts release database locks; stale generating rows recover to failed after three minutes.

If a connection ends without `done`, the UI reports interruption and offers reloading. No automatic resend occurs, avoiding duplicate user messages or repeat provider charges. A persistence failure is reported instead of claiming the final answer was saved.

## Context and modes

Context comes only from the owned conversation. It excludes superseded, failed/cancelled answers and arbitrary system messages. It walks recent history backwards until the count/character budget is reached, restores order and avoids beginning with an orphan assistant turn. No unlimited history, vector retrieval or advanced memory is used.

Quick uses a concise high-level system instruction. Think adds a careful-answer instruction. Research uses bounded real-web and optionally user-owned document evidence with citations. Agent can call registered read-only tools, including the bounded research tool. Progress states are operational summaries and never expose hidden chain-of-thought.

## Rendering and security

React renders user content as text. `react-markdown` and `remark-gfm` render assistant Markdown without raw HTML; unsafe protocols are sanitized and images are disabled to avoid unrequested remote loads. Links open with `noopener noreferrer`. Code preserves whitespace and scrolls horizontally, displays its language where present and supports copying. Copy response copies Markdown text, not HTML.

Chat endpoints use server-side user scoping and 404 for another user's IDs. The frontend proxy validates exact Origin for writes, keeps JWT and provider keys server-side, limits body size and disables caching. The API rate limit is 30 creates/sends/regenerations per authenticated user per minute. Existing auth limits remain. The limits are process-local; production distributed quotas and immediate JWT revocation are future work.

## Tests and future boundary

Live Node tests exercise HTTP streaming and PostgreSQL, including ownership and disconnect/stop. Chrome tests exercise registration/chat/history/copy/Markdown/cancellation. The xUnit host uses PostgreSQL and replaces the resolver only inside the test assembly to simulate failure, empty output, timeout and oversized responses. Compatible transport tests use HTTP fixtures, not real credentials.

Future Python boundary: the same bounded context request and cancellation contract can cross to a FastAPI gateway. ASP.NET continues to own identity and database authorization. Day 3 adds files, embeddings, pgvector and RAG inside that existing application boundary; no Python service, LangGraph, exam mocks or PYQ platform is built. See [rag.md](rag.md) for grounded context, citations, storage, evaluation and free-development configuration.

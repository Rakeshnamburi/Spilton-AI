# Spilton MVP v1 release readiness

Spilton's primary surface is one general-purpose chat that routes general, coding, tutoring, reasoning, document, tool, agent and optional exam requests. Government Exam Intelligence is a specialization and its profile is excluded when the active request is unrelated.

## Verified local release checks

- .NET 10 API build and backend integration suite pass from the established isolated build directory.
- Next.js production build, TypeScript and ESLint pass from the established isolated build directory.
- Browser coverage exercises authentication, streaming chat/history, stop/regenerate, unsafe Markdown, tools/Agent progress, documents/RAG/citations, Spaces/personalization and government resources.
- PostgreSQL health and migrations are preserved; no database reset is part of release setup.

The workspace `obj` and `.next` outputs are unwritable to the controlled runner identity on this machine. This is an environment permission boundary, not a source failure. Verification uses source copies under the user's temporary directory. Ordinary user terminals can follow the README commands.

## Honest limits

Live Web Search and the public Page Reader are implemented through Tavily and an SSRF-restricted reader. Advanced Research is implemented with bounded source selection, reading, evidence extraction, web + document context and citations; full local regression and real Tavily validation after the mapped-IPv6 security repair remain pending. Vision/audio are interfaces only. Agent mode is bounded to registered read-only tools. Arbitrary code execution, external side effects, payments and cloud deployment remain unavailable.

## Release checklist

1. Supply production PostgreSQL, HTTPS origins, a strong JWT secret and provider secrets through the hosting secret store.
2. Apply reviewed EF migrations before routing traffic.
3. Use durable object storage for documents and shared cancellation/rate-limiting state when running multiple API instances.
4. Disable the development provider, enable HTTPS cookies, configure health checks, backups and secret-safe structured logs.
5. Run backend, frontend and browser suites against the release environment.

See [deployment.md](deployment.md), [capability-matrix.md](capability-matrix.md), [architecture.md](architecture.md), and [ai-system.md](ai-system.md).

# SPILTON production readiness

This document records the safe local/demo posture and the work required before a public deployment. It is deliberately honest: an interface or configuration placeholder is not treated as an enabled production capability.

## Current status

| Area | Status | Notes |
| --- | --- | --- |
| General AI, coding, tutor, reasoning | WORKING | Configured provider path is exercised by the MVP tests. |
| Web search, page reader, research | WORKING | Tavily is optional and must remain within its free quota. SSRF checks include IPv4-mapped IPv6. |
| RAG, memory, Spaces, exam specialization | WORKING | Ownership checks remain mandatory. |
| Sessions | WORKING | Hashed, rotating refresh credentials, family revocation and logout-all are implemented. |
| Local rate limits | WORKING | Fixed-window process-local budgets. |
| Distributed rate limits | PARTIAL | `IDistributedRateLimitStore` is an extension point; configure Redis/another atomic store before multiple API replicas. |
| Durable object storage | PARTIAL | Local storage is protected; an S3-compatible provider remains an integration point. |
| Malware scanning | UNAVAILABLE | `MALWARE_SCANNER_NOT_CONFIGURED`; uploads still undergo type, size, signature and ownership validation. |
| Vision/audio | UNAVAILABLE | No configured provider currently supports these modalities. |
| Isolated code execution | UNAVAILABLE | Never execute generated code directly on the host. |
| CI/deployment | PARTIAL | Templates and production compose are provided; no cloud resources are activated. |

## Local production-like startup

1. Start PostgreSQL with pgvector and apply additive EF migrations.
2. Configure environment variables from `.env.example` using a local secret store; never commit `.env`.
3. Start the API and then the Next.js frontend. Use HTTPS and secure cookies behind a reverse proxy in a real deployment.

## Backup and recovery

Use `pg_dump` to a protected, access-controlled backup location and retain encrypted copies according to the operator's policy. Test restores in a separate database before adopting a migration. Never restore over the active database as part of routine deployment. pgvector extension creation and EF migration history must be included in the backup/restore runbook.

## Public deployment gate

Before exposing the service publicly, configure a distributed rate-limit store, durable private object storage, malware scanning (for example an isolated ClamAV service), managed secrets, HTTPS/CORS allowlists, backups, monitoring, and a sandboxed code-execution service if that feature is enabled. These are production-hardening requirements, not reasons to weaken the local/demo security boundary.

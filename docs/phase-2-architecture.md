# Phase 2 architecture

SPILTON remains a modular monolith. The primary flow is capability routing → bounded context → configured model/tool/RAG/Research/Agent → verification → response. Government Exam Intelligence uses the same components as an optional specialization and is injected only when routing marks it relevant.

Implemented boundaries:

- Advanced Research: bounded plans, deduplicated queries, source selection/classification, evidence extraction, conflict/gap checks, web plus owned-document evidence and citation validation.
- Model routing: capability metadata, configured-only selection and health telemetry.
- Multimodal: safe media metadata and an honest unavailable provider.
- Coding workspace: canonical path/security contracts and explicit unavailable execution.
- Agent: bounded steps/tool calls, structured observations, cancellation propagation and final verification metadata. Consequential actions stop for approval; none are registered today.
- Production security: security headers, privacy-conscious request telemetry and deployment extension points.
- Deployment: Docker build definitions, an inert production Compose example and a CI quality gate. No cloud resource is activated.


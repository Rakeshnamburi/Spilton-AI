# Saved-source research foundation

**Live web search is not connected.** Codex's one-time inspection of official documentation does not install a search provider in Spilton. There is no crawler, scheduled monitoring or hidden internet search.

`IResearchSourceProvider` is a replaceable boundary; `ManualResearchSources` resolves only selected Ready documents owned by the user in the conversation's Space. The government library orders sources by explicit trust rank. Model context includes their server-derived labels, and asks the model to prefer relevant official evidence, explain conflicts and disclose that live updates/corrigenda were not checked.

Research flow: select uploaded/registered document → question embedding → existing exact pgvector retrieval → bounded evidence/citations → source-label and preparation context → configured model → SSE response → stored messages and citations. The existing provider abstraction remains unchanged. A future genuinely free search provider can extend the source boundary after cost, terms, extraction and safety review. No paid endpoint is configured today.

Research without selected documents uses the configured real Web Search and SSRF-restricted Page Reader. Freshness detection remains deterministic and ordinary questions avoid web calls. In Research mode, selected documents are retrieved through owned RAG and combined with web evidence under one citation sequence. Search/page failures are reported honestly.

Progress distinguishes searching public sources from reading selected documents. The Sources section opens retrieved web URLs or actual owned document excerpts. Answers must use supplied evidence, acknowledge missing evidence and cite server-assigned source numbers. AI remains fallible: inspect cited sources before acting.

## Government resources and current affairs

`/government` provides three views of the same private `GovernmentResources` table: NOTIFICATION, PYQ and CURRENT_AFFAIRS. Shared ownership/source metadata avoids redundant tables. Filters: kind, title search, exam, organization, year, stage, subject, library status and publication date. Maximum 100 resources/user, returned with trust ranking. Metadata creation has a separate 20/min/user limit; upload and auth limits are unchanged.

URL-only registration stores metadata and explicitly reports LINK_METADATA_ONLY. It is not indexed or analyzed until the user uploads a document and registers an entry linked to it. Uploads reuse PDF/TXT/DOCX ingestion and real MiniLM embeddings. Notification section detection stores exact excerpts with chunk/page provenance. There is no unsupported normalized date/vacancy inference.

Current-affairs entries store a headline, concise manual summary (700 characters), category, reported date, optional exam and source, and a saved flag. No articles are copied by a crawler. Relevance is 25 points each for a policy/scheme/mission keyword, an appointment/award/report/index/economy keyword, verified official provenance and assigned exam; capped at 100. It is a transparent organizer, not ML or a factual-verification score. Today's content is filtered by reported date; empty days show no entries. Quiz remains future work.

## APIs

- GET/POST `/api/government/resources`
- GET/PATCH/DELETE `/api/government/resources/{id}`
- GET `/api/government/resources/{id}/download` (permitted owned paper copy only)
- POST `/api/government/resources/{id}/eligibility`
- POST `/api/government/compare` (two owned notifications in the same Space)
- GET `/api/government/research-status`
- Existing POST `/api/conversations/{id}/messages` supports `mode: "research"` with selected document IDs.

Deletion removes the library entry and its extracted fields, preserving the uploaded document. Deleting the source document cascades linked resource/field removal. Moving a document moves its library metadata to the same Space atomically. Historical chat text remains, while deleted citations become unavailable.

Tests: `scripts/test-government.mjs`, `GovernmentTests.cs`, `frontend/tests/government.spec.ts`. Real provider/official PDF options are explicit; tests never silently substitute a mock model.

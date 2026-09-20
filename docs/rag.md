# Day 3 document grounding

The existing authenticated ASP.NET application owns uploads, metadata, ingestion, retrieval and chat persistence. Local ONNX embeddings and PostgreSQL/pgvector provide real semantic retrieval. Groq `openai/gpt-oss-20b` has been tested for real streaming chat and grounded answers. No Python process, AWS, paid vector database, agent or Research mode is involved.

## Pipeline and ownership

Upload → validate type/name/size → store under a random name → save UPLOADED metadata → background worker marks PROCESSING → extract → clean → chunk → batch local embeddings → commit vectors and READY status.

The worker polls PostgreSQL; queued uploads survive restarts. One worker processes a document at a time, protected by a PostgreSQL advisory lock. PROCESSING work abandoned for five minutes is marked FAILED, with an instruction to upload again. A processing error never becomes READY. A three-minute processing cancellation limit is checked between extraction pages and embedding batches; native parser/inference calls are not forcibly killed mid-call.

Every document operation scopes to the authenticated user. Retrieval additionally filters selected IDs and READY state inside its SQL query. Chat validates selected ownership before acquiring document locks and holds those locks through generation. A user cannot delete a source while an answer is using it. Invalid/unavailable selections are rejected; the request does not silently become general chat.

## Local storage and deletion

`IFileStorage` uses ignored `.local/documents/` by default and supports private S3-compatible storage in production. Stored names are generated GUIDs plus validated extensions. Original names are display metadata only. Names containing path separators, colon or control characters are rejected. Both providers validate the generated-name format as a second boundary. S3 writes use `If-None-Match: *` so an existing object is never overwritten. Reads are bounded to the same 5 MB upload limit. Files are not exposed through public static hosting.

One `Documents` row represents one uploaded file and its processing result. A separate Files table is intentionally omitted to avoid duplicating one-to-one metadata. `DocumentChunks` has a required document FK and cascade delete. Existing Messages gain selected document IDs and citation-reference JSON. Source text is stored in chunks, not duplicated into new citation records.

Deletion first commits a DELETING tombstone, making the document unavailable for retrieval. It removes vectors/chunks and the local file, then removes metadata. Disk deletion failures are retried by the worker. Historical chat answers and reference labels remain part of conversation history; opening a deleted source returns unavailable. No embedding cache or second vector store needs cleanup.

## Formats and limits

| Limit | Default |
| --- | --- |
| Formats | PDF, TXT, DOCX |
| File size | 5 MiB |
| Documents per user | 30 |
| Selected documents per question | 5 |
| PDF pages | 100 |
| Extracted characters | 250,000 |
| DOCX expanded archive | 10 MiB, at most 1,000 entries |
| Chunks per document | 500 |
| Chunk size / overlap | 200 / 30 WordPiece tokens |
| Embedding batch | 4 chunks, two CPU inference threads |
| Retrieval | Top 6; cosine similarity minimum 0.25 |
| Evidence context | At most 10,000 characters |

PDF signature and DOCX ZIP signature are checked alongside extension/MIME. TXT accepts UTF-8 and BOM-detected text; invalid binary/NUL content fails. PdfPig extracts text from ordinary PDFs, preserving actual page numbers. No OCR: an empty/scanned PDF becomes FAILED with an explanatory message. DOCX parsing prohibits XML DTD resolution, bounds expanded ZIP size, rejects macro content and preserves paragraph headings where available. DOCX/TXT never invent page numbers.

Whitespace cleanup normalizes line endings and repeated horizontal whitespace, removes soft hyphens and preserves paragraph separation. Chunking groups by page/heading, prefers paragraphs and sentences, then uses word boundaries for oversized passages. Overlap stays within the same page/heading. Token counts use the pinned model vocabulary; chunks cannot exceed the model's input window. Long pathological segments fail rather than silently losing content.

## Real embeddings

Model: `sentence-transformers/all-MiniLM-L6-v2`, revision `1110a243fdf4706b3f48f1d95db1a4f5529b4d41`. ONNX model download: 90,405,214 bytes, plus a small vocabulary. Output: **384 dimensions**, as specified in the [official model card](https://huggingface.co/sentence-transformers/all-MiniLM-L6-v2). It is a trained sentence embedding model, not random vectors or keyword hashing.

The model's [published ONNX export](https://huggingface.co/sentence-transformers/all-MiniLM-L6-v2/tree/1110a243fdf4706b3f48f1d95db1a4f5529b4d41/onnx) runs through Microsoft ONNX Runtime on CPU. BERT uncased normalization/WordPiece tokenization, attention-mask mean pooling and L2 normalization follow the model specification. Inputs are capped at 256 pieces including special tokens. Long questions use their first 254 content pieces for retrieval. This is primarily an English embedding model; multilingual/Telugu retrieval has not been evaluated.

`IEmbeddingProvider` keeps orchestration independent of this implementation. Each document records the embedding model identity. Retrieval rejects documents embedded by a different configured model; changing model/dimensionality requires a reviewed migration/reindex strategy, not mixing vectors silently.

Machine inspected before download: 15.7 GB RAM, i5-1135G7 CPU (4 cores/8 logical processors), integrated Iris Xe graphics; around 2.6 GB RAM free during initial testing. A small CPU embedding model was selected instead of downloading a multi-gigabyte language model. Actual inference and semantic similarity tests are included. No GPU drivers or Python installation were needed.

## pgvector

Existing PostgreSQL 17.11 is retained. Enabled pgvector **0.8.6** with `scripts/enable-pgvector.ps1`, then applied the additive EF document migration. Application database credentials remain non-superuser; the existing local administrator is used only for extension enablement.

Upstream [pgvector Windows instructions](https://github.com/pgvector/pgvector#windows) require a C++ build toolchain absent on this machine. The conda package inspected targeted PostgreSQL 16 and was not used. The installed binary is a **community Windows build**, not a binary signed/released by upstream pgvector: [PostgreSQL 17 release](https://github.com/andreiramani/pgvector_pgsql_windows/releases/tag/0.8.6_17). Archive SHA-256, checked against GitHub release metadata:

```text
420388e9e9f05d92f06d6967ce8772483629b27a66ca9255925fa0fdd445438e
```

Only vector extension DLL/control/SQL files were copied to the project-local PostgreSQL directory. Existing executables, configuration and data were not replaced. Fresh setup can instead compile upstream source with the official Windows instructions. Do not use binaries for a different PostgreSQL major version.

Fresh local extension download (not needed on this configured computer):

```powershell
Set-Location 'D:\Spilton AI\spilton-ai'
New-Item -ItemType Directory -Force .tools/pgvector | Out-Null
Invoke-WebRequest 'https://github.com/andreiramani/pgvector_pgsql_windows/releases/download/0.8.6_17/vector.v0.8.6-pg17.zip' -OutFile '.tools/pgvector/pg17.zip'
.\scripts\enable-pgvector.ps1
```

Vector column: `vector(384)`. Search uses **exact cosine distance**, with ownership/document predicates in SQL. No HNSW/IVFFlat index: the small per-user development dataset favors correctness and avoids approximate-search filtering surprises. `TopK` and threshold are configurable; explicit `page N` questions filter to that actual PDF page and bypass the generic similarity threshold while retaining the Top-K/context limits.

Hybrid full-text search and reranking are postponed. `RetrievalService` returns scored evidence separately from `RagContextBuilder`, leaving a clean insertion point for future filtering/reranking. No paid reranking service was added.

## Grounded chat and citations

Ordinary chat bypasses retrieval. Selecting Ready documents switches the next message to document-grounded mode; selections are stored with user/assistant messages and restored when the conversation opens. Regeneration reuses the original user message's document IDs. To return to general chat, remove the selected document cards.

Grounded prompts contain only the current question and bounded retrieved excerpts. They treat file text/names as untrusted data, instruct the model to ignore instructions embedded in documents, require numbered evidence references and require an insufficient-evidence answer when the facts are absent. General chat context excludes prior document-grounded turns to avoid reintroducing unselected/deleted source material. Follow-up questions should name their subject explicitly; query rewriting and broad conversational RAG memory are not implemented.

The server assigns citation numbers and actual document/page/heading/chunk identifiers. The UI lists **Retrieved sources**, which can include evidence the model did not ultimately cite. Clicking one fetches the current owned source excerpt. No public file URL or fabricated page number is generated. Unsupported numeric citation IDs mark the response failed instead of complete. Source references are inspectable, but an LLM's factual interpretation is not a formal guarantee; the evaluation below measures a small development corpus.

## APIs and diagnostics

Authenticated routes:

- `POST /api/documents`: multipart field `file`, optional category; returns 202 with UPLOADED metadata.
- `GET /api/documents`: user's bounded document list.
- `GET /api/documents/{id}`: metadata and processing status.
- `DELETE /api/documents/{id}`: tombstone and cleanup.
- `GET /api/documents/{id}/chunks/{chunkId}`: owned source excerpt.
- Existing chat send endpoint accepts optional `documentIds` (up to five).

Categories: NOTIFICATION, SYLLABUS, PREVIOUS_YEAR_PAPER, STUDY_MATERIAL, USER_NOTES, RESUME, OTHER. The UI currently displays category metadata; upload classification defaults to OTHER. No exam-specific business logic was added.

`POST /api/documents/diagnostics` accepts `question` and `documentIds`. It is available only in Development when `RAG_DIAGNOSTICS=true`. It still requires authentication and ownership and returns scored retrieved chunks for development evaluation. The browser proxy intentionally does not expose this route and the normal UI has no diagnostic controls. Leave diagnostics disabled for normal use.

## Environment and startup

Existing PostgreSQL/JWT/frontend variables remain unchanged. The launcher maps root configuration to ASP.NET and supplies absolute local storage/model paths. Optional root `.env` controls:

```dotenv
RAG_CHUNK_TOKENS=200
RAG_OVERLAP_TOKENS=30
RAG_TOP_K=6
RAG_MIN_SIMILARITY=0.25
RAG_DIAGNOSTICS=false
```

Custom launchers may set `Rag__StoragePath`, `Rag__ModelPath` and corresponding `Rag__...` settings directly. Never place API keys in frontend variables. `scripts/setup-embeddings.ps1` downloads only the pinned local model/vocabulary if absent; existing files are reused.

Daily startup:

```powershell
Set-Location 'D:\Spilton AI\spilton-ai'
.\scripts\start-postgres.ps1
.\scripts\migrate.ps1
.\scripts\start-backend.ps1
```

Separate terminal:

```powershell
Set-Location 'D:\Spilton AI\spilton-ai\frontend'
npm.cmd run dev -- --hostname 127.0.0.1 --port 3000
```

No AI service command is needed. The future Python gateway remains a documented boundary, not a required microservice.

## Evaluation and costs

`scripts/test-documents.mjs` generates tiny fictional PDF/TXT/DOCX fixtures and tests real ingestion/retrieval/security. Set `RAG_DIAGNOSTICS=true` in the backend environment only while running diagnostics tests. It makes no hosted call by default. Set `TEST_REAL_RAG=1` explicitly to test four short real questions. Browser document tests similarly use the demo by default, with real local embeddings; `TEST_REAL_RAG=1` opts into real Groq generation.

Evaluated: direct age answer (PDF page 1), degree plus deadline (pages 1 and 2), missing scholarship award amount (insufficient evidence), scholarship question with only a gardening document (insufficient evidence), cross-user isolation and actual source-page metadata. Real Groq responses passed all four answer cases. General real streaming was separately verified by `scripts/test-real-provider.mjs` (explicit `TEST_REAL_PROVIDER=1` opt-in).

Local storage, PostgreSQL, pgvector and local embeddings require no paid service. Groq is configured for the user's selected Free development path; no card, upgrade, credit purchase or paid infrastructure was activated by this work. The API key alone does not reveal the organization's billing tier; account billing was not independently inspected. A paid Groq organization can incur charges, so keep it on Free. Rate limits return errors; there is no paid-provider fallback or automatic repeated generation. [Official billing](https://console.groq.com/docs/billing-faqs) and [account-specific limits](https://console.groq.com/docs/rate-limits).

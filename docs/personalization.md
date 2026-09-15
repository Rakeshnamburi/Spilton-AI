# Personalization and Spaces

Spaces, exam profiles, goals, explicit memory, manual topic progress and editable plans extend the existing database. General means nullable SpaceId and preserves pre-Space records. Chats and document lists/retrieval use exact user + Space scope. Uploaded documents can be assigned through PATCH /api/documents/{id}. Archived Spaces preserve data and reject new chat/profile writes. Only empty Spaces can be deleted.

`/prepare` exposes the dashboard and controls. An explicit Space URL preserves selection across reload; account data persists in PostgreSQL across login. No secret or JWT is put in browser storage.

Memory is manual and requires approval. No chat is automatically promoted to permanent memory. Active memory from the current Space is used; global Preference, StudyPreference and UserApprovedFact may apply in every Space. Global weak areas do not cross into other Spaces. Deactivated/deleted entries are excluded from future assembled context. A basic credential-pattern guard rejects obvious secrets; users must never enter credentials. Earlier chat text remains visible as ordinary conversation history.

The context builder includes the current Space, current profile, three active goals, five relevant reported progress records and eight active memory entries, reduced if needed to a 7,500-character data budget. Document-grounded facts must still come from document evidence; personalization only adjusts teaching style. Provider instructions prohibit official predictions and private chain-of-thought output.

Recommendations are rules, not ML: accuracy under 60% after at least 10 questions is prioritized; review/mastered topics without practice for seven days are due for review; learning and unstarted topics follow. A topic-name-matching goal due within 14 days adds priority. Reasons are visible. Dates are preparation preferences, not official exam dates.

Readiness is labelled an internal preparation indicator: average status points across catalog topics (NOT_STARTED=0, LEARNING=25, PRACTICING=50, REVIEW=75, MASTERED=100). It is not an official prediction. Accuracy is correct/attempted only when attempts exist. Manual diagnostic entry records actual counts; no standardized assessment is claimed.

Plans cover 1–14 days, respect daily minutes (15–480) and stop at the user's expected exam date. One priority topic recurs; other topics rotate. Items can be edited or marked done/skipped. Manual edits can change daily totals. Plans do not automatically invent practice counts or mastery.

Limits: 30 Spaces/user, 50 goals and memory entries/workspace, 30 saved plans/workspace. Ownership enforced server-side; no shared Spaces. Tests: scripts/test-preparation.mjs, PreparationTests.cs, frontend/tests/preparation.spec.ts. See completion report for actual execution status.

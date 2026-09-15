# Spilton frontend

See [the project README](../README.md) for complete local setup, environment variables and tested commands.

Run `npm.cmd run dev -- --hostname 127.0.0.1 --port 3000`. Install dependencies with `npm.cmd ci` only on a fresh checkout. Open http://localhost:3000. The backend and PostgreSQL must be running. `/login`, `/register` and protected `/chat` retain the Day 1 authentication flow.

Day 2 adds streaming chat, persisted history, rename/delete, stop/regenerate, response/code copying and safe Markdown. Generation currently uses a clearly labelled deterministic development provider, not real AI. Quick is available; Think, Research, Agent and future modules are disabled and labelled Coming Soon. New Chat starts a draft; the conversation is saved when its first message is sent.

Checks: `npm.cmd run lint`, `npm.cmd run build`, and `npx.cmd playwright test tests/auth.spec.ts tests/chat.spec.ts` (installed Chrome required). See the root README for the separate backend-outage test and provider configuration. JWT cookies and provider credentials remain server-side.

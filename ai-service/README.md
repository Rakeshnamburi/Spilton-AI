# Future AI service boundary — placeholder through Day 2

No separate Python service is implemented. Python is not required for the Day 2 frontend, backend, database, authentication or streaming development chat.

Simple generation currently runs through ASP.NET's `IModelProvider` and resolver. A future remote provider/gateway can forward bounded message context to FastAPI and stream visible text/status back. ASP.NET should retain authentication, ownership, persistence, input limits and cancellation coordination.

Future direction: ASP.NET → Python FastAPI → LangGraph / models / tools. RAG, agents, research mode, advanced memory and AWS are not implemented. This folder intentionally remains documentation only. See `docs/ai-system.md`.

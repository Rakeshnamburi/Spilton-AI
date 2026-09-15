# Tools and research boundary

Tools implement a small `ISpiltonTool` contract with a name, description, input schema, permission level, execution result and safe error. `ToolRegistry` is the Day 7 boundary for a future agent orchestrator; it does not loop or autonomously select tools.

The initial local tools are:

- `calculator`: a bounded parser for numbers, parentheses and `+ - * / %`. It never executes code or calls the model.
- `date_time`: current UTC time or a locally available time zone.

Web Search uses the configured Tavily free tier and Web Page Reader fetches bounded public HTTP/HTTPS text after DNS/IP validation. Both are READ_ONLY tools. Search and page failures remain explicit; no result or citation is fabricated. Research mode uses selected owned documents when attached and real web sources otherwise.

Tool execution is authenticated and exposed through `/api/tools`. Inputs have short limits, errors are sanitized, and tool results are never treated as trusted instructions. The next safe extension is a reviewed free search provider with domain allowlists, source limits, timeouts, robots/terms compliance, and citation capture.

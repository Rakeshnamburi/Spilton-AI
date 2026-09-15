# Agent foundation

Day 8 adds a bounded `Planner` and `AgentOrchestrator`. A run has a goal, at most four planned steps, structured observations, a terminal status, and a final safe answer. The orchestrator can execute only registered tools. It does not execute shell commands, browse implicitly, loop indefinitely, or expose private reasoning.

The registered read-only tools include calculator, date/time, Web Search, Web Page Reader and the bounded Research Orchestrator. Agent mode is available in the main chat, persists the user request and result, emits concise progress events, and uses the existing generation stop signal. Research tool failures remain explicit and never fabricate results. Document RAG and web + document research preserve ownership and citations.

Tool permissions are represented as `READ_ONLY`, `LOW_RISK`, or `CONSEQUENTIAL`. Consequential steps return `approval_required` and are not executed. The approval state is a foundation only; no dangerous action is connected.

The next agent extension should add a reviewed free web provider behind the existing tool contract and connect source provenance. Keep maximum steps, tool calls, retries, source count, and context size bounded. Approval continuation remains a foundation because no consequential external action is connected.

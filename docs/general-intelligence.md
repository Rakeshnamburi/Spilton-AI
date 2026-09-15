# General and coding intelligence

Spilton's main chat is general-purpose. A small server-side capability router classifies an explicit request as `GENERAL`, `CODING`, `TUTOR`, `REASONING`, `EXAM`, `DOCUMENT_RAG`, or `RESEARCH`. Obvious requests use deterministic rules; there is no extra classifier-model call. Selected documents always take precedence and require grounded retrieval. Current or latest questions without selected evidence return an honest live-research-unavailable response.

Coding requests receive a larger bounded context and output allowance. The system instruction asks for complete files, folder structure, dependencies, run commands, testing steps, and debugging corrections when relevant. Recent coding requests are retained within the normal context limit, with a small older-request excerpt foundation for follow-ups. The model is never told that code was executed unless a future execution tool actually runs it.

Exam context is included only for explicit exam requests or relevant preparation topics. General software questions therefore do not inherit SSC or other exam-profile details. User-approved communication preferences may be used when relevant; Space-specific exam context remains isolated.

Markdown rendering uses safe React Markdown with raw HTML and images disabled. Fenced code blocks show a language label, safe lightweight token coloring, horizontal scrolling, and a copy action.

Think mode currently uses the configured model with a careful-answer instruction. It does not expose hidden reasoning or claim a separate reasoning engine. Agent mode runs bounded registered local tools in the main chat. Live web research remains unavailable.

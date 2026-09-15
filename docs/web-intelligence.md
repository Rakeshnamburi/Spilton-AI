# Phase 2.1 Web Intelligence

Fresh/current requests route deterministically to `RESEARCH`; ordinary general and coding questions do not search. Tavily basic search returns bounded structured titles, URLs, snippets, rank, provider and retrieval time. The independent page reader follows at most three redirects and validates every destination.

The reader permits public HTTP/HTTPS only. It rejects URL credentials, nonstandard ports, localhost, loopback, private, link-local, multicast and cloud-metadata addresses. The connection handler resolves and pins a public address, reducing DNS-rebinding risk. Redirects are manual, response types are limited to HTML/plain text, compressed responses are not automatically expanded, response size is capped at 1 MB, extracted prompt content is capped, and requests have bounded timeouts.

Web content is labelled untrusted data. It cannot change system policy, request secrets, grant tool permission or trigger actions. P2.2 selects and extracts bounded evidence from at most four pages and performs a second search only for an identified deeper-research gap. The model may cite only server-assigned source numbers. Stored citations contain the retrieved title, final URL, retrieval timestamp and source classification; the UI opens the actual URL in a separate safe tab.

Configuration in ignored `.env`:

```dotenv
WEB_TAVILY_API_KEY=your_private_key
```

`scripts/dev-environment.ps1` maps this to `Web__TavilyApiKey`. No key reaches frontend JavaScript or logs. Tavily basic search consumes one free-plan credit per request; the application never upgrades or enables pay-as-you-go.

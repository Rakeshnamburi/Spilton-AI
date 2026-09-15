# Production security posture

Implemented controls include short-lived signed JWTs, active-user validation, high-cost password hashing, timing-resistant unknown-account login, explicit CORS origins, frontend origin validation for cookie mutations, per-user/IP rate limits, ownership filters, upload limits, SSRF-safe page reading, bounded tools/Agents, safe errors, no-store API responses, HSTS outside development and security headers.

Phase 2 adds explicit boundaries for malware scanning, distributed rate limiting and secret fingerprinting without logging secret values. They currently report `MALWARE_SCANNER_NOT_CONFIGURED` and `PROCESS_LOCAL_ONLY`.

Production gaps are refresh-token rotation/revocation, MFA enrollment/challenge, a configured malware scanner, distributed throttling, durable object storage, managed secret storage, tested backup restoration and central audit retention. These require deliberate deployment choices and are not presented as working local features.


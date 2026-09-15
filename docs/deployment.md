# Deployment architecture (planning only)

The repository includes backend/frontend Dockerfiles, `compose.production.example.yaml`, and `.github/workflows/ci.yml`. They are templates only and activate no external infrastructure. Production still requires explicit secret injection, HTTPS termination, durable object storage, backup/restore testing, configured malware scanning and distributed rate limiting.

No cloud resources or paid services were activated for MVP v1. A production deployment can host the Next.js application behind HTTPS, the ASP.NET Core API as a private service, PostgreSQL with pgvector as the durable database, and uploaded documents in object storage through the existing file-storage abstraction.

```text
Browser -> HTTPS frontend -> private HTTPS API -> PostgreSQL + pgvector
                                      |        -> object storage
                                      +--------> configured model provider
```

Use a managed secret store for the database connection, JWT signing secret and model credential. Never place them in frontend public variables or an image. Restrict CORS and the frontend mutation-origin allowlist to the deployed HTTPS origin, set secure HTTP-only cookies, terminate TLS at a trusted ingress, and allow the API/database/object store only the network access they need.

Run reviewed EF migrations as a controlled release step. Configure database backups and restore drills, object lifecycle policy, `/api/health` probes, structured log collection with redaction, alerting for failures/latency, and provider/rate-limit dashboards. Multiple API instances require distributed rate limits and cancellation coordination or sticky routing. Replace local document storage with an `IFileStorage` object-storage implementation; document business logic does not need to change.

Provider and web integrations remain configuration-driven. Only advertise a model, search provider or multimodal capability after a real deployment smoke test succeeds. Estimated infrastructure price depends on the selected host and is deliberately not claimed here.

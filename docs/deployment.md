# Deployment architecture (planning only)

The repository includes backend/frontend Dockerfiles, `compose.production.example.yaml`, and `.github/workflows/ci.yml`. They are templates only and activate no external infrastructure. Production still requires explicit secret injection, HTTPS termination, durable object storage, backup/restore testing, configured malware scanning and distributed rate limiting.

No cloud resources or paid services were activated for MVP v1. A production deployment can host the Next.js application behind HTTPS, the ASP.NET Core API as a private service, PostgreSQL with pgvector as the durable database, and uploaded documents in object storage through the existing file-storage abstraction.

```text
Browser -> HTTPS frontend -> private HTTPS API -> PostgreSQL + pgvector
                                      |        -> object storage
                                      +--------> configured model provider
```

Use a managed secret store for the database connection, JWT signing secret and model credential. Never place them in frontend public variables or an image. Restrict CORS and the frontend mutation-origin allowlist to the deployed HTTPS origin, set secure HTTP-only cookies, terminate TLS at a trusted ingress, and allow the API/database/object store only the network access they need.

Reviewed EF migrations run once during production startup under EF's migration lock. Set `Database__ApplyMigrationsOnStartup=false` only when the hosting platform runs migrations as a separate release command. Configure database backups and restore drills, object lifecycle policy, `/api/health` probes, structured log collection with redaction, alerting for failures/latency, and provider/rate-limit dashboards. Multiple API instances require distributed rate limits and cancellation coordination or sticky routing.

Durable private document storage is selected with `Storage__Provider=S3`. Configure `Storage__Endpoint`, `Storage__Region`, `Storage__Bucket`, `Storage__AccessKey`, `Storage__SecretKey`, `Storage__KeyPrefix`, and `Storage__ForcePathStyle`. The endpoint must use HTTPS. The bucket must remain private and its credential should be limited to get, conditional put, and delete access for the configured prefix. Local storage remains the development default.

Provider and web integrations remain configuration-driven. Only advertise a model, search provider or multimodal capability after a real deployment smoke test succeeds. Estimated infrastructure price depends on the selected host and is deliberately not claimed here.

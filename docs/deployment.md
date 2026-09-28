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

For a Render deployment that does not have R2 or S3, set `Storage__Provider=Database`. Uploaded files (maximum 5 MB each) are stored in the existing PostgreSQL database and survive service restarts. Monitor the database storage quota. For larger production workloads use `Storage__Provider=S3` and configure `Storage__Endpoint`, `Storage__Region`, `Storage__Bucket`, `Storage__AccessKey`, `Storage__SecretKey`, `Storage__KeyPrefix`, and `Storage__ForcePathStyle`. Local storage remains the development default and is not durable on Render.

For OTP email on Render use Brevo's HTTPS API: `Email__Provider=Brevo`, `Email__BrevoApiKey`, `Email__FromAddress`, and `Email__FromName`. The API key must be a Brevo API v3 key, the from-address must be verified under **Senders, domains, IPs**, and Brevo must show the account as activated for transactional sending. SMTP variables are ignored when the provider is Brevo.

For Google login configure `GOOGLE_CLIENT_ID`, `GOOGLE_CLIENT_SECRET`, `Google__RedirectUri`, and `Google__FrontendOrigin` on the backend. Add the exact backend callback URI (for example `https://spilton-ai.onrender.com/api/auth/google/callback`) to the Google OAuth client's authorized redirect URIs. The frontend origin is the deployed Vercel origin without a trailing slash.

Image generation is available through an OpenAI-compatible Images API. Configure `ImageGeneration__Provider=OpenAICompatible`, `ImageGeneration__BaseUrl`, `ImageGeneration__ApiKey`, `ImageGeneration__Model`, and optionally `ImageGeneration__TimeoutSeconds=120` in Render. For OpenAI use the current GPT image model available to the API project. The base URL must be HTTPS and normally ends in `/v1`; do not include `/images/generations`. The API key remains backend-only. The Groq text-chat key cannot be reused because Groq chat models do not return generated image files.

Provider and web integrations remain configuration-driven. Only advertise a model, search provider or multimodal capability after a real deployment smoke test succeeds. Estimated infrastructure price depends on the selected host and is deliberately not claimed here.

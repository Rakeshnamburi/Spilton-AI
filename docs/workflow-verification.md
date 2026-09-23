# Workflow verification

Registration requests an email-verification code and opens `/verify-email`.
The account remains usable if email delivery is disabled or fails; the user can
resend the code from that page or Settings. Verification uses the signed-in session.
Password reset uses a separate, single-use code and revokes existing sessions.

Render email configuration (server only):

- `Email__Provider=Brevo`
- `Email__BrevoApiKey`: a Brevo API key, not an SMTP key
- `Email__FromAddress`: a verified sender in that Brevo account
- `Email__FromName=Spilton AI`

Do not add duplicate variables. Redeploy after changing values. To verify delivery,
request a code for a registered account, inspect Brevo Transactional logs, and
complete the flow using the received code. HTTP 202 alone does not prove delivery:
unknown addresses receive the same acknowledgement to avoid exposing accounts.
`/api/health` checks the database and chat provider, not email delivery.

The exam catalog includes national and state preparation targets. It remains a
starter taxonomy, not a verified current syllabus. The mock screen lists all
targets but only starts tests when actual questions exist. Adding an exam does not
create its question bank; official papers and checked answers still need ingestion.

Image generation is not configured. The Groq text-chat connection cannot produce
image files. An image provider and its credentials are still needed; no image
generation or delivery should be claimed as verified.

The documents popup reports document errors independently of chat errors. File
uploads renew expired sessions. Local storage remains temporary on Render; durable
storage setup was deferred. These UI fixes do not establish durable storage or
prove production document ingestion.

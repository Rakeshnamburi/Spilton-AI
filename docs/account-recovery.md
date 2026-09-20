# Account recovery configuration

Spilton password recovery uses a six-digit, ten-minute, single-use code. Only a salted hash of the code and a hash of the subsequent reset token are stored. Five invalid attempts invalidate the challenge. A successful password change revokes every existing session for that user.

Signed-in users can also verify their email from Settings. Email verification uses a separate challenge purpose with the same expiry, hashing, five-attempt lockout, invalidation, and single-use guarantees. Existing email/password login and rotating sessions remain available while verification is introduced.

For Render Free, use Brevo over HTTPS (SMTP ports 25, 465 and 587 are blocked). Configure these private backend variables:

- `Email__Provider=Brevo`
- `Email__BrevoApiKey` (Brevo API key, not an SMTP key)
- `Email__FromAddress` (a sender verified in Brevo)
- `Email__FromName=Spilton AI`

Complete Brevo account verification and enable transactional sending. Save and redeploy Render, then request an OTP for an existing active account. Check Brevo transactional logs for delivery, bounce or rejection status. HTTP 202 alone does not prove delivery: unknown accounts receive the same response to avoid exposing account membership. Existing SMTP values are ignored when Brevo is selected. Never share the API key in chat.

For hosts that support SMTP, configure:

- `Email__Provider=Smtp`
- `Email__SmtpHost`
- `Email__SmtpPort`
- `Email__SmtpUsername`
- `Email__SmtpPassword`
- `Email__FromAddress`
- `Email__FromName`
- `Email__EnableSsl=true`

Restart the backend after changing them; production startup applies reviewed migrations before serving requests. Password reset begins at `/forgot-password`, and email verification is available in authenticated Settings. Never place SMTP credentials in Vercel or any `NEXT_PUBLIC_` variable. When SMTP is absent the API returns a clear configuration-required error and never pretends that delivery succeeded.

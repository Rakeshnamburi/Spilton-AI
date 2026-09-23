"use client";
import { useState, type FormEvent } from "react";
import Link from "next/link";
import { apiRequest } from "@/lib/api-client";

export default function VerifyEmailPage() {
  const [busy, setBusy] = useState(false), [error, setError] = useState(""), [notice, setNotice] = useState("Check your registered email for a verification code. If it has not arrived, use Resend code."), [done, setDone] = useState(false);
  async function run(job: () => Promise<void>) { setBusy(true); setError(""); try { await job(); } catch (e) { setError((e as Error).message); } finally { setBusy(false); } }
  function verify(e: FormEvent<HTMLFormElement>) { e.preventDefault(); const code = new FormData(e.currentTarget).get("code"); void run(async () => { await apiRequest("/auth/verify-email", { code }); setDone(true); setNotice("Your email is verified."); }); }
  return <main className="auth-page auth-single-page"><section className="auth-card"><h1>Verify your email</h1><p role="status">{notice}</p>{error && <p role="alert" className="error-message">{error}</p>}{!done && <form onSubmit={verify}><fieldset disabled={busy}><label>6-digit verification code<input name="code" autoComplete="one-time-code" inputMode="numeric" pattern="[0-9]{6}" maxLength={6} required /></label><button className="primary-button">{busy ? "Please wait…" : "Verify email"}</button><button type="button" className="secondary-button" onClick={() => void run(async () => { await apiRequest("/auth/request-email-verification", {}); setNotice("A new verification code was sent. Check your inbox and spam folder."); })}>Resend code</button></fieldset></form>}<p className="auth-switch"><Link href="/chat">{done ? "Continue to Spilton" : "Continue and verify later"}</Link></p></section></main>;
}

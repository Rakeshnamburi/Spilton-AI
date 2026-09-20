"use client";

import Link from "next/link";
import { useState } from "react";
import { useRouter } from "next/navigation";
import { apiRequest } from "@/lib/api-client";
import { PasswordField } from "@/components/password-field";

type Stage = "email" | "code" | "password" | "done";

export function PasswordRecoveryForm({ initialEmail = "" }: { initialEmail?: string }) {
  const router = useRouter();
  const [stage, setStage] = useState<Stage>("email");
  const [email, setEmail] = useState(initialEmail);
  const [token, setToken] = useState("");
  const [busy, setBusy] = useState(false);
  const [error, setError] = useState("");
  const [notice, setNotice] = useState("");

  async function sendCode(value: string) {
    await apiRequest("/auth/forgot-password", { email: value });
    setEmail(value);
    setNotice("If this email belongs to an active account, a 6-digit OTP has been sent. Check your inbox and spam folder. It expires in 10 minutes.");
    setStage("code");
  }

  async function submit(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setBusy(true);
    setError("");
    const data = new FormData(event.currentTarget);
    try {
      if (stage === "email") {
        await sendCode(String(data.get("email")).trim());
      } else if (stage === "code") {
        const result = await apiRequest<{ resetToken: string }>("/auth/verify-reset", { email, code: String(data.get("code")).trim() });
        setToken(result.resetToken);
        setNotice("OTP verified. Create a new password for your account.");
        setStage("password");
      } else if (stage === "password") {
        const password = String(data.get("password"));
        if (password !== String(data.get("confirmPassword"))) throw new Error("Passwords do not match.");
        await apiRequest("/auth/reset-password", { resetToken: token, newPassword: password });
        setStage("done");
        setNotice("Password updated. You can now sign in with your new password.");
      }
    } catch (cause) {
      setError(cause instanceof Error ? cause.message : "Please try again.");
    } finally {
      setBusy(false);
    }
  }

  async function resendCode() {
    setBusy(true);
    setError("");
    try {
      await sendCode(email);
      setNotice("If this email belongs to an active account, a new OTP has been sent. Only the newest code will work.");
    } catch (cause) {
      setError(cause instanceof Error ? cause.message : "Please try again.");
    } finally {
      setBusy(false);
    }
  }

  const step = stage === "email" ? 1 : stage === "code" ? 2 : 3;
  return <main className="auth-page auth-single-page">
    <Link href="/" className="brand"><span className="brand-mark">S</span> Spilton <span className="muted">AI</span></Link>
    <section className="auth-card">
      <p className="eyebrow">SECURE ACCOUNT RECOVERY</p>
      <h1>{stage === "done" ? "Password changed" : "Reset your password"}</h1>
      <p className="muted">{stage === "email" ? "Enter your registered email and we will send a one-time password." : stage === "code" ? "Enter the OTP sent to your registered email." : stage === "password" ? "Choose a new password with at least 6 characters." : "Your account is ready to use again."}</p>
      {stage !== "done" && <ol className="recovery-steps" aria-label="Password recovery progress">
        <li className={step === 1 ? "active" : step > 1 ? "complete" : ""}>Email</li>
        <li className={step === 2 ? "active" : step > 2 ? "complete" : ""}>6-digit OTP</li>
        <li className={step === 3 ? "active" : ""}>New password</li>
      </ol>}
      {stage !== "email" && stage !== "done" && <p className="recovery-destination">OTP email: <strong>{email}</strong></p>}
      {notice && <p role="status" className="field-help">{notice}</p>}
      {error && <p role="alert" className="error-message">{error}</p>}
      {stage !== "done" ? <form onSubmit={submit}><fieldset disabled={busy}>
        {stage === "email" && <label>Registered email<input name="email" type="email" autoComplete="email" required maxLength={254} defaultValue={initialEmail} placeholder="you@example.com" /></label>}
        {stage === "code" && <label>6-digit OTP<input name="code" inputMode="numeric" autoComplete="one-time-code" pattern="[0-9]{6}" minLength={6} maxLength={6} required placeholder="000000" /></label>}
        {stage === "password" && <><PasswordField id="new-password" name="password" label="New password" autoComplete="new-password" minLength={6} /><PasswordField id="confirm-new-password" name="confirmPassword" label="Confirm new password" autoComplete="new-password" minLength={6} /></>}
        <button className="primary-button" type="submit">{busy ? "Please wait…" : stage === "email" ? "Send OTP to my email" : stage === "code" ? "Verify OTP" : "Save new password"}</button>
        {stage === "code" && <><button className="secondary-button" type="button" onClick={resendCode}>Resend OTP</button><button className="link-button" type="button" onClick={() => { setStage("email"); setNotice(""); setError(""); }}>Use a different email</button></>}
      </fieldset></form> : <button className="primary-button" onClick={() => router.replace("/login")}>Sign in with new password</button>}
      <p className="auth-switch"><Link href="/login">Back to sign in</Link></p>
    </section>
  </main>;
}

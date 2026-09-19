"use client";
import Link from "next/link";
import { useState, useSyncExternalStore } from "react";
import { useRouter } from "next/navigation";
import { apiRequest } from "@/lib/api-client";
const subscribe = () => () => {};
export function AuthForm({ mode }: { mode: "login" | "register" }) {
  const router = useRouter();
  const hydrated = useSyncExternalStore(subscribe, () => true, () => false);
  const register = mode === "register";
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  async function submit(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault(); setLoading(true); setError("");
    const data = new FormData(event.currentTarget);
    try {
      await apiRequest(`/auth/${mode}`, { ...(register ? { name: String(data.get("name")).trim() } : {}), email: String(data.get("email")).trim(), password: data.get("password") });
      let destination="/chat";const returnTo=new URLSearchParams(window.location.search).get("returnTo");if(returnTo){try{const target=new URL(returnTo,window.location.origin);if(target.origin===window.location.origin&&target.pathname==="/mocks")destination=target.pathname+target.search;}catch{}}router.replace(destination); router.refresh();
    } catch (err) { setError(err instanceof Error ? err.message : "Please try again."); setLoading(false); }
  }
  return <main className="auth-page">
    <Link href="/" className="brand"><span className="brand-mark">S</span> Spilton <span className="muted">AI</span></Link>
    <section className="auth-card"><p className="eyebrow">YOUR WORKSPACE STARTS HERE</p><h1>{register ? "Create your account" : "Welcome back"}</h1><p className="muted">{register ? "A little space for your next big idea." : "Sign in to your Spilton workspace."}</p>
      <form method="post" onSubmit={submit} aria-busy={loading || !hydrated}><fieldset disabled={loading || !hydrated}>
        {register && <label>Full name<input name="name" autoComplete="name" required maxLength={100} placeholder="Your name" /></label>}
        <label>Email<input name="email" type="email" autoComplete="email" required maxLength={254} placeholder="you@example.com" /></label>
        <label>Password<input name="password" type="password" autoComplete={register ? "new-password" : "current-password"} required minLength={register ? 6 : 1} maxLength={128} aria-describedby={register ? "password-help" : undefined} /></label>
        {register && <p id="password-help" className="field-help">Use 6–128 characters.</p>}
        {error && <p role="alert" className="error-message">{error}</p>}
        <button className="primary-button" type="submit">{loading ? (register ? "Creating account…" : "Signing in…") : (register ? "Create account" : "Sign in")}</button>
        <button className="secondary-button" type="button" disabled title="Google sign-in will be enabled after the OAuth callback is deployed">Continue with Google (coming soon)</button>
      </fieldset></form>
      <p className="auth-switch">{register ? "Already have an account?" : "New to Spilton?"} <Link href={register ? "/login" : "/register"}>{register ? "Sign in" : "Create an account"}</Link></p>
    </section><p className="auth-footer">Spilton AI · Your preparation workspace</p>
  </main>;
}

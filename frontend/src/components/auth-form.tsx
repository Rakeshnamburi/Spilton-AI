"use client";
import Link from "next/link";
import { useState, useSyncExternalStore } from "react";
import { useRouter } from "next/navigation";
import { apiRequest } from "@/lib/api-client";
import { BrainCircuit, FileText, GraduationCap, ShieldCheck } from "lucide-react";
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
    <section className="auth-showcase" aria-label="About Spilton AI"><Link href="/" className="brand auth-brand"><span className="brand-mark">S</span> Spilton <span>AI</span></Link><div className="auth-promise"><p className="eyebrow">YOUR INTELLIGENT WORKSPACE</p><h2>Learn, research and build with clarity.</h2><p>One secure workspace for focused conversations, grounded documents and exam preparation.</p><div className="auth-benefits"><div><BrainCircuit/><span><strong>Thoughtful assistance</strong><small>Quick answers and careful reasoning</small></span></div><div><FileText/><span><strong>Work with your documents</strong><small>Private uploads with cited answers</small></span></div><div><GraduationCap/><span><strong>Prepare with purpose</strong><small>Exam profiles, practice and progress</small></span></div></div></div><p className="auth-trust"><ShieldCheck size={17}/> Your credentials and API keys stay protected.</p></section>
    <section className="auth-form-panel"><Link href="/" className="brand auth-mobile-brand"><span className="brand-mark">S</span> Spilton <span>AI</span></Link><section className="auth-card"><p className="eyebrow">{register ? "CREATE YOUR WORKSPACE" : "WELCOME BACK"}</p><h1>{register ? "Create your account" : "Sign in to Spilton"}</h1><p className="muted">{register ? "Start your personal AI and preparation workspace." : "Continue where you left off."}</p>
      <form method="post" onSubmit={submit} aria-busy={loading || !hydrated}><fieldset disabled={loading || !hydrated}>
        {register && <label>Full name<input name="name" autoComplete="name" required maxLength={100} placeholder="Your name" /></label>}
        <label>Email<input name="email" type="email" autoComplete="email" required maxLength={254} placeholder="you@example.com" /></label>
        <label>Password<input name="password" type="password" autoComplete={register ? "new-password" : "current-password"} required minLength={register ? 6 : 1} maxLength={128} aria-describedby={register ? "password-help" : undefined} /></label>
        {!register && <p className="field-help"><Link href="/forgot-password">Forgot password?</Link></p>}
        {register && <p id="password-help" className="field-help">Use 6–128 characters.</p>}
        {error && <p role="alert" className="error-message">{error}</p>}
        <button className="primary-button" type="submit">{loading ? (register ? "Creating account…" : "Signing in…") : (register ? "Create account" : "Sign in")}</button>
        <button className="secondary-button" type="button" disabled title="Google sign-in will be available after secure OAuth setup">Google sign-in · coming soon</button>
      </fieldset></form>
      <p className="auth-switch">{register ? "Already have an account?" : "New to Spilton?"} <Link href={register ? "/login" : "/register"}>{register ? "Sign in" : "Create an account"}</Link></p>
    </section><p className="auth-footer">Secure access · Spilton AI</p></section>
  </main>;
}

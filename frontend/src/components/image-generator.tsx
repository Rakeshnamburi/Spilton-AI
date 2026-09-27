"use client";
import Link from "next/link";
import Image from "next/image";
import { Download, Image as ImageIcon, LoaderCircle, Sparkles } from "lucide-react";
import { useEffect, useState } from "react";
import { sessionFetch } from "@/lib/session-fetch";
import "./image-generator.css";

type Capability = { configured: boolean; status: string; provider: string; model?: string | null };
type Result = { dataUrl: string; mimeType: string; provider: string; model: string; revisedPrompt: string };

export function ImageGenerator() {
  const [capability, setCapability] = useState<Capability | null>(null);
  const [prompt, setPrompt] = useState("");
  const [layout, setLayout] = useState("square");
  const [busy, setBusy] = useState(false);
  const [error, setError] = useState("");
  const [result, setResult] = useState<Result | null>(null);

  useEffect(() => {
    void sessionFetch("/api/chat/images/capability", { cache: "no-store" })
      .then(async response => {
        if (response.status === 401) { window.location.replace("/login?returnTo=%2Fimages"); return null; }
        if (!response.ok) throw new Error("Unable to check image generation.");
        return response.json() as Promise<Capability>;
      })
      .then(value => value && setCapability(value))
      .catch(() => setError("Image-generation status is temporarily unavailable."));
  }, []);

  async function generate(event: React.FormEvent) {
    event.preventDefault();
    if (!prompt.trim() || busy) return;
    setBusy(true); setError(""); setResult(null);
    try {
      const response = await sessionFetch("/api/chat/images/generate", {
        method: "POST", headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ prompt: prompt.trim(), layout }), cache: "no-store",
        signal: AbortSignal.timeout(150000)
      });
      const data = await response.json().catch(() => ({}));
      if (!response.ok) throw new Error(data.title || "The image could not be created.");
      setResult(data as Result);
    } catch (reason) {
      setError(reason instanceof Error ? reason.message : "The image could not be created.");
    } finally { setBusy(false); }
  }

  return <main className="image-workspace">
    <header><Link href="/chat" className="image-brand" aria-label="Back to Spilton AI"/><Link href="/chat">Back to chat</Link></header>
    <section className="image-hero"><span><Sparkles size={17}/> SPILTON IMAGE STUDIO</span><h1>Create an image from your idea</h1><p>Describe the scene, style, colours and important details. Your configured provider creates the image securely.</p></section>
    <div className="image-grid">
      <form className="image-card" onSubmit={generate}>
        <label htmlFor="image-prompt">Describe your image</label>
        <textarea id="image-prompt" rows={8} maxLength={1000} value={prompt} onChange={e=>setPrompt(e.target.value)} placeholder="A modern passenger aeroplane above clouds at sunrise, professional advertising photograph, high detail"/>
        <div className="image-layouts" role="group" aria-label="Image layout">
          {[['square','Square'],['portrait','Portrait'],['landscape','Landscape']].map(([value,label])=><button type="button" key={value} className={layout===value?'selected':''} onClick={()=>setLayout(value)}>{label}</button>)}
        </div>
        {capability && !capability.configured && <div className="image-notice"><strong>Provider setup required</strong><span>Add the image-generation variables in Render, then redeploy. No API key is stored in the browser.</span></div>}
        {error && <div className="error-message" role="alert">{error}</div>}
        <button className="image-generate" disabled={busy || !prompt.trim() || capability?.configured === false}>{busy?<><LoaderCircle className="spin"/>Creating image…</>:<><Sparkles/>Generate image</>}</button>
      </form>
      <section className="image-preview" aria-live="polite">
        {result ? <><Image unoptimized width={1024} height={1024} src={result.dataUrl} alt={result.revisedPrompt || prompt}/><div><p>{result.revisedPrompt}</p><a href={result.dataUrl} download="spilton-generated-image.png"><Download size={17}/> Download PNG</a></div></> : <div className="image-empty"><ImageIcon size={52}/><h2>Your image will appear here</h2><p>Choose a layout, describe what you need, and generate.</p></div>}
      </section>
    </div>
  </main>;
}

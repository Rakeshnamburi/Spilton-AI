"use client";
import { useState, type ReactNode } from 'react';
import ReactMarkdown from 'react-markdown';
import remarkGfm from 'remark-gfm';
import { Copy, Check } from 'lucide-react';
export function hasNamedProjectFiles(content: string) { return /```[\w+#.-]+\s+(?:file|filename)=[^\r\n`]+\r?\n[\s\S]*?```/i.test(content); }
export function DownloadProjectButton({ content, projectName = 'spilton-project' }: { content: string; projectName?: string }) {
  const [status, setStatus] = useState('');
  async function download() {
    setStatus('Preparing…');
    try {
      const response = await fetch('/api/chat/workspace/archive', { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify({ content, projectName }) });
      if (!response.ok) { const problem = await response.json().catch(() => ({})); throw new Error(problem.title || 'Project export failed.'); }
      const blob = await response.blob(); const url = URL.createObjectURL(blob); const anchor = document.createElement('a'); anchor.href = url; anchor.download = `${projectName}.zip`; anchor.click(); URL.revokeObjectURL(url); setStatus('Downloaded');
    } catch (error) { setStatus(error instanceof Error ? error.message : 'Download failed.'); }
    setTimeout(() => setStatus(''), 3500);
  }
  return <span className="copy-control"><button type="button" className="sp-icon-text" onClick={() => void download()}>{status || 'Download project ZIP'}</button></span>;
}
export function CopyButton({ text, label = 'Copy response' }: { text: string; label?: string }) {
  const [status, setStatus] = useState('');
  async function copy() {
    try { await navigator.clipboard.writeText(text); setStatus('Copied'); }
    catch { setStatus('Copy unavailable. Select the text manually.'); }
    setTimeout(() => setStatus(''), 2500);
  }
  return <span className="copy-control"><button type="button" className="sp-icon-text" aria-label={label} onClick={copy}>{status === 'Copied' ? <Check size={14} /> : <Copy size={14} />}{status === 'Copied' ? 'Copied' : label}</button>{status && status !== 'Copied' && <span role="status">{status}</span>}</span>;
}
function CodeBlock({ children, className }: { children?: ReactNode; className?: string }) {
  const language = /language-([\w+-]+)/.exec(className || '')?.[1] || 'text';
  const text = String(children ?? '').replace(/\n$/, '');
  // Lightweight lexical highlighting. React escapes every token; no HTML injection.
  const tokens = text.length <= 64000 ? text.split(/("(?:\\.|[^"\\])*"|'(?:\\.|[^'\\])*'|\/\/[^\n]*|\b(?:const|let|var|function|return|if|else|for|while|class|public|private|static|void|new|using|import|from|export|default|async|await|def|print|SELECT|FROM|WHERE|JOIN|true|false|null|None)\b|\b\d+(?:\.\d+)?\b)/g) : [text];
  return <div className="sp-code"><div className="sp-code-toolbar"><span>{language}</span><CopyButton text={text} label="Copy code" /></div><pre><code className={className}>{tokens.map((token,i)=>i%2===0 ? token : <span key={i} style={{color:token.startsWith('//')?'#94a3b8':/^["']/.test(token)?'#86efac':/^\d/.test(token)?'#fbbf24':'#93c5fd'}}>{token}</span>)}</code></pre></div>;
}
export function MarkdownMessage({ content }: { content: string }) {
  return <div className="sp-markdown"><ReactMarkdown remarkPlugins={[remarkGfm]} skipHtml components={{
    // Raw HTML is never enabled. Images are disabled to avoid unexpected remote tracking requests.
    img: () => <span>[Image display unavailable]</span>,
    a: ({ href, children }) => <a href={href} target="_blank" rel="noopener noreferrer">{children}</a>,
    pre: ({ children }) => <>{children}</>,
    code: ({ children, className, node }) => {
      const block = Boolean(className) || String(children).includes('\n') || (node?.position?.start.line !== node?.position?.end.line);
      return block ? <CodeBlock className={className}>{children}</CodeBlock> : <code className="sp-inline-code">{children}</code>;
    },
    table: ({ children }) => <div className="sp-table-scroll"><table>{children}</table></div>,
  }}>{content}</ReactMarkdown></div>;
}

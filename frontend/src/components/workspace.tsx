"use client";
import { useEffect, useRef, useState, useSyncExternalStore } from 'react';
import Link from 'next/link';
import { Plus, Search, Home, BookOpen, ListChecks, Files, Bot, Settings, MessageSquare, Menu, X, Send, Square, Paperclip, Globe, Zap, Pencil, Trash2, RotateCcw, Sparkles, ChevronRight } from 'lucide-react';
import { apiRequest, type User } from '@/lib/api-client';
import { chatRequest, streamChat, type Conversation, type ConversationDetail, type ChatMessage, type Model } from '@/lib/chat-client';
import { MarkdownMessage, CopyButton, DownloadProjectButton, hasNamedProjectFiles } from './markdown-message';
import './chat.css';
import './documents.css';
import { DocumentsPanel, Citations, type UploadedDocument } from './documents-panel';
function spaceId(){return typeof window==='undefined'?null:new URLSearchParams(window.location.search).get('space');}
function scoped(path:string){const id=spaceId();return path+(id?(path.includes('?')?'&':'?')+'spaceId='+encodeURIComponent(id):'');}
function chatUrl(id?:string){const p=new URLSearchParams();if(spaceId())p.set('space',spaceId()!);if(id)p.set('c',id);return '/chat'+(p.size?'?'+p.toString():'');}
const subscribe = () => () => {};
function dayGroup(date: string) {
  const today = new Date(); const yesterday = new Date(); yesterday.setDate(today.getDate() - 1);
  return new Date(date).toDateString() === today.toDateString() ? 'Today' : new Date(date).toDateString() === yesterday.toDateString() ? 'Yesterday' : 'Earlier';
}
export function Workspace({ user }: { user: User }) {
  const hydrated = useSyncExternalStore(subscribe, () => true, () => false);
  const [history, setHistory] = useState<Conversation[]>([]);
  const [historyMore, setHistoryMore] = useState(false);
  const [models, setModels] = useState<Model[]>([]);
  const [defaultModel, setDefaultModel] = useState('');
  const [model, setModel] = useState('auto');const [mode,setMode]=useState('quick');
  const [documents, setDocuments] = useState<UploadedDocument[]>([]);
  const [selectedDocuments, setSelectedDocuments] = useState<string[]>([]);
  const [documentsOpen, setDocumentsOpen] = useState(false);
  const [uploading, setUploading] = useState(false);
  const attachment = useRef<HTMLInputElement>(null);
  const selectAfterReady = useRef<string | null>(null);
  async function refreshDocuments() {
    const list = await chatRequest<{items:UploadedDocument[]}>(scoped('/documents')); setDocuments(list.items);
    const waiting = list.items.find(d=>d.id===selectAfterReady.current);
    if(waiting?.status==='READY'){setSelectedDocuments(ids=>ids.includes(waiting.id)?ids:[...ids.slice(0,4),waiting.id]);selectAfterReady.current=null;}
  }
  useEffect(()=>{let alive=true;async function refresh(){try{const list=await chatRequest<{items:UploadedDocument[]}>(scoped('/documents'));if(!alive)return;setDocuments(list.items);const ready=list.items.find(d=>d.id===selectAfterReady.current&&d.status==='READY');if(ready){setSelectedDocuments(ids=>ids.includes(ready.id)?ids:[...ids.slice(0,4),ready.id]);selectAfterReady.current=null;}}catch{/* Explicit upload/list errors remain actionable. */}}void refresh();const timer=setInterval(refresh,2500);return()=>{alive=false;clearInterval(timer);};},[]);
  async function uploadDocument(file:File){if(file.size>5*1024*1024){setError('Files must be 5 MB or smaller.');return;}if(!/\.(pdf|txt|docx)$/i.test(file.name)){setError('Supported formats are PDF, TXT and DOCX.');return;}setUploading(true);setError('');try{const form=new FormData();form.append('file',file);if(spaceId())form.append('spaceId',spaceId()!);const response=await fetch('/api/chat/documents',{method:'POST',body:form,signal:AbortSignal.timeout(60000)});const data=await response.json();if(!response.ok)throw new Error(data.title||'Upload failed.');selectAfterReady.current=data.id;await refreshDocuments();setNotice('File uploaded. Processing must finish before it can be used for chat.');setDocumentsOpen(true);}catch(e){fail(e);}finally{setUploading(false);}}
  const [active, setActive] = useState<Conversation | null>(null);
  const [messages, setMessages] = useState<ChatMessage[]>([]);
  const [messagesMore, setMessagesMore] = useState(false);
  const [loading, setLoading] = useState(true);
  const [generating, setGenerating] = useState(false);
  const [draft, setDraft] = useState('');
  const [error, setError] = useState('');
  const [notice, setNotice] = useState('');
  const [sidebar, setSidebar] = useState(false);
  const [settings, setSettings] = useState(false);
  const [dialog, setDialog] = useState<{ kind: 'rename' | 'delete'; conversation: Conversation } | null>(null);
  const [title, setTitle] = useState('');
  const [mutating, setMutating] = useState(false);
  const [loggingOut, setLoggingOut] = useState(false);
  const stream = useRef<AbortController | null>(null);
  const inFlight = useRef(false);
  const epoch = useRef(0);
  const bottom = useRef<HTMLDivElement>(null);
  const scroller = useRef<HTMLDivElement>(null);
  const follow = useRef(true);
  const selectedModel = models.find(m => m.id === (model === 'auto' ? defaultModel : model));
  const pending = generating || messages.some(m => m.status === 'generating');
  const lastAssistant = messages.findLast(m => m.role === 'ASSISTANT');
  function fail(e: unknown) { setError(e instanceof Error ? e.message : 'Something went wrong. Please retry.'); }
  function upsert(c: Conversation) { setHistory(items => [c, ...items.filter(item => item.id !== c.id)].sort((a, b) => b.updatedAt.localeCompare(a.updatedAt))); }
  useEffect(() => {
    let alive = true;
    async function boot() {
      try {
        const [list, configuration] = await Promise.all([
          chatRequest<{ items: Conversation[]; hasMore: boolean }>(scoped('/conversations')),
          chatRequest<{ models: Model[]; defaultModel: string }>('/models'),
        ]);
        if (!alive) return;
        setHistory(list.items); setHistoryMore(list.hasMore); setModels(configuration.models); setDefaultModel(configuration.defaultModel);
        const params=new URLSearchParams(window.location.search);if(params.get('mode')==='research')setMode('research');const docs=params.get('docs');if(docs)setSelectedDocuments(docs.split(',').filter(x=>/^[0-9a-f-]{36}$/.test(x)).slice(0,5));const prompt=params.get('prompt');if(prompt)setDraft(prompt.slice(0,8000));const id = params.get('c');
        if (id) {
          const detail = await chatRequest<ConversationDetail>(`/conversations/${encodeURIComponent(id)}`);
          if((detail.conversation.spaceId??null)!==spaceId()){const target=new URLSearchParams({c:id});if(detail.conversation.spaceId)target.set('space',detail.conversation.spaceId);window.location.replace('/chat?'+target.toString());return;}
          if (alive) { setActive(detail.conversation); setMessages(detail.messages); setMessagesMore(detail.hasMore); setSelectedDocuments(detail.messages.findLast(m=>m.role==='USER')?.documentIds??[]); }
        }
      } catch (e) { if (alive) fail(e); }
      finally { if (alive) setLoading(false); }
    }
    void boot();
    const verify = async () => {
      try { await apiRequest('/auth/session'); }
      catch (e) { if ((e as { status?: number }).status === 401) window.location.replace('/login'); }
    };
    const timer = setInterval(verify, 60000); window.addEventListener('focus', verify);
    return () => { alive = false; clearInterval(timer); window.removeEventListener('focus', verify); stream.current?.abort(); };
  }, []);
  useEffect(() => {
    if (follow.current) bottom.current?.scrollIntoView({ block: 'end' });
  }, [messages]);
  useEffect(() => {
    if (!active || generating || !messages.some(m => m.status === 'generating')) return;
    let alive = true;
    const timer = setInterval(async () => {
      try { const detail = await chatRequest<ConversationDetail>(`/conversations/${active.id}`); if (alive) setMessages(detail.messages); }
      catch { /* Existing error stays visible; saved partials remain on screen. */ }
    }, 2000);
    return () => { alive = false; clearInterval(timer); };
  }, [active, generating, messages]);
  async function openConversation(id: string) {
    if (inFlight.current) return;
    const current = ++epoch.current; setLoading(true); setError(''); setNotice('');
    try {
      const detail = await chatRequest<ConversationDetail>(`/conversations/${id}`);
      if (current !== epoch.current) return;
      setActive(detail.conversation); setMessages(detail.messages); setMessagesMore(detail.hasMore); setDraft(''); setSidebar(false); follow.current = true;
      setSelectedDocuments(detail.messages.findLast(m=>m.role==='USER')?.documentIds??[]);
      window.history.replaceState(null, '', chatUrl(id));
    } catch (e) { fail(e); }
    finally { if (current === epoch.current) setLoading(false); }
  }
  function newChat() {
    if (inFlight.current) return;
    epoch.current++; setActive(null); setMessages([]); setMessagesMore(false); setDraft(''); setError(''); setNotice('A fresh workspace is ready. Send a message to start your conversation.'); setSidebar(false); setLoading(false);
    window.history.replaceState(null, '', chatUrl());
    setSelectedDocuments([]);
  }
  async function generate(regenerate = false) {
    if (inFlight.current || pending || (!regenerate && !draft.trim())) return;
    if (!selectedModel) { setError('No default model is available. Select a configured model or open Settings.'); return; }
    inFlight.current = true; setGenerating(true); setError(''); setNotice(''); follow.current = true;
    const controller = new AbortController(); stream.current = controller;
    let conversation = active; let assistantId = '';
    try {
      if (!conversation) { conversation = await chatRequest<Conversation>('/conversations', 'POST', {spaceId:spaceId()}); setActive(conversation); upsert(conversation); window.history.replaceState(null, '', chatUrl(conversation.id)); }
      await streamChat(`/conversations/${conversation.id}/${regenerate ? 'regenerate' : 'messages'}`, { content: regenerate ? undefined : draft, model, mode, documentIds: regenerate ? undefined : selectedDocuments }, controller.signal, (type, data) => {
        if (type === 'start') {
          const c = data.conversation as Conversation; const assistant = data.message as ChatMessage; assistantId = assistant.id;
          setActive(c); upsert(c); if (!regenerate) setDraft('');
          setMessages(items => [...items.filter(m => m.id !== data.replacesId), ...(data.userMessage ? [data.userMessage as ChatMessage] : []), assistant]);
        } else if(type==='progress'){setNotice(String(data.message));} else if (type === 'delta') {
          setMessages(items => items.map(m => m.id === assistantId ? { ...m, content: m.content + String(data.text ?? '') } : m));
        } else if (type === 'done') {
          const message = data.message as ChatMessage; setMessages(items => items.map(m => m.id === message.id ? message : m));
        } else if (type === 'error') {
          if (data.code === 'cancelled') setNotice(String(data.message)); else setError(String(data.message));
        }
      });
    } catch (e) { if ((e as Error).name !== 'AbortError') fail(e); else setNotice('Connection stopped. Reload to check the last saved response.'); }
    finally {
      if (conversation) {
        try { const detail = await chatRequest<ConversationDetail>(`/conversations/${conversation.id}`); setMessages(detail.messages); setActive(detail.conversation); setMessagesMore(detail.hasMore); upsert(detail.conversation); }
        catch { setError('Unable to reload the saved response. Use Reload conversation when the connection returns.'); }
      }
      stream.current = null; inFlight.current = false; setGenerating(false);
    }
  }
  async function stop() {
    if (!active) return;
    try { await chatRequest(`/conversations/${active.id}/stop`, 'POST', {}); setNotice('Stopping generation…'); }
    catch (e) { stream.current?.abort(); fail(e); }
  }
  async function saveDialog() {
    if (!dialog) return; setMutating(true); setError('');
    try {
      if (dialog.kind === 'delete') {
        await chatRequest(`/conversations/${dialog.conversation.id}`, 'DELETE');
        setHistory(items => items.filter(c => c.id !== dialog.conversation.id)); if (active?.id === dialog.conversation.id) newChat();
      } else {
        const updated = await chatRequest<Conversation>(`/conversations/${dialog.conversation.id}`, 'PATCH', { title: title.trim() });
        upsert(updated); if (active?.id === updated.id) setActive(updated);
      }
      setDialog(null);
    } catch (e) { fail(e); } finally { setMutating(false); }
  }
  async function loadMoreHistory() {
    try { const list = await chatRequest<{ items: Conversation[]; hasMore: boolean }>(scoped(`/conversations?offset=${history.length}`)); setHistory(items => [...items, ...list.items.filter(c => !items.some(i => i.id === c.id))]); setHistoryMore(list.hasMore); }
    catch (e) { fail(e); }
  }
  async function loadOlder() {
    if (!active || !messages.length) return;
    try { const detail = await chatRequest<ConversationDetail>(`/conversations/${active.id}?before=${messages[0].sequence}`); follow.current = false; setMessages(items => [...detail.messages, ...items]); setMessagesMore(detail.hasMore); }
    catch (e) { fail(e); }
  }
  async function logout() {
    setLoggingOut(true);
    try { if (active && generating) await chatRequest(`/conversations/${active.id}/stop`, 'POST', {}); await apiRequest('/auth/logout', {}); window.location.replace('/login'); }
    catch (e) { fail(e); setLoggingOut(false); }
  }
  return <div className="sp-app">
    <input ref={attachment} className="sr-only" type="file" aria-label="Chat attachment" accept=".pdf,.txt,.docx" onChange={e=>{const file=e.target.files?.[0];e.target.value='';if(file)void uploadDocument(file);}}/>
    <DocumentsPanel open={documentsOpen} onClose={()=>setDocumentsOpen(false)} documents={documents} refresh={refreshDocuments} selected={selectedDocuments} onSelect={setSelectedDocuments} onUpload={uploadDocument} busy={uploading} uploadError={error}/>
    {sidebar && <button className="sp-overlay" aria-label="Close navigation" onClick={() => setSidebar(false)} />}
    <aside className={`sp-sidebar ${sidebar ? 'is-open' : ''}`}>
      <div className="sp-brand"><span className="sp-brand-symbol"><Bot size={25} /></span><div>Spilton <b>AI</b><small>A little clarity. A lot of possibility.</small></div><button className="sp-mobile-close" aria-label="Close sidebar" onClick={() => setSidebar(false)}><X size={18}/></button></div>
      <button className="sp-new" aria-label="New Chat" disabled={!hydrated || generating} onClick={newChat}><Plus size={18} /> New Chat <span aria-hidden="true">＋</span></button>
      <button className="sp-search" disabled><Search size={15} /> Search chats <small>Soon</small></button>
      <button className="sp-nav" onClick={()=>{setDocumentsOpen(true);void refreshDocuments().catch(fail);setSidebar(false);}}><Files size={17}/> Documents</button>
      <Link className="sp-nav" href={hydrated?"/prepare"+(spaceId()?"?space="+spaceId():""):"/prepare"}><Home size={17}/> Dashboard · Spaces</Link><Link className="sp-nav" href={hydrated?"/government"+(spaceId()?"?space="+spaceId():""):"/government"}><BookOpen size={17}/> Notifications · PYQs · Affairs</Link><Link className="sp-nav" href={hydrated?"/mocks"+(spaceId()?"?space="+spaceId():""):"/mocks"}><ListChecks size={17}/> Mock Tests · Practice</Link><nav aria-label="Main navigation"><button className="sp-nav active" disabled={generating} onClick={newChat}><Home size={17} /> Home</button></nav>
      <div className="sp-history"><div className="sp-section-label">CHAT HISTORY</div>{loading && !history.length && <p className="sp-muted">Loading conversations…</p>}{!loading && !history.length && <p className="sp-history-empty">Your next idea starts with a conversation.</p>}
        {['Today', 'Yesterday', 'Earlier'].map(group => { const items = history.filter(c => dayGroup(c.updatedAt) === group); return items.length ? <section key={group}><h2>{group}</h2>{items.map(c => <div className={`sp-history-row ${active?.id === c.id ? 'selected' : ''}`} key={c.id}>
          <button className="sp-history-open" disabled={generating} onClick={() => openConversation(c.id)} title={c.title}><MessageSquare size={14} /><span>{c.title}</span></button>
          <button className="sp-history-action" disabled={pending} aria-label={`Rename ${c.title}`} onClick={() => { setTitle(c.title); setDialog({ kind: 'rename', conversation: c }); }}><Pencil size={13}/></button>
          <button className="sp-history-action" disabled={pending} aria-label={`Delete ${c.title}`} onClick={() => setDialog({ kind: 'delete', conversation: c })}><Trash2 size={13}/></button>
        </div>)}</section> : null; })}
        {historyMore && <button className="sp-link" onClick={loadMoreHistory}>Load more conversations</button>}
      </div>
      <div className="sp-sidebar-bottom"><button className="sp-nav" onClick={() => setSettings(true)}><Settings size={17}/> Settings <ChevronRight size={14}/></button><div className="sp-profile"><span className="sp-avatar">{user.name[0]?.toUpperCase()}</span><div><strong>{user.name}</strong><small title={user.email}>{user.email}</small></div><button disabled={!hydrated || loggingOut} onClick={logout}>{loggingOut ? 'Signing out…' : 'Log out'}</button></div></div>
    </aside>
    <main className="sp-main"><header className="sp-header"><button className="sp-menu" aria-label="Open sidebar" onClick={() => setSidebar(true)}><Menu size={21}/></button><div className="sp-breadcrumb">Workspace <span>/</span> <strong>{active?.title || 'New conversation'}</strong></div><span className="sp-header-badge"><span/> {selectedModel?.isDevelopment ? 'Development preview' : 'Chat workspace'}</span><button className="sp-settings-button" aria-label="Model settings" onClick={() => setSettings(true)}><Settings size={18}/></button></header>
      <div className="sp-chat-layout"><div className="sp-chat-column">
        <div className="sp-message-scroll" ref={scroller} onScroll={() => { const el = scroller.current; if (el) follow.current = el.scrollHeight - el.scrollTop - el.clientHeight < 160; }}>
          {loading ? <div className="sp-loading" role="status"><span className="sp-spinner"/> Opening your workspace…</div> : !messages.length ? <section className="sp-welcome"><div className="sp-welcome-logo"><Bot size={40}/></div><p className="sp-eyebrow">MAKE ROOM FOR YOUR NEXT IDEA</p><p className="sp-greeting">Welcome to Spilton, {user.name.split(' ')[0]}.</p><h1>What do you want<br/>to accomplish?</h1><p className="sp-welcome-sub">A question, a fresh perspective, or a place to start.<br/>Let’s take the next step together.</p><div className="sp-suggestions">{[[Zap, 'Understand a concept', 'What is polymorphism?'], [Files, 'Write a little code', 'Explain Python lists with a code example.'], [Sparkles, 'Break it down', 'Explain how percentages work.']].map(([Icon, label, prompt]) => { const I = Icon as typeof Zap; return <button key={String(label)} onClick={() => setDraft(String(prompt))}><I size={19}/><strong>{String(label)}</strong><span>{String(prompt)}</span><ChevronRight size={15}/></button>; })}</div></section> : <div className="sp-messages">{messagesMore && <button className="sp-load-older" onClick={loadOlder}>Load earlier messages</button>}{messages.map(message => <article key={message.id} className={`sp-message ${message.role === 'USER' ? 'is-user' : 'is-assistant'}`} data-message-role={message.role} data-status={message.status}>
          <div className="sp-message-heading"><span className={`sp-message-avatar ${message.role === 'USER' ? 'user' : ''}`}>{message.role === 'USER' ? user.name[0] : <Bot size={18}/>}</span><strong>{message.role === 'USER' ? 'You' : 'Spilton'}</strong>{message.role === 'ASSISTANT' && <small>{message.modelProvider === 'Development' ? 'Development demo · not real AI' : message.modelName}</small>}</div>
          <div className="sp-message-content">{message.role === 'USER' ? <p className="sp-user-text">{message.content}</p> : <>{Boolean(message.documentIds?.length)&&<p className="sp-document-message-label">Document-grounded · selected sources only</p>}<MarkdownMessage content={message.content}/><Citations items={message.citations??[]}/>{message.status === 'generating' && <span className="sp-generating" role="status"><span/> Generating response…</span>}{message.status === 'failed' && <p className="sp-message-warning">Response incomplete. You can regenerate the latest answer.</p>}{message.status === 'cancelled' && <p className="sp-message-warning">Stopped · partial response saved</p>}
            {message.status !== 'generating' && <div className="sp-message-actions"><CopyButton text={message.content}/>{hasNamedProjectFiles(message.content)&&<DownloadProjectButton content={message.content} projectName={active?.title||'spilton-project'}/>} {lastAssistant?.id === message.id && <button className="sp-icon-text" disabled={pending} onClick={() => generate(true)}><RotateCcw size={14}/> Regenerate response</button>}</div>}</> }</div>
        </article>)}</div>}
        <div ref={bottom}/></div>
        <div className="sp-compose-zone">{error && <div className="error-message sp-error" role="alert"><span>{error}</span>{active && !generating && <button onClick={() => openConversation(active.id)}>Reload conversation</button>}<button aria-label="Dismiss error" onClick={() => setError('')}><X size={14}/></button></div>}{notice && <p className="sp-notice" role="status">{notice}</p>}
          <p className="sp-document-context">{selectedDocuments.length?mode==='research'?`Research · web plus ${selectedDocuments.length} selected document${selectedDocuments.length===1?'':'s'}.`:`Document-grounded chat · ${selectedDocuments.length} selected. Excerpts go to the selected provider.`:mode==='research'?"Advanced research · current public sources with provenance":"General chat · no documents selected"}</p><div className="sp-selected-docs">{selectedDocuments.map(id=><button key={id} aria-label={`Remove selected ${documents.find(d=>d.id===id)?.name||"document"}`} onClick={()=>setSelectedDocuments(ids=>ids.filter(x=>x!==id))}>{documents.find(d=>d.id===id)?.name||"Unavailable document"} · {documents.find(d=>d.id===id)?.status||"Unavailable"} ×</button>)}</div><form className="sp-composer" onSubmit={e => { e.preventDefault(); void generate(); }}><label className="sr-only" htmlFor="message">Message</label><textarea id="message" placeholder="Ask Spilton anything…" value={draft} onChange={e => setDraft(e.target.value)} maxLength={8000} disabled={!hydrated || loading || pending} rows={3} onKeyDown={e => { if (e.key === 'Enter' && !e.shiftKey && !e.nativeEvent.isComposing) { e.preventDefault(); void generate(); } }}/><div className="sp-composer-tools"><div className="sp-composer-left"><button type="button" disabled={pending||uploading} title="Upload PDF, TXT or DOCX" aria-label="Attach file" onClick={()=>attachment.current?.click()}><Paperclip size={18}/></button><button type="button" className="sp-web" title="Use current public web sources" onClick={()=>setMode('research')}><Globe size={15}/> Web <small>Search</small></button><div className="sp-mode-select"><Zap size={14}/><select aria-label="AI mode" value={mode} onChange={e=>setMode(e.target.value)} disabled={pending}><option value="quick">Quick</option><option value="think">Think · careful answers</option><option value="research">Research · web + documents</option><option value="agent">Agent · safe tools</option></select></div></div><div className="sp-composer-right"><select className="sp-model-select" aria-label="Model" value={model} onChange={e => setModel(e.target.value)} disabled={pending || loading}><option value="auto">Spilton Auto{selectedModel?.isDevelopment ? ' · Demo' : ''}</option>{models.map(m => <option key={m.id} value={m.id}>{m.label}</option>)}</select>{pending ? <button className="sp-send sp-stop" type="button" aria-label="Stop generation" onClick={stop}><Square size={18} fill="currentColor"/></button> : <button className="sp-send" type="submit" aria-label="Send message" disabled={!hydrated || loading || !draft.trim() || !selectedModel}><Send size={19}/></button>}</div></div></form>
          <p className="sp-composer-footnote">{selectedModel?.isDevelopment ? 'Development provider is active. Responses are scripted demonstrations, not real AI.' : 'AI can make mistakes. Check important information.'}<span>{draft.length}/8000</span></p>
        </div>
      </div>{!messages.length && <aside className="sp-insights"><div className="sp-insight-card"><span className="sp-mini-label">YOUR WORKSPACE</span><h2>A good place to begin.</h2><p>Your conversations stay with your account. Pick up where you left off, whenever you’re ready.</p><div className="sp-feature-line"><MessageSquare size={17}/><span>Saved conversations</span><span className="sp-enabled">Ready</span></div><div className="sp-feature-line"><Zap size={17}/><span>Quick mode</span><span className="sp-enabled">Ready</span></div></div><div className="sp-insight-card sp-coming-card"><div className="sp-card-icon"><BookOpen size={21}/></div><h2>Built for what’s next.</h2><p>Ask general questions, write code, learn a topic or work with your documents. Exam preparation is one specialized workspace.</p><span className="sp-coming-badge">Coming Soon</span></div><div className="sp-provider-card"><span className="sp-mini-label">CONNECTED MODEL</span><strong>{selectedModel?.label || 'No model configured'}</strong><p>{selectedModel?.isDevelopment ? 'Try streaming and saved chats with the development demo.' : 'Your selection is configured by the server administrator.'}</p><button onClick={() => setSettings(true)}>View model settings <ChevronRight size={14}/></button></div></aside>}</div>
    </main>
    {dialog && <div className="sp-modal-backdrop"><section role="dialog" aria-modal="true" aria-labelledby="conversation-dialog-title" className="sp-modal"><h2 id="conversation-dialog-title">{dialog.kind === 'rename' ? 'Rename conversation' : 'Delete conversation?'}</h2>{dialog.kind === 'rename' ? <label>Conversation title<input autoFocus value={title} maxLength={100} onChange={e => setTitle(e.target.value)}/></label> : <p>“{dialog.conversation.title}” and its messages will be permanently deleted.</p>}<div className="sp-modal-actions"><button disabled={mutating} onClick={() => setDialog(null)}>Cancel</button><button className={dialog.kind === 'delete' ? 'danger' : 'confirm'} disabled={mutating || (dialog.kind === 'rename' && !title.trim())} onClick={saveDialog}>{mutating ? 'Saving…' : dialog.kind === 'delete' ? 'Delete conversation' : 'Save title'}</button></div></section></div>}
    {settings && <div className="sp-modal-backdrop"><section role="dialog" aria-modal="true" aria-labelledby="model-settings-title" className="sp-modal"><button className="sp-modal-close" aria-label="Close settings" onClick={() => setSettings(false)}><X size={19}/></button><span className="sp-mini-label">SPILTON SETTINGS</span><h2 id="model-settings-title">Your model connection</h2><p>Spilton Auto uses the configured default model. Only server-configured models appear in the selector.</p>{models.length ? models.map(m => <div className="sp-model-card" key={m.id}><strong>{m.label}</strong><small>{m.isDevelopment ? 'Scripted demo · no real AI connection' : 'Configured transport · access depends on provider credentials'}</small></div>) : <p>No model is configured.</p>}<p className="sp-settings-note">A server administrator can configure a compatible provider through the private environment file. API keys stay on the server and are never entered in this interface.</p><p className="sp-settings-note">Research uses selected documents when attached and current public web sources otherwise. Think uses the same model with a careful-answer instruction. Agent runs bounded read-only tools. Documents support PDF, TXT and DOCX.</p><button className="sp-settings-done" onClick={() => setSettings(false)}>Done</button></section></div>}
  </div>;
}


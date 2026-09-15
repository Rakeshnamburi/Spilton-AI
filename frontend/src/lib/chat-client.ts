import { ApiError } from './api-client';
import { sessionFetch } from './session-fetch';
export type Conversation = { id: string; spaceId?:string|null; title: string; createdAt: string; updatedAt: string };
export type Citation = { number: number; documentId: string; chunkId: string; name: string; page: number | null; section: string | null; excerpt: string; url?:string|null; retrievedAt?:string|null; sourceType?:string|null };
export type ChatMessage = { id: string; sequence: number; role: 'USER' | 'ASSISTANT' | 'SYSTEM'; content: string; modelProvider: string | null; modelName: string | null; status: string; createdAt: string; documentIds?: string[]; citations?: Citation[] };
export type Model = { id: string; label: string; provider: string; model: string; isDevelopment: boolean };
export type ConversationDetail = { conversation: Conversation; messages: ChatMessage[]; hasMore: boolean };
export async function chatRequest<T>(path: string, method = 'GET', body?: unknown): Promise<T> {
  let response: Response;
  try { response = await sessionFetch(`/api/chat${path}`, { method, headers: { 'Content-Type': 'application/json' }, body: body === undefined ? undefined : JSON.stringify(body), cache: 'no-store', signal: AbortSignal.timeout(15000) }); }
  catch { throw new ApiError('Connection unavailable. Please retry.', 0); }
  if (!response.ok) {
    const data = await response.json().catch(() => ({}));
    if (response.status === 401) window.location.replace('/login'+(window.location.pathname==='/mocks'?'?returnTo='+encodeURIComponent(window.location.pathname+window.location.search):''));
    throw new ApiError(data.title || 'Unable to complete the request.', response.status);
  }
  return response.status === 204 ? undefined as T : response.json();
}
export type ChatEvent = { type: string; data: { conversation?: Conversation; userMessage?: ChatMessage; message?: ChatMessage; replacesId?: string; text?: string; code?: string; saved?: boolean; messageText?: string } };
// The payload differs for errors; keep parsing explicit instead of silently dropping terminal events.
export async function streamChat(path: string, body: unknown, signal: AbortSignal, onEvent: (event: string, data: Record<string, unknown>) => void) {
  const response = await sessionFetch(`/api/chat${path}`, { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(body), signal, cache: 'no-store' });
  if (!response.ok) {
    const data = await response.json().catch(() => ({}));
    if (response.status === 401) window.location.replace('/login'+(window.location.pathname==='/mocks'?'?returnTo='+encodeURIComponent(window.location.pathname+window.location.search):''));
    throw new Error(data.title || 'Unable to start a response. Please try again.');
  }
  if (!response.body || !response.headers.get('content-type')?.includes('text/event-stream')) throw new Error('Invalid streaming response. Please reload.');
  const reader = response.body.getReader(); const decoder = new TextDecoder(); let buffer = ''; let completed = false;
  try {
    while (true) {
      const { done, value } = await reader.read();
      buffer += decoder.decode(value, { stream: !done }).replace(/\r\n/g, '\n');
      let split: number;
      while ((split = buffer.indexOf('\n\n')) >= 0) {
        const frame = buffer.slice(0, split); buffer = buffer.slice(split + 2);
        const type = frame.split('\n').find(l => l.startsWith('event:'))?.slice(6).trim();
        const data = frame.split('\n').filter(l => l.startsWith('data:')).map(l => l.slice(5).trim()).join('\n');
        if (type && data) { onEvent(type, JSON.parse(data)); if (type === 'done') completed = true; }
      }
      if (buffer.length > 150000) throw new Error('Invalid response size. Please reload.');
      if (done) break;
    }
    if (!completed) throw new Error('The connection ended before completion. Reload to recover saved messages, or regenerate the answer.');
  } finally { reader.releaseLock(); }
}

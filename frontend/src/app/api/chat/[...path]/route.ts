import { NextRequest, NextResponse } from 'next/server';
import { cookies } from 'next/headers';
import { AUTH_COOKIE, backendRequest, isAllowedOrigin } from '@/lib/backend';
export const dynamic = 'force-dynamic';
export const maxDuration = 140;
type Context = { params: Promise<{ path: string[] }> };
async function proxy(request: NextRequest, { params }: Context) {
  const path = (await params).path.join('/');
  if (path === 'agents/run') { /* explicitly allow the bounded agent endpoint */ }
  if (!/^(capabilities|tools(?:\/(?:calculator|date_time))?|mocks\/(?:catalog|practice|questions|tests\/[0-9a-f-]{36}\/attempts|attempts(?:\/[0-9a-f-]{36}(?:\/(?:submit|tutor|answers\/[0-9a-f-]{36}))?)?|papers\/[0-9a-f-]{36}\/(?:status|questions|convert))|government\/(?:resources(?:\/[0-9a-f-]{36}(?:\/(?:eligibility|download))?)?|compare|research-status)|spaces(?:\/[0-9a-f-]{36})?|preparation\/(?:catalog|dashboard|profile|goals|memories|plans|progress|plan-items)(?:\/[0-9a-f-]{36})?|models|conversations(?:\/[0-9a-f-]{36}(?:\/(messages|regenerate|stop))?)?|documents(?:\/[0-9a-f-]{36}(?:\/chunks\/[0-9a-f-]{36})?)?)$/.test(path)) return new Response(null, { status: 404 });
  const token = (await cookies()).get(AUTH_COOKIE)?.value;
  if (!token) return NextResponse.json({ title: 'Please sign in again.' }, { status: 401 });
  if (request.method !== 'GET' && !isAllowedOrigin(request.headers.get('origin')))
    return NextResponse.json({ title: 'Request origin is not allowed.' }, { status: 403 });
  const upload = path === 'documents' && request.method === 'POST';
  const contentType = request.headers.get('content-type') || 'application/json';
  if (upload && !contentType.startsWith('multipart/form-data;')) return NextResponse.json({title:'Use the file upload form.'},{status:415});
  let body: ArrayBuffer | undefined;
  if (request.body) {
    const reader = request.body.getReader(); const chunks: Uint8Array[] = []; let size = 0;
    while (true) {
      const { done, value } = await reader.read(); if (done) break;
      size += value.byteLength;
      if (size > (upload ? 5300000 : 40000)) { await reader.cancel(); return NextResponse.json({ title: upload ? 'Files must be 5 MB or smaller.' : 'Message is too large.' }, { status: 413 }); }
      chunks.push(value);
    }
    body = Uint8Array.from(Buffer.concat(chunks)).buffer;
  }
  try {
    const upstream = await backendRequest(`/${path}${request.nextUrl.search}`, {
      method: request.method, headers: { Authorization: `Bearer ${token}`, 'Content-Type': contentType }, body,
      signal: AbortSignal.any([request.signal, AbortSignal.timeout(135000)]),
    });
    if (!upstream.ok) {
      const problem = await upstream.json().catch(() => ({}));
      return NextResponse.json({ title: problem.title || (upstream.status === 429 ? 'Too many requests. Please wait a minute.' : 'The request could not be completed.'), errors: problem.errors }, { status: upstream.status, headers: { 'Cache-Control': 'no-store' } });
    }
    return new Response(upstream.body, { status: upstream.status, headers: { 'Content-Type': upstream.headers.get('content-type') || 'application/json', 'Cache-Control': 'no-store, no-transform', 'X-Accel-Buffering': 'no',...(upstream.headers.get('content-disposition')?{'Content-Disposition':upstream.headers.get('content-disposition')!}:{}),'X-Content-Type-Options':'nosniff' } });
  } catch { return NextResponse.json({ title: 'Spilton is temporarily unavailable. Your saved conversations are safe. Please retry.' }, { status: 503 }); }
}
export { proxy as GET, proxy as POST, proxy as PATCH, proxy as DELETE };

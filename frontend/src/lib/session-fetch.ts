// Serialize refresh within and across tabs so a simultaneous renewal cannot
// replay a rotated credential. Tokens stay in HttpOnly cookies.
let renewal: Promise<boolean> | null = null;
async function renew(): Promise<boolean> {
  const run = async () => {
    const current = await fetch('/api/auth/session', { cache: 'no-store' });
    if (current.ok) return true;
    if (current.status !== 401) return false;
    return (await fetch('/api/auth/refresh', { method: 'POST', cache: 'no-store' })).ok;
  };
  return navigator.locks ? navigator.locks.request('spilton-session-renewal', run) : false;
}
export async function sessionFetch(input: RequestInfo | URL, init?: RequestInit): Promise<Response> {
  const response = await fetch(input, init);
  if (response.status !== 401 || init?.signal?.aborted) return response;
  renewal ??= renew().catch(() => false).finally(() => { renewal = null; });
  if (!await renewal) return response;
  return fetch(input, init);
}

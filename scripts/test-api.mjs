import { test } from 'node:test';
import assert from 'node:assert/strict';
import { randomBytes, randomUUID } from 'node:crypto';
const base = process.env.TEST_API_URL || 'http://localhost:5081';
const email = `api-${randomUUID()}@example.test`;
const password = randomBytes(24).toString('base64url');
async function request(path, body, token, headers = {}) {
  return fetch(`${base}${path}`, { method: body ? 'POST' : 'GET', headers: { 'Content-Type': 'application/json', ...(token ? { Authorization: `Bearer ${token}` } : {}), ...headers }, body: body ? JSON.stringify(body) : undefined, signal: AbortSignal.timeout(15000) });
}
test('Day 1 API integration against real PostgreSQL', async t => {
  let token, userId;
  await t.test('health confirms PostgreSQL connection', async () => {
    const r = await request('/api/health'); assert.equal(r.status, 200); assert.equal((await r.json()).database, 'connected');
  });
  await t.test('OpenAPI describes auth endpoints', async () => {
    const r = await request('/openapi/v1.json'); assert.equal(r.status, 200);
    const d = await r.json(); for (const path of ['/api/auth/register', '/api/auth/login', '/api/auth/me', '/api/health']) assert.ok(d.paths[path]);
  });
  await t.test('protected endpoint rejects missing JWT', async () => { assert.equal((await request('/api/auth/me')).status, 401); });
  await t.test('protected endpoint rejects malformed JWT', async () => { assert.equal((await request('/api/auth/me', undefined, 'invalid')).status, 401); });
  await t.test('registration validates input', async () => { assert.equal((await request('/api/auth/register', { name: ' ', email: 'bad', password: 'short' })).status, 400); });
  await t.test('register persists user and returns JWT without password data', async () => {
    const r = await request('/api/auth/register', { name: 'API Test', email, password }); assert.equal(r.status, 201);
    const d = await r.json(); token = d.accessToken; userId = d.user.id; assert.ok(token); assert.deepEqual(d.user.roles, ['User']);
    assert.equal(d.user.email, email); assert.equal('passwordHash' in d.user, false); assert.equal('password' in d.user, false);
  });
  await t.test('case-insensitive duplicate email returns conflict', async () => { assert.equal((await request('/api/auth/register', { name: 'Duplicate', email: email.toUpperCase(), password })).status, 409); });
  await t.test('invalid password returns unauthorized', async () => { assert.equal((await request('/api/auth/login', { email, password: 'definitely-wrong-password' })).status, 401); });
  await t.test('unknown email has same unauthorized response', async () => { const r = await request('/api/auth/login', { email: `unknown-${email}`, password }); assert.equal(r.status, 401); assert.equal((await r.json()).title, 'Invalid email or password.'); });
  await t.test('login reads stored user and validates password', async () => {
    const r = await request('/api/auth/login', { email: email.toUpperCase(), password }); assert.equal(r.status, 200);
    const d = await r.json(); token = d.accessToken; assert.equal(d.user.id, userId);
  });
  await t.test('protected endpoint accepts JWT and reads user', async () => {
    const r = await request('/api/auth/me', undefined, token); assert.equal(r.status, 200); assert.equal((await r.json()).id, userId);
  });
  await t.test('tampered JWT rejected', async () => {
    const parts = token.split('.'); parts[1] = Buffer.from(JSON.stringify({ sub: userId, role: 'Admin', exp: 9999999999 })).toString('base64url');
    assert.equal((await request('/api/auth/me', undefined, parts.join('.'))).status, 401);
  });
  await t.test('CORS allows configured frontend only', async () => {
    const good = await request('/api/health', undefined, undefined, { Origin: 'http://localhost:3000' }); assert.equal(good.headers.get('access-control-allow-origin'), 'http://localhost:3000');
    const bad = await request('/api/health', undefined, undefined, { Origin: 'https://untrusted.example' }); assert.equal(bad.headers.get('access-control-allow-origin'), null);
  });
});

import { test } from 'node:test';
import assert from 'node:assert/strict';
import { randomUUID, randomBytes } from 'node:crypto';
const base = process.env.TEST_API_URL || 'http://localhost:5081';
async function req(path, token, method = 'GET', body) {
  if(method==='POST' && /\/(messages|regenerate)$/.test(path)) body={model:'development',...body};
  return fetch(base + '/api' + path, { method, headers: { 'Content-Type': 'application/json', ...(token ? { Authorization: `Bearer ${token}` } : {}) }, body: body === undefined ? undefined : JSON.stringify(body), signal: AbortSignal.timeout(20000) });
}
async function account() {
  const credentials = { name: 'Chat Test', email: `chat-${randomUUID()}@example.test`, password: randomBytes(24).toString('base64url') };
  const r = await req('/auth/register', null, 'POST', credentials); assert.equal(r.status, 201);
  return { ...await r.json(), credentials };
}
async function collect(response, onEvent = () => {}) {
  assert.equal(response.status, 200); assert.match(response.headers.get('content-type'), /text\/event-stream/);
  let buffer = ''; const events = []; const decoder = new TextDecoder();
  for await (const chunk of response.body) {
    buffer += decoder.decode(chunk, { stream: true });
    let index;
    while ((index = buffer.indexOf('\n\n')) >= 0) {
      const frame = buffer.slice(0, index); buffer = buffer.slice(index + 2);
      const type = frame.match(/^event: (.+)$/m)?.[1]; const data = frame.match(/^data: (.+)$/m)?.[1];
      if (type && data) { const event = { type, data: JSON.parse(data), at: Date.now() }; events.push(event); await onEvent(event); }
    }
  }
  return events;
}
test('Day 2 live API + PostgreSQL, DEVELOPMENT provider (not real AI)', async t => {
  const a = await account(); const b = await account(); let id; let originalAnswer;
  await t.test('all chat endpoints require authentication', async () => {
    for (const [path, method] of [['/conversations','GET'],['/conversations','POST'],['/models','GET'],[`/conversations/${randomUUID()}/messages`,'POST']]) assert.equal((await req(path,null,method,method==='POST'?{}:undefined)).status,401);
  });
  await t.test('only configured models listed; development truthfully labelled', async () => {
    const data = await (await req('/models',a.accessToken)).json(); assert.ok(data.models.some(m=>m.id==='development'&&m.isDevelopment)); assert.ok(data.models.every(m=>!('apiKey' in m)&&!('baseUrl' in m)));
  });
  await t.test('create conversation', async () => { const r=await req('/conversations',a.accessToken,'POST',{}); assert.equal(r.status,201); id=(await r.json()).id; });
  await t.test('cross-user get/rename/delete/send/regenerate/stop all hidden', async () => {
    for(const [tail,method,body] of [['','GET'],['','PATCH',{title:'stolen'}],['','DELETE'],['/messages','POST',{content:'intrusion'}],['/regenerate','POST',{}],['/stop','POST',{}]]) assert.equal((await req(`/conversations/${id}${tail}`,b.accessToken,method,body)).status,404);
    const list=await(await req('/conversations',b.accessToken)).json(); assert.ok(!list.items.some(c=>c.id===id));
  });
  await t.test('unconfigured model rejected without persisting a message', async () => {
    assert.equal((await req(`/conversations/${id}/messages`,a.accessToken,'POST',{content:'Hello',model:'missing'})).status,503);
    assert.equal((await(await req(`/conversations/${id}`,a.accessToken)).json()).messages.length,0);
  });
  await t.test('future modes, blank and oversize inputs rejected', async () => {
    for(const body of [{content:'Hello',mode:'research'},{content:' '},{content:'x'.repeat(8001)}]) assert.equal((await req(`/conversations/${id}/messages`,a.accessToken,'POST',body)).status,400);
  });
  await t.test('stream progressively and persist both messages', async () => {
    const events=await collect(await req(`/conversations/${id}/messages`,a.accessToken,'POST',{content:'Explain Python lists with an example',model:'development'}));
    assert.equal(events[0].type,'start'); assert.equal(events.at(-1).type,'done');
    const deltas=events.filter(e=>e.type==='delta'); assert.ok(deltas.length>5); assert.ok(deltas.at(-1).at-deltas[0].at>300);
    const detail=await(await req(`/conversations/${id}`,a.accessToken)).json(); assert.equal(detail.messages.length,2); assert.equal(detail.messages[0].role,'USER');
    originalAnswer=detail.messages[1].id; assert.equal(detail.messages[1].status,'completed'); assert.match(detail.messages[1].content,/not a real AI/); assert.equal(detail.messages[1].modelProvider,'Development');
    assert.equal(detail.conversation.title,'Explain Python lists with an example');
  });
  await t.test('regeneration replaces display answer without duplicating user input', async () => {
    await collect(await req(`/conversations/${id}/regenerate`,a.accessToken,'POST',{model:'development'}));
    const d=await(await req(`/conversations/${id}`,a.accessToken)).json(); assert.equal(d.messages.length,2); assert.notEqual(d.messages[1].id,originalAnswer);
  });
  await t.test('continue previous conversation with recent context', async () => {
    await collect(await req(`/conversations/${id}/messages`,a.accessToken,'POST',{content:'Can you continue that example?'}));
    const d=await(await req(`/conversations/${id}`,a.accessToken)).json(); assert.equal(d.messages.length,4); assert.match(d.messages.at(-1).content,/\*\*1 earlier user message/);
  });
  await t.test('rename and list persisted conversation', async () => {
    assert.equal((await req(`/conversations/${id}`,a.accessToken,'PATCH',{title:'Saved Python conversation'})).status,200);
    const d=await(await req('/conversations',a.accessToken)).json(); assert.equal(d.items.find(c=>c.id===id).title,'Saved Python conversation');
  });
  await t.test('stop saves partial response and concurrent mutations are rejected', async () => {
    let stopped=false;
    const events=await collect(await req(`/conversations/${id}/messages`,a.accessToken,'POST',{content:'Demonstrate stopping a response.'}),async event=>{
      if(event.type==='delta'&&!stopped){stopped=true;
        assert.equal((await req(`/conversations/${id}/messages`,a.accessToken,'POST',{content:'second tab'})).status,409);
        assert.equal((await req(`/conversations/${id}`,a.accessToken,'DELETE')).status,409);
        assert.equal((await req(`/conversations/${id}/stop`,a.accessToken,'POST',{})).status,200);
      }
    });
    assert.equal(events.at(-1).data.message.status,'cancelled');
    const d=await(await req(`/conversations/${id}`,a.accessToken)).json(); assert.equal(d.messages.at(-1).status,'cancelled'); assert.ok(d.messages.at(-1).content.length>0);
  });
  await t.test('login again preserves history', async () => {
    const r=await req('/auth/login',null,'POST',a.credentials);assert.equal(r.status,200);const token=(await r.json()).accessToken;
    const d=await(await req('/conversations',token)).json();assert.ok(d.items.some(c=>c.id===id));
  });
  await t.test('client disconnect cancels generation and saves its status', async () => {
    const controller = new AbortController();
    const response = await fetch(`${base}/api/conversations/${id}/messages`, { method:'POST', headers:{'Content-Type':'application/json',Authorization:`Bearer ${a.accessToken}`}, body:JSON.stringify({content:'Disconnect during generation'}), signal:controller.signal });
    const reader=response.body.getReader(); const decoder=new TextDecoder(); let received='';
    while(!received.includes('event: delta')) { const {value,done}=await reader.read(); if(done)break; received+=decoder.decode(value); }
    controller.abort();
    let status='generating';
    for(let i=0;i<30&&status==='generating';i++){await new Promise(resolve=>setTimeout(resolve,100));const detail=await(await req(`/conversations/${id}`,a.accessToken)).json();status=detail.messages.at(-1).status;}
    assert.equal(status,'cancelled');
  });
  await t.test('delete own conversation removes messages from accessible history', async () => {
    assert.equal((await req(`/conversations/${id}`,a.accessToken,'DELETE')).status,204);
    assert.equal((await req(`/conversations/${id}`,a.accessToken)).status,404);
    assert.ok(!(await(await req('/conversations',a.accessToken)).json()).items.some(c=>c.id===id));
  });
});

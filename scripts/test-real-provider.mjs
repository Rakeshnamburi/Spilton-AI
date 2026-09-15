// Explicit opt-in only: sends one short request to the configured real provider.
import assert from 'node:assert/strict';
import { randomBytes, randomUUID } from 'node:crypto';
assert.equal(process.env.TEST_REAL_PROVIDER, '1', 'Set TEST_REAL_PROVIDER=1 after verifying the provider account is Free.');
const base = 'http://localhost:5081/api';
let token, id;
async function request(path, method='GET', body) {
  return fetch(base+path,{method,headers:{'Content-Type':'application/json',...(token?{Authorization:`Bearer ${token}`}:{})},body:body===undefined?undefined:JSON.stringify(body),signal:AbortSignal.timeout(120000)});
}
try {
  const registered=await request('/auth/register','POST',{name:'Real provider verification',email:`real-${randomUUID()}@example.test`,password:randomBytes(24).toString('base64url')});
  assert.equal(registered.status,201); token=(await registered.json()).accessToken;
  const models=await (await request('/models')).json();
  assert.ok(models.models.some(m=>m.id==='compatible'&&!m.isDevelopment));
  const created=await request('/conversations','POST',{});assert.equal(created.status,201);id=(await created.json()).id;
  const response=await request(`/conversations/${id}/messages`,'POST',{content:'What is 25% of 480? Give the result and one short sentence explaining the calculation.',model:'compatible',mode:'quick'});
  assert.equal(response.status,200);
  const events=[];let buffer='',chunks=0;const decoder=new TextDecoder();const began=Date.now();
  for await(const bytes of response.body){chunks++;buffer+=decoder.decode(bytes,{stream:true});let split;
    while((split=buffer.indexOf('\n\n'))>=0){const frame=buffer.slice(0,split);buffer=buffer.slice(split+2);const type=frame.match(/^event: (.+)$/m)?.[1];const data=frame.match(/^data: (.+)$/m)?.[1];if(type&&data)events.push({type,data:JSON.parse(data),ms:Date.now()-began});}
  }
  const errors=events.filter(e=>e.type==='error');
  if(errors.length){console.log(JSON.stringify({status:'REAL_PROVIDER_FAILED',errors:errors.map(e=>e.data)}));process.exitCode=1;}
  else {
    const deltas=events.filter(e=>e.type==='delta');const answer=deltas.map(e=>e.data.text).join('');
    const done=events.find(e=>e.type==='done');assert.equal(done?.data.saved,true);assert.equal(done?.data.message.status,'completed');assert.match(answer,/120/);assert.ok(deltas.length>1);
    const saved=await(await request(`/conversations/${id}`)).json();assert.equal(saved.messages.at(-1).content,answer);
    assert.equal(saved.messages.at(-1).modelName,'openai/gpt-oss-20b');
    console.log(JSON.stringify({status:'REAL_PROVIDER_TESTED',model:saved.messages.at(-1).modelName,answer,deltaEvents:deltas.length,httpChunks:chunks,firstDeltaMs:deltas[0].ms,lastDeltaMs:deltas.at(-1).ms,persisted:true}));
  }
} finally {if(id)await request(`/conversations/${id}`,'DELETE');}

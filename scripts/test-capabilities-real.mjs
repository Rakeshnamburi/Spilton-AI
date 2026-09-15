import assert from 'node:assert/strict';
import {randomUUID,randomBytes} from 'node:crypto';
import {writeFileSync,readFileSync,existsSync} from 'node:fs';
if(process.env.TEST_REAL_CAPABILITIES!=='1')throw Error('Opt in with TEST_REAL_CAPABILITIES=1; uses the existing configured model.');
const base='http://localhost:5081/api';let token;const results=process.env.RESUME_CAPABILITIES==='1'&&existsSync('.local/day6-capabilities-real.json')?JSON.parse(readFileSync('.local/day6-capabilities-real.json','utf8')):[];
async function call(path,method='GET',body){const r=await fetch(base+path,{method,headers:{...(token?{Authorization:'Bearer '+token}:{}),...(body instanceof FormData?{}:{'Content-Type':'application/json'})},body:body===undefined?undefined:body instanceof FormData?body:JSON.stringify(body),signal:AbortSignal.timeout(125000)});assert.ok(r.ok,`${path}: ${r.status}`);return r;}
token=(await(await call('/auth/register','POST',{name:'Capability test',email:`cap-${randomUUID()}@example.test`,password:randomBytes(24).toString('base64url')})).json()).accessToken;
const space=await(await call('/spaces','POST',{name:'Capability isolation test',type:'EXAM'})).json();
const cat=await(await call('/preparation/catalog')).json();const exam=cat.exams.find(e=>e.name==='SSC CGL');const stage=cat.stages.find(s=>s.examId===exam.id&&s.name==='Tier 1');
await call('/preparation/profile','POST',{spaceId:space.id,examStageId:stage.id,dailyMinutes:60,level:'BEGINNER',preferredLanguage:'English'});
async function conversation(){return(await(await call('/conversations','POST',{spaceId:space.id})).json()).id;}
async function answer(name,prompt,capability,pattern,id,documentIds=[]){
 if(results.some(r=>r.name===name&&r.passed)){console.log('Previously passed '+name);return;}
 const response=await call(`/conversations/${id??await conversation()}/messages`,'POST',{content:prompt,model:'auto',documentIds});
 const raw=await response.text();const frames=raw.split('\n\n').map(f=>({type:f.match(/^event: (.+)$/m)?.[1],data:f.match(/^data: (.+)$/m)?.[1]})).filter(f=>f.data).map(f=>({...f,data:JSON.parse(f.data)}));
 assert.equal(frames.find(f=>f.type==='start')?.data.capability,capability);
 assert.ok(!frames.some(f=>f.type==='error'),JSON.stringify(frames.find(f=>f.type==='error')?.data));
 const message=frames.find(f=>f.type==='done')?.data.message;assert.equal(message.status,'completed');assert.notEqual(message.modelProvider,'Development');assert.match(message.content,pattern);
 if(capability!=='EXAM')assert.doesNotMatch(message.content,/SSC|CGL|target exam/i);
 assert.ok(frames.filter(f=>f.type==='delta').length>1);
 if(documentIds.length){assert.ok(message.citations.length);assert.match(message.content,/\[1\]/);}
 results.push({name,capability,passed:true,answer:message.content});console.log('PASS REAL '+name);writeFileSync('.local/day6-capabilities-real.json',JSON.stringify(results,null,2));
}
await answer('general','Explain cloud computing simply in 80 words.','TUTOR',/cloud|server/i);
await answer('python','Write a complete Python palindrome program, with one example and how to run it.','CODING',/```python/);
const coding=await conversation();await answer('full frontend','Build a responsive login page using HTML and CSS. Give full code for index.html and style.css, plus run instructions. No backend needed.','CODING',/index\.html[\s\S]*style\.css|style\.css[\s\S]*index\.html/,coding);
await answer('follow-up','Now convert the previous HTML page to React. Provide complete component and preserve its styling.','CODING',/React|jsx/,coding);
await answer('react','Create a React counter component. Keep explanation short.','CODING',/useState/);
await answer('java','Explain Java inheritance with a small complete code example.','CODING',/extends/);
await answer('csharp','Create a simple ASP.NET Core API example with complete Program.cs and run commands.','CODING',/MapGet|Controller/);
await answer('debug','Debug this Python code: for i in range(3)\n    print(i)\nShow corrected code and how to test it. Keep it short.','CODING',/colon|:/);
await answer('exam','Teach SSC CGL percentages from basics in 100 words.','EXAM',/100|hundred/);
const form=new FormData();form.append('file',new Blob(['Java handbook\n\nJava inheritance uses the extends keyword. A subclass inherits accessible members from its parent. Java classes extend one class; interfaces allow multiple implementations.'],{type:'text/plain'}),'java-handbook.txt');form.append('spaceId',space.id);
const doc=await(await call('/documents','POST',form)).json();let ready=false;
for(let i=0;i<120;i++){const d=await(await call('/documents/'+doc.id)).json();if(d.status==='READY'){ready=true;break;}assert.notEqual(d.status,'FAILED');await new Promise(r=>setTimeout(r,500));}assert.ok(ready);
await answer('document','According to this handbook, what keyword does Java inheritance use?','DOCUMENT_RAG',/extends/,undefined,[doc.id]);
const unavailable=await fetch(base+`/conversations/${await conversation()}/messages`,{method:'POST',headers:{Authorization:'Bearer '+token,'Content-Type':'application/json'},body:JSON.stringify({content:'Latest .NET changes',model:'auto'})});assert.equal(unavailable.status,400);console.log('PASS current-question honesty: live research unavailable');
console.log('10 real responses tested. Test Space and evidence retained for inspection; no existing data deleted.');

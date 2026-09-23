import {test,expect} from '@playwright/test';
import {randomUUID,randomBytes} from 'node:crypto';
import path from 'node:path';
test('documents: upload, ready, select, grounded stream, source, reload, regenerate and delete',async({page})=>{
  test.setTimeout(120000);
  const real=process.env.TEST_REAL_RAG==='1';
  await page.goto('/register');await page.getByLabel('Full name').fill('Document Explorer');await page.getByLabel('Email').fill(`document-ui-${randomUUID()}@example.test`);await page.getByLabel('Password',{exact:true}).fill(randomBytes(24).toString('base64url'));await page.getByRole('button',{name:'Create account',exact:true}).click();await expect(page).toHaveURL(/verify-email/); await page.getByRole('link',{name:'Continue and verify later'}).click(); await expect(page).toHaveURL(/\/chat$/);
  await page.getByRole('combobox',{name:'Model',exact:true}).selectOption(real?'compatible':'development');
  await page.getByLabel('Chat attachment',{exact:true}).setInputFiles(path.resolve('../.local/day3-fixtures/scholarship.pdf'));
  const dialog=page.getByRole('dialog',{name:'Your documents'});await expect(dialog).toBeVisible();await expect(dialog.getByText('Ready',{exact:true})).toBeVisible({timeout:45000});await expect(page.getByRole('checkbox',{name:'Select scholarship.pdf'})).toBeChecked();
  await page.screenshot({path:'../.local/day3-documents-desktop.png',fullPage:true,animations:'disabled'});
  await page.getByRole('button',{name:'Use selected documents (1)'}).click();
  await expect(page.getByText('Document-grounded chat · 1 selected.',{exact:false})).toBeVisible();
  await page.getByRole('textbox',{name:'Message',exact:true}).fill('What age range is eligible for this scholarship?');await page.getByRole('button',{name:'Send message',exact:true}).click();
  const assistant=page.locator('[data-message-role="ASSISTANT"]').last();await expect(assistant).toHaveAttribute('data-status','completed',{timeout:60000});
  if(real){await expect(assistant).toContainText('21');await expect(assistant).toContainText('30');await expect(assistant).not.toContainText('Development provider');}
  await assistant.getByRole('button',{name:/\[1\] scholarship.pdf — Page 1/}).click();await expect(page.getByRole('dialog',{name:'Source excerpt'})).toContainText('21 to 30');await page.getByRole('button',{name:'Close source'}).click();
  await page.reload();await expect(page.locator('[data-message-role="ASSISTANT"]')).toHaveCount(1);await expect(page.getByRole('button',{name:'Remove selected scholarship.pdf'})).toBeVisible();
  await page.getByRole('combobox',{name:'Model',exact:true}).selectOption(real?'compatible':'development');
  await page.getByRole('button',{name:'Regenerate response'}).click();await expect(page.getByRole('button',{name:'Stop generation'})).toBeVisible();await expect(page.getByRole('button',{name:'Stop generation'})).not.toBeVisible({timeout:60000});await expect(page.locator('.sp-citations')).toContainText('scholarship.pdf');
  await page.screenshot({path:'../.local/day3-rag-desktop.png',fullPage:true,animations:'disabled'});
  await page.setViewportSize({width:390,height:844});expect(await page.evaluate(()=>document.documentElement.scrollWidth<=innerWidth)).toBe(true);await page.screenshot({path:'../.local/day3-rag-mobile.png',fullPage:true,animations:'disabled'});
  await page.getByRole('button',{name:'Open sidebar',exact:true}).click();await page.getByRole('button',{name:'Documents',exact:true}).click();
  await page.getByRole('button',{name:'Delete document scholarship.pdf'}).click();await page.getByRole('button',{name:'Cancel deletion'}).click();await expect(dialog.getByText('scholarship.pdf',{exact:true})).toBeVisible();await page.getByRole('button',{name:'Delete document scholarship.pdf'}).click();await page.getByRole('button',{name:'Confirm delete document'}).click();await expect(dialog.getByText('No documents yet.',{exact:false})).toBeVisible();await page.getByRole('button',{name:'Close documents'}).click();
  await page.locator('.sp-citations').getByRole('button').first().click();await expect(page.getByRole('dialog',{name:'Source excerpt'})).toContainText('no longer available');await page.getByRole('button',{name:'Close source'}).click();
  // Clean up only this test's conversation; uploaded file has already been deleted.
  const id=new URL(page.url()).searchParams.get('c');await page.evaluate(async id=>{await fetch(`/api/chat/conversations/${id}`,{method:'DELETE'});},id);
});
test('document proxy rejects unauthenticated upload',async({request})=>{const response=await request.post('/api/chat/documents',{multipart:{file:{name:'a.txt',mimeType:'text/plain',buffer:Buffer.from('test')}}});expect(response.status()).toBe(401);});

import { test, expect, type Page } from '@playwright/test';
import { randomBytes, randomUUID } from 'node:crypto';
async function register(page: Page) {
  const credentials = { email: `chat-ui-${randomUUID()}@example.test`, password: randomBytes(24).toString('base64url') };
  await page.goto('/register'); await page.getByLabel('Full name').fill('Spilton Explorer'); await page.getByLabel('Email').fill(credentials.email); await page.getByLabel('Password', { exact: true }).fill(credentials.password);
  await page.getByRole('button', { name: 'Create account', exact: true }).click(); await expect(page).toHaveURL(/verify-email/); await page.getByRole('link',{name:'Continue and verify later'}).click(); await expect(page).toHaveURL(/\/chat$/);
  await expect(page.getByRole('textbox', { name: 'Message', exact: true })).toBeEnabled(); await page.getByRole('combobox',{name:'Model',exact:true}).selectOption('development'); return credentials;
}
async function send(page: Page, content: string) {
  await page.getByRole('combobox',{name:'Model',exact:true}).selectOption('development');
  await page.getByRole('textbox', { name: 'Message', exact: true }).fill(content); await page.getByRole('button', { name: 'Send message', exact: true }).click();
  await expect(page.locator('[data-message-role="ASSISTANT"]').last()).toHaveAttribute('data-status','completed');
  await expect(page.getByRole('button', { name: 'Send message', exact: true })).toBeVisible();
}
test('development chat: stream, persist, reopen, continue, rename, copy, regenerate, relogin and delete', async ({ page, context }) => {
  test.setTimeout(90000);
  const errors: string[] = []; page.on('pageerror', e => errors.push(e.message));
  const credentials = await register(page);
  await expect(page.getByRole('heading', { name: 'What do you want to accomplish?' })).toBeVisible();
  await expect(page.getByRole('option', { name: 'Research · web + documents' })).toBeEnabled();
  await expect(page.getByRole('option', { name: 'Agent · safe tools' })).toBeEnabled();
  await expect(page.getByRole('option', { name: 'Think · careful answers' })).toBeEnabled();
  await page.getByRole('combobox', { name: 'Model', exact: true }).selectOption('development');
  await page.screenshot({ path: '../.local/day2-welcome-desktop.png', fullPage: true });
  await page.getByRole('textbox', { name: 'Message', exact: true }).fill('Explain Python lists with examples');
  await page.getByRole('button', { name: 'Send message', exact: true }).click();
  const assistant = page.locator('[data-message-role="ASSISTANT"]').last();
  await expect(assistant).toHaveAttribute('data-status','generating');
  await expect(assistant.locator('.sp-markdown')).toContainText('Development provider');
  await expect(assistant).toHaveAttribute('data-status','completed');
  await expect(assistant.locator('table')).toBeVisible(); await expect(assistant.locator('.sp-code')).toBeVisible();
  await expect(assistant.locator('.sp-code-toolbar')).toContainText('python');
  const conversationUrl = page.url();
  await page.reload(); await expect(page.locator('[data-message-role="USER"]')).toHaveCount(1); await expect(page.locator('[data-message-role="ASSISTANT"]')).toHaveCount(1);
  await expect(page.locator('[data-message-role="ASSISTANT"]')).toContainText('not a real AI');
  await page.getByRole('button', { name: 'New Chat', exact: true }).click();
  await expect(page.getByRole('heading', { name: 'What do you want to accomplish?' })).toBeVisible();
  await page.locator('.sp-history-open').filter({ hasText: 'Explain Python lists with examples' }).click();
  await expect(page).toHaveURL(conversationUrl); await expect(page.locator('[data-message-role="USER"]')).toHaveCount(1);
  await send(page, 'Continue the earlier explanation'); await expect(page.locator('[data-message-role="ASSISTANT"]').last()).toContainText('1 earlier user message');
  await context.grantPermissions(['clipboard-read','clipboard-write']);
  await page.getByRole('button', { name: 'Copy response', exact: true }).last().click();
  expect(await page.evaluate(() => navigator.clipboard.readText())).toContain('Development provider');
  await page.getByRole('button', { name: 'Copy code', exact: true }).last().click();
  expect(await page.evaluate(() => navigator.clipboard.readText())).toContain('print("Hello from Spilton")');
  await page.getByRole('button', { name: 'Regenerate response', exact: true }).click();
  await expect(page.getByRole('button', { name: 'Stop generation' })).toBeVisible();
  await expect(page.getByRole('button', { name: 'Stop generation' })).not.toBeVisible();
  await expect(page.locator('[data-message-role="USER"]')).toHaveCount(2);
  await expect(page.locator('[data-message-role="ASSISTANT"]')).toHaveCount(2);
  await page.getByRole('button', { name: 'Rename Explain Python lists with examples', exact: true }).click();
  await page.getByLabel('Conversation title').fill('My saved Python chat'); await page.getByRole('button', { name: 'Save title', exact: true }).click();
  await expect(page.getByRole('dialog')).not.toBeVisible(); await expect(page.locator('.sp-history-open')).toContainText('My saved Python chat');
  await page.screenshot({ path: '../.local/day2-chat-desktop.png', fullPage: true });
  await page.setViewportSize({width:768,height:1024}); expect(await page.evaluate(()=>document.documentElement.scrollWidth<=window.innerWidth)).toBe(true);
  await page.screenshot({path:'../.local/day2-chat-tablet.png',fullPage:true,animations:'disabled'});
  await page.setViewportSize({width:390,height:844}); expect(await page.evaluate(()=>document.documentElement.scrollWidth<=window.innerWidth)).toBe(true);
  await page.screenshot({path:'../.local/day2-chat-mobile.png',fullPage:true,animations:'disabled'});
  await page.getByRole('button',{name:'Open sidebar',exact:true}).click();
  await page.getByRole('button',{name:'Log out',exact:true}).click(); await expect(page).toHaveURL(/\/login$/);
  await page.getByLabel('Email').fill(credentials.email); await page.getByLabel('Password',{exact:true}).fill(credentials.password); await page.getByRole('button',{name:'Sign in',exact:true}).click();
  await expect(page).toHaveURL(/\/chat$/); await page.getByRole('button',{name:'Open sidebar',exact:true}).click();
  await page.locator('.sp-history-open').filter({hasText:'My saved Python chat'}).click(); await expect(page.locator('[data-message-role="USER"]')).toHaveCount(2);
  await page.getByRole('button',{name:'Open sidebar',exact:true}).click();
  await page.getByRole('button',{name:'Delete My saved Python chat',exact:true}).click(); await page.getByRole('button',{name:'Cancel',exact:true}).click();
  await expect(page.locator('.sp-history-open')).toContainText('My saved Python chat');
  await page.getByRole('button',{name:'Delete My saved Python chat',exact:true}).click(); await page.getByRole('button',{name:'Delete conversation',exact:true}).click();
  await expect(page.getByRole('dialog')).not.toBeVisible(); await expect(page.locator('.sp-history-open')).toHaveCount(0);
  await page.reload(); await expect(page.locator('.sp-history-open')).toHaveCount(0); expect(errors).toEqual([]);
});
test('stop generation persists partial answer and allows regeneration', async ({page})=>{
  await register(page); await page.getByLabel('Message',{exact:true}).fill('Show a streaming demonstration that I can stop'); await page.getByRole('button',{name:'Send message',exact:true}).click();
  await expect(page.locator('[data-message-role="ASSISTANT"] .sp-markdown')).toContainText('Development');
  await page.getByRole('button',{name:'Stop generation'}).click();
  await expect(page.locator('[data-message-role="ASSISTANT"]')).toHaveAttribute('data-status','cancelled');
  await page.reload(); await expect(page.locator('[data-message-role="ASSISTANT"]')).toHaveAttribute('data-status','cancelled');
  await page.getByRole('combobox',{name:'Model',exact:true}).selectOption('development');
  await page.getByRole('button',{name:'Regenerate response',exact:true}).click();
  await expect(page.locator('[data-message-role="ASSISTANT"]').last()).toHaveAttribute('data-status','completed');
});
test('unsafe Markdown does not execute HTML or javascript links', async ({page})=>{
  await register(page); await send(page,'<img src=x onerror="window.spiltonXss=1">\n\n[bad](javascript:alert(1))\n\n**Bold** and *italic*, `inline`, https://example.com');
  expect(await page.evaluate(()=>('spiltonXss' in window))).toBe(false);
  await expect(page.locator('.sp-markdown img')).toHaveCount(0); await expect(page.locator('.sp-markdown a[href^="javascript:"]')).toHaveCount(0);
  await expect(page.locator('.sp-markdown strong').filter({hasText:'Bold'})).toBeVisible();
  await expect(page.locator('.sp-markdown em').filter({hasText:'italic'})).toBeVisible();
});
test('chat proxy rejects cross-origin writes and unauthenticated requests',async({page,request})=>{
  expect((await request.get('/api/chat/conversations')).status()).toBe(401);
  await register(page);
  const result=await page.request.post('/api/chat/conversations',{headers:{Origin:'https://untrusted.example'},data:{}});expect(result.status()).toBe(403);
});
test('network interruption is visible and does not pretend generation completed',async({page})=>{
  await register(page);
  await page.route('**/api/chat/conversations/*/messages',route=>route.abort('connectionfailed'));
  await page.getByLabel('Message',{exact:true}).fill('Network failure test'); await page.getByRole('button',{name:'Send message',exact:true}).click();
  await expect(page.locator('.sp-error')).toBeVisible(); await expect(page.getByRole('button',{name:'Stop generation'})).not.toBeVisible();
  await expect(page.getByLabel('Message',{exact:true})).toHaveValue('Network failure test');
});

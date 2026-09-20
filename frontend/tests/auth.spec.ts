import { test, expect } from '@playwright/test';
import { randomBytes, randomUUID } from 'node:crypto';
const email = `browser-${randomUUID()}@example.test`;
const password = randomBytes(24).toString('base64url');
test('unauthenticated protected page redirects to login', async ({ page }) => {
  await page.goto('/chat'); await expect(page).toHaveURL(/\/login$/); await expect(page.getByRole('heading', { name: 'Welcome back' })).toBeVisible();
});
test('real frontend registration, duplicate, invalid login, login, refresh, and logout', async ({ page, context }) => {
  const errors: string[] = []; page.on('pageerror', e => errors.push(e.message));
  await page.goto('/register'); await page.getByLabel('Full name').fill('Day One'); await page.getByLabel('Email').fill(email); await page.getByLabel('Password', { exact: true }).fill(password);
  await page.getByRole('button', { name: 'Create account', exact: true }).click();
  await expect(page).toHaveURL(/\/chat$/); await expect(page.getByText('Welcome to Spilton, Day.', { exact: true })).toBeVisible();
  const cookie = (await context.cookies()).find(c => c.name === 'spilton_session'); expect(cookie?.httpOnly).toBe(true); expect(cookie?.sameSite).toBe('Lax');
  expect(await page.evaluate(() => localStorage.length)).toBe(0); expect(await page.evaluate(() => document.cookie)).not.toContain('spilton_session');
  await page.reload(); await expect(page.getByText('Welcome to Spilton, Day.', { exact: true })).toBeVisible();
  await expect(page.getByRole('textbox', { name: 'Message', exact: true })).toBeEnabled();
  await page.getByRole('button', { name: 'New chat' }).click(); await expect(page.getByRole('status')).toContainText('fresh workspace');
  await page.screenshot({ path: '../.local/workspace-desktop.png', fullPage: true });
  await page.setViewportSize({ width: 390, height: 844 });
  expect(await page.evaluate(() => document.documentElement.scrollWidth <= window.innerWidth)).toBe(true);
  await page.screenshot({ path: '../.local/workspace-mobile.png', fullPage: true });
  await page.getByRole('button', { name: 'Open sidebar', exact: true }).click();
  await page.getByRole('button', { name: 'Log out' }).click(); await expect(page).toHaveURL(/\/login$/);
  expect((await context.cookies()).find(c => c.name === 'spilton_session')).toBeUndefined();
  await page.goto('/chat'); await expect(page).toHaveURL(/\/login$/);
  await page.goto('/register'); await page.getByLabel('Full name').fill('Duplicate'); await page.getByLabel('Email').fill(email); await page.getByLabel('Password', { exact: true }).fill(password);
  await page.getByRole('button', { name: 'Create account', exact: true }).click(); await expect(page.locator('.error-message')).toContainText('already exists');
  await page.goto('/login'); await page.getByLabel('Email').fill(email); await page.getByLabel('Password', { exact: true }).fill('wrong-password-123');
  await page.getByRole('button', { name: 'Sign in', exact: true }).click(); await expect(page.locator('.error-message')).toContainText('Invalid email or password');
  await page.getByLabel('Password', { exact: true }).fill(password); await page.getByRole('button', { name: 'Sign in', exact: true }).click();
  await expect(page).toHaveURL(/\/chat$/); await expect(page.getByText('Welcome to Spilton, Day.', { exact: true })).toBeVisible();
  await page.getByRole('button', { name: 'Open sidebar', exact: true }).click();
  await page.getByRole('button', { name: 'Log out' }).click(); await expect(page).toHaveURL(/\/login$/); expect(errors).toEqual([]);
});
test('login loading state disables duplicate submission (controlled delay)', async ({ page }) => {
  let release!: () => void;
  const gate = new Promise<void>(resolve => { release = resolve; });
  await page.route('**/api/auth/login', async route => { await gate; await route.fulfill({ status: 401, json: { title: 'Invalid email or password.' } }); });
  await page.goto('/login'); await page.getByLabel('Email').fill('loading@example.test'); await page.getByLabel('Password', { exact: true }).fill('wrong-password-123');
  await page.getByRole('button', { name: 'Sign in', exact: true }).click(); await expect(page.getByRole('button', { name: 'Signing in…' })).toBeDisabled();
  release(); await expect(page.locator('.error-message')).toBeVisible(); await expect(page.getByRole('button', { name: 'Sign in', exact: true })).toBeEnabled();
});
test('same-origin protection rejects cross-origin cookie mutation', async ({ request }) => {
  const response = await request.post('/api/auth/logout', { headers: { Origin: 'https://untrusted.example' }, data: {} }); expect(response.status()).toBe(403);
});
test('password recovery guides the user through email, OTP, and new password', async ({ page }) => {
  await page.route('**/api/auth/forgot-password', route => route.fulfill({ status: 202, json: { message: 'sent' } }));
  await page.route('**/api/auth/verify-reset', route => route.fulfill({ status: 200, json: { resetToken: 'A'.repeat(96) } }));
  await page.route('**/api/auth/reset-password', route => route.fulfill({ status: 204, body: '' }));
  await page.goto('/login');await page.getByRole('link',{name:'Forgot password?'}).click();await expect(page).toHaveURL(/forgot-password/);
  await page.getByLabel('Email').fill('member@example.test');await page.getByRole('button',{name:'Send reset code'}).click();
  await page.getByLabel('Six-digit code').fill('123456');await page.getByRole('button',{name:'Verify code'}).click();
  await page.getByLabel('New password',{exact:true}).fill('new-password');await page.getByLabel('Confirm new password').fill('new-password');await page.getByRole('button',{name:'Save new password'}).click();
  await expect(page.getByText('Password updated. Sign in with your new password.')).toBeVisible();await expect(page.getByRole('button',{name:'Go to sign in'})).toBeVisible();
});

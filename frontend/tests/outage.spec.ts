import { test, expect } from '@playwright/test';
test('real backend outage displays a retryable login error @outage', async ({ page, request }) => {
  test.skip(process.env.TEST_OUTAGE !== '1', 'Run explicitly with backend stopped.');
  const probe = await request.get('http://localhost:5081/api/health').catch(() => null);
  expect(probe).toBeNull();
  await page.goto('/login'); await page.getByLabel('Email').fill('outage@example.test'); await page.getByLabel('Password', { exact: true }).fill('outage-test-password');
  await page.getByRole('button', { name: 'Sign in', exact: true }).click();
  await expect(page.locator('.error-message')).toContainText('temporarily unavailable'); await expect(page.getByRole('button', { name: 'Sign in', exact: true })).toBeEnabled();
});

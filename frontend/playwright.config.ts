import { defineConfig } from '@playwright/test';
export default defineConfig({
  testDir: './tests', fullyParallel: false, workers: 1, retries: 0, timeout: 45000,
  expect: { timeout: 15000 },
  use: { baseURL: process.env.TEST_WEB_URL || 'http://localhost:3000', channel: 'chrome', headless: true, viewport: { width: 1366, height: 900 }, trace: 'off' },
  reporter: [['list']],
});

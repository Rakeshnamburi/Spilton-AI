import {test,expect} from '@playwright/test';
import {randomBytes,randomUUID} from 'node:crypto';
test('bounded agent calculates, streams progress, persists and uses configured web research',async({page})=>{
 test.setTimeout(90000);const email=`agent-ui-${randomUUID()}@example.test`,password=randomBytes(24).toString('base64url');
 await page.goto('/register');await page.getByLabel('Full name').fill('Agent Explorer');await page.getByLabel('Email').fill(email);await page.getByLabel('Password',{exact:true}).fill(password);await page.getByRole('button',{name:'Create account',exact:true}).click();await expect(page).toHaveURL(/\/chat$/);
 await page.getByRole('combobox',{name:'AI mode'}).selectOption('agent');await page.getByRole('textbox',{name:'Message'}).fill('Calculate 18% of 45000');await page.getByRole('button',{name:'Send message'}).click();
 const answer=page.locator('[data-message-role="ASSISTANT"]').last();await expect(answer).toHaveAttribute('data-status','completed');await expect(answer).toContainText('8100');await page.reload();await expect(page.locator('[data-message-role="ASSISTANT"]').last()).toContainText('8100');
 await page.getByRole('button',{name:'New Chat',exact:true}).click();await page.getByRole('combobox',{name:'AI mode'}).selectOption('agent');await page.getByRole('textbox',{name:'Message'}).fill('Research the latest stable .NET release');await page.getByRole('button',{name:'Send message'}).click();const researched=page.locator('[data-message-role="ASSISTANT"]').last();await expect(researched).toHaveAttribute('data-status','completed',{timeout:60000});await expect(researched).toContainText(/https:\/\//);
});

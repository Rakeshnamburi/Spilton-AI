import {test,expect,type Page} from '@playwright/test';

async function openGovernment(page:Page){
  await page.goto('/government');
  await expect(page.getByRole('heading',{name:'Government exam resources'})).toBeVisible();
}

test('manual notification upload, cited eligibility, library filters and mobile',async({page})=>{
  test.info().annotations.push({type:'environment-flake',description:'System Chrome can replace localhost with chrome-error://chromewebdata while both servers remain healthy. The test reopens the page and verifies the accepted upload from persisted application state.'});
  test.setTimeout(180000);
  await page.goto('/register');
  await page.getByLabel('Full name').fill('Government Browser');
  await page.getByLabel('Email').fill(`gov-ui-${Date.now()}@example.test`);
  await page.getByLabel('Password',{exact:true}).fill('Test-password-'+Date.now()+'!');
  await page.getByRole('button',{name:'Create account',exact:true}).click();
  await expect(page).toHaveURL(/chat/);
  await openGovernment(page);

  const fileName='notification-ui.txt';
  const input=page.getByLabel('Upload government document');
  await expect(input).toBeEnabled();
  await input.setInputFiles({name:fileName,mimeType:'text/plain',buffer:Buffer.from('FICTIONAL TEST NOTIFICATION\nQualification: bachelor degree.\nAge: 21 to 30 years\nApplication deadline: 15 October 2026.')});
  try{
    await expect(page.getByRole('status')).toContainText('Document Ready',{timeout:90000});
  }catch(error){
    const browserError=await page.getByRole('heading',{name:'This page couldn’t load'}).isVisible().catch(()=>false);
    if(!browserError)throw error;
    await openGovernment(page);
    await expect(page.getByLabel('Ready document')).toContainText(fileName,{timeout:90000});
    await page.getByLabel('Ready document').selectOption({label:fileName});
  }

  await page.getByLabel('Title / headline').fill('Fictional browser notification');
  await page.getByLabel('Organization',{exact:true}).fill('Development fixture');
  await page.getByRole('button',{name:'Register source',exact:true}).click();
  await expect(page.getByText('Fictional browser notification',{exact:true})).toBeVisible();
  await page.getByRole('button',{name:'View details',exact:true}).click();
  await expect(page.getByRole('heading',{name:'Eligibility screening'})).toBeVisible();
  await page.getByRole('combobox',{name:'Education level',exact:true}).selectOption('BACHELOR_OR_HIGHER');
  await page.getByLabel('I reviewed the complete source for other conditions, post and cutoff dates').check();
  await page.getByRole('button',{name:'Check eligibility'}).click();
  await expect(page.getByRole('heading',{name:'INSUFFICIENT INFORMATION'})).toBeVisible();
  await page.getByLabel('Age at stated cutoff (optional)').fill('25');
  await page.getByRole('button',{name:'Check eligibility'}).click();
  await expect(page.getByRole('heading',{name:'LIKELY ELIGIBLE'})).toBeVisible();
  await page.getByRole('button',{name:/\[1\] Fictional browser notification/}).first().click();
  await expect(page.getByRole('dialog',{name:'Source excerpt'})).toContainText('bachelor');
  await page.getByRole('button',{name:'Close source'}).click();

  await page.getByRole('link',{name:'Analyse with AI / RAG'}).click();
  await expect(page.getByLabel('AI mode')).toHaveValue('research');
  await expect(page.getByText(/Document-grounded chat · 1 selected/)).toBeVisible();
  await page.getByRole('button',{name:/Remove selected/}).click();
  await expect(page.getByText('General chat · no documents selected')).toBeVisible();

  await openGovernment(page);
  await page.getByRole('button',{name:'Previous Year Papers',exact:true}).click();
  await expect(page.getByRole('heading',{name:'Previous Year Papers library'})).toBeVisible();
  await page.getByRole('button',{name:'Current Affairs',exact:true}).click();
  await expect(page.getByRole('heading',{name:'Today’s Current Affairs · manual sources'})).toBeVisible();
  await page.setViewportSize({width:390,height:844});
  expect(await page.evaluate(()=>document.documentElement.scrollWidth<=innerWidth)).toBe(true);
  await page.screenshot({path:'../.local/day5-government-mobile.png',fullPage:true});
});

test('government proxy and page enforce authentication',async({request,page})=>{
  expect((await request.get('/api/chat/government/resources')).status()).toBe(401);
  await page.goto('/government');
  await expect(page).toHaveURL(/login/);
});

import { test, expect } from '@playwright/test';

test('User can login with valid credentials', async ({ page }) => {
    await page.goto('https://test-fuse-web.practicemgmt-test.pattersondevops.com/');

    // Wait for login form
    await page.waitForSelector('#userNameInput', { timeout: 10000 });

    // Fill in credentials
    await page.fill('#userNameInput', 'pa26899@fusepatterson.com');
    await page.fill('#passwordInput', 'So@rt3st');

    // Submit the form
    await page.click('#submitButton');

    // Wait for redirect and verify login success
    await expect(page).toHaveURL(/.*#\/$/, { timeout: 10000 });
    await expect(page.locator('#fuseLogo')).toBeVisible({ timeout: 10000 });
});

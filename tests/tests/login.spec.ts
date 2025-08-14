import { test } from '@playwright/test';
import { LoginPage } from '../pages/LoginPage';

test.describe('Login Module', () => {
    let loginPage: LoginPage;

    test.beforeEach(async ({ page }) => {
        loginPage = new LoginPage(page);
        await loginPage.navigate('https://your-app-url.com/login');
    });

    test('User can login with valid credentials', async () => {
        await loginPage.login('validUser', 'validPass');
        // Add assertions here after successful login
    });

    test('Shows error on invalid credentials', async () => {
        await loginPage.login('invalidUser', 'wrongPass');
        await loginPage.expectError('Invalid username or password.');
    });
});

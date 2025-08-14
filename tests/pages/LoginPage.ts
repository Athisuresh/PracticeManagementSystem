import { expect } from '@playwright/test';
import { BasePage } from '../../utils/BasePage';

export class LoginPage extends BasePage {
    private usernameInput = '#userNameInput';
    private passwordInput = '#passwordInput';
    private submitButton = '#submitButton';
    private errorMessage = '.error-message'; // <- update if needed

    async login(username: string, password: string) {
        await this.page.fill(this.usernameInput, username);
        await this.page.fill(this.passwordInput, password);
        await this.page.click(this.submitButton);
    }

    async expectError(message: string) {
        await expect(this.page.locator(this.errorMessage)).toHaveText(message);
    }
}

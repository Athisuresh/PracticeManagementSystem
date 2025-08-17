using Microsoft.Playwright;
using System;
using System.Threading.Tasks;

namespace PracticeManagementSystem.Pages
{
    /// <summary>
    /// Represents the login page and its interactions.
    /// </summary>
    public class LoginPage : BasePage
    {
        // Selector constants
        private const string UsernameSelector = "#userNameInput";
        private const string PasswordSelector = "#passwordInput";
        private const string SubmitButtonSelector = "#submitButton";
        private const string FuseLogoSelector = "#fuseLogo";

        /// <summary>
        /// Username input locator.
        /// </summary>
        private readonly ILocator _usernameInput;
        /// <summary>
        /// Password input locator.
        /// </summary>
        private readonly ILocator _passwordInput;
        /// <summary>
        /// Submit button locator.
        /// </summary>
        private readonly ILocator _submitButton;
        /// <summary>
        /// Fuse logo locator.
        /// </summary>
        private readonly ILocator _fuseLogo;

        public LoginPage(IPage page) : base(page)
        {
            _usernameInput = page.Locator(UsernameSelector);
            _passwordInput = page.Locator(PasswordSelector);
            _submitButton = page.Locator(SubmitButtonSelector);
            _fuseLogo = page.Locator(FuseLogoSelector);
        }

        /// <summary>
        /// Navigates to the specified URL.
        /// </summary>
        public async Task NavigateAsync(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("URL cannot be null or empty.", nameof(url));

            await page.GotoAsync(url);
        }

        /// <summary>
        /// Performs login with the provided credentials.
        /// </summary>
        public async Task LoginAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be null or empty.", nameof(username));
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be null or empty.", nameof(password));

            await WaitAndFillAsync(_usernameInput, username);
            await WaitAndFillAsync(_passwordInput, password);
            await WaitAndClickAsync(_submitButton);
        }

        /// <summary>
        /// Checks if login was successful by verifying the fuse logo is visible.
        /// </summary>
        public Task<bool> IsLoginSuccessfulAsync() => _fuseLogo.IsVisibleAsync();

        /// <summary>
        /// Waits for the locator to be visible and fills it with the provided text.
        /// </summary>
        private async Task WaitAndFillAsync(ILocator locator, string text)
        {
            await locator.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
            await locator.FillAsync(text);
        }

        /// <summary>
        /// Waits for the locator to be visible and clicks it.
        /// </summary>
        private async Task WaitAndClickAsync(ILocator locator)
        {
            await locator.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
            await locator.ClickAsync();
        }
    }
}
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PracticeManagementSystem.Pages;
using PracticeManagementSystem.Utils;
using System.Threading.Tasks;

namespace PracticeManagementSystem.Tests
{
    [TestClass]
    public class LoginTests : TestBase
    {
        [TestMethod]
        public async Task User_Can_Login_With_Valid_Credentials()
        {
            var loginPage = new LoginPage(_page);
            await loginPage.NavigateAsync(ConfigManager.BaseUrl);
            await loginPage.LoginAsync(ConfigManager.Username, ConfigManager.Password);
            await _page.Locator("#dashboardSelectList").WaitForAsync(new() { Timeout = 20000 });

            Assert.IsTrue(await loginPage.IsLoginSuccessfulAsync(), "Login was not successful.");
        }


        [TestMethod]
        public async Task User_Cannot_Login_With_Empty_Credentials()
        {
            var loginPage = new LoginPage(_page);
            await loginPage.NavigateAsync(ConfigManager.BaseUrl);
            await loginPage.LoginAsync("", "");

            var errorMessage = await loginPage.GetErrorMessageAsync();

            // Adjust assertion to match the real validation message
            Assert.IsTrue(
                errorMessage.Contains("Enter your user ID"),
                $"Expected validation message not shown. Actual: '{errorMessage}'"
            );
        }


        [TestMethod]
        public async Task User_Can_Logout_Successfully()
        {
            var loginPage = new LoginPage(_page);
            await loginPage.NavigateAsync(ConfigManager.BaseUrl);
            await loginPage.LoginAsync(ConfigManager.Username, ConfigManager.Password);

            var dashboardPage = new DashboardPage(_page);
            await dashboardPage.LogoutAsync();

            // Assert user is redirected to ADFS login page
            StringAssert.Contains(_page.Url, "adfs/ls", "User was not redirected to ADFS login page after logout.");
        }

    }
}
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
    }
}
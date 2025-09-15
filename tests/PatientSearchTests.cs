using Microsoft.VisualStudio.TestTools.UnitTesting;
using PracticeManagementSystem.Pages;
using PracticeManagementSystem.Utils;
using System.Threading.Tasks;

namespace PracticeManagementSystem.Tests
{
    [TestClass]
    public class PatientSearchTests : TestBase
    {
        [TestMethod]
        public async Task User_CanSearch_AndView_PatientDetails()
        {
            var loginPage = new LoginPage(_page);
            await loginPage.NavigateAsync(ConfigManager.BaseUrl);
            await loginPage.LoginAsync(ConfigManager.Username, ConfigManager.Password);

            var homePage = new HomePage(_page);
            await homePage.WaitForHomePageAsync();

            var patientPage = new PatientPage(_page);
            await patientPage.SearchPatientAsync("tracie burch");

            var (name, dob, gender, email, phone) = await patientPage.GetFirstPatientRowAsync();

            Assert.AreEqual("Tracie Burch", name, "Patient name mismatch.");
            Assert.AreEqual("Sep 28, 1947", dob, "Patient DOB mismatch.");
            Assert.AreEqual("f", gender, "Patient gender mismatch.");
            Assert.AreEqual("zvaughn@example.com", email, "Patient email mismatch.");
            Assert.AreEqual("954.341.6267x88443", phone, "Patient phone mismatch.");
        }


        [TestMethod]
        public async Task User_SearchesFor_NonExistent_PatientShouldSeeNoResults()
        {
            // Step 1: Login
            var loginPage = new LoginPage(_page);
            await loginPage.NavigateAsync(ConfigManager.BaseUrl);
            await loginPage.LoginAsync(ConfigManager.Username, ConfigManager.Password);

            var homePage = new HomePage(_page);
            await homePage.WaitForHomePageAsync();

            // Step 2: Search for a non-existent patient
            var patientPage = new PatientPage(_page);
            await patientPage.SearchPatientAsync("nonexistentuser");

            // Step 3: Validate paginator footer shows "0 of 0"
            var footerText = await patientPage.GetFooterTextAsync();
            Assert.AreEqual("0 of 0", footerText,
                $"Expected '0 of 0' in footer, but got '{footerText}' instead.");
        }


    }
}

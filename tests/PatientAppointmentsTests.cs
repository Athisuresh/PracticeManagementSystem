using Microsoft.VisualStudio.TestTools.UnitTesting;
using PracticeManagementSystem.Pages;
using PracticeManagementSystem.Utils;
using System.Threading.Tasks;

namespace PracticeManagementSystem.Tests
{
    [TestClass]
    public class PatientAppointmentsTests : TestBase
    {
        [TestMethod]
        public async Task User_Can_View_Appointments_For_Patient()
        {
            // Step 1: Login
            var loginPage = new LoginPage(_page);
            await loginPage.NavigateAsync(ConfigManager.BaseUrl);
            await loginPage.LoginAsync(ConfigManager.Username, ConfigManager.Password);

            var homePage = new HomePage(_page);
            await homePage.WaitForHomePageAsync();

            // Step 2: Search patient
            var patientPage = new PatientPage(_page);
            await patientPage.SearchPatientAsync("ricky flores");

            // Step 3: Open appointments
            await patientPage.OpenFirstPatientAppointmentsAsync();

            // Step 4: Verify appointments page
            var appointmentsPage = new AppointmentsPage(_page);
            await appointmentsPage.WaitForPageAsync();

            var details = await appointmentsPage.GetAppointmentDetailsAsync();

            // Assertions
            Assert.IsTrue(details.Patient.Contains("Ricky Flores"), "Patient name is incorrect.");
            Assert.AreEqual("Wednesday, August 20, 2025", details.BookedDate, "Booked date mismatch.");
            Assert.AreEqual("Thursday, August 21, 2025", details.ScheduledDate, "Scheduled date mismatch.");
            Assert.IsTrue(details.Notes.Contains("not healthy"), "Notes are incorrect.");
        }
    }
}

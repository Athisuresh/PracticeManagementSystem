using Microsoft.Playwright;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeManagementSystem.Pages
{
    public class AppointmentsPage
    {
        private readonly IPage _page;

        public AppointmentsPage(IPage page)
        {
            _page = page;
        }

        // Locators
        private ILocator PatientName => _page.Locator("mat-card-title");
        private ILocator AppointmentDates => _page.Locator("mat-card-subtitle");
        private ILocator Notes => _page.Locator("mat-card-content p");
        private ILocator EditButton => _page.GetByRole(AriaRole.Button, new() { Name = "Edit" });
        private ILocator DeleteButton => _page.GetByRole(AriaRole.Button, new() { Name = "Delete" });

        // Wait for page load
        public async Task WaitForPageAsync(int timeoutMs = 10000)
        {
            await PatientName.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = timeoutMs
            });
        }

        // Extract appointment details
        public async Task<(string Patient, string BookedDate, string ScheduledDate, string Notes)> GetAppointmentDetailsAsync()
        {
            var patientRaw = (await PatientName.InnerTextAsync()).Trim();
            var patient = patientRaw.Replace("Patient Name:", "").Trim();

            var datesText = (await AppointmentDates.InnerTextAsync()).Trim();
            var parts = datesText.Split('\n').Select(p => p.Trim()).ToList();

            string booked = "";
            string scheduled = "";

            if (parts.Count > 0 && parts[0].StartsWith("Booked At:", StringComparison.OrdinalIgnoreCase))
                booked = parts[0].Replace("Booked At:", "").Trim();

            if (parts.Count > 1 && parts[1].StartsWith("Scheduled At:", StringComparison.OrdinalIgnoreCase))
                scheduled = parts[1].Replace("Scheduled At:", "").Trim();

            var notesRaw = (await Notes.InnerTextAsync()).Trim();
            var notes = notesRaw.Replace("Notes:", "").Trim();

            return (patient, booked, scheduled, notes);
        }

        // Button actions
        public async Task ClickEditAsync() => await EditButton.ClickAsync();
        public async Task ClickDeleteAsync() => await DeleteButton.ClickAsync();
    }
}

using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeManagementSystem.Pages
{
    public class PatientPage
    {
        private readonly IPage _page;

        public PatientPage(IPage page)
        {
            _page = page;
        }

        // Search box and button
        private ILocator SearchInput => _page.GetByRole(AriaRole.Textbox, new() { Name = "Search Patient" });
        private ILocator SearchButton => _page.GetByRole(AriaRole.Button, new() { Name = "Search" });

        // Table rows
        private ILocator PatientRows => _page.Locator("tbody.mdc-data-table__content tr");

        // Table cells
        private ILocator FullNameCells => _page.Locator("td.mat-column-full_name");
        private ILocator DobCells => _page.Locator("td.mat-column-dob");
        private ILocator GenderCells => _page.Locator("td.mat-column-gender");
        private ILocator EmailCells => _page.Locator("td.mat-column-email");
        private ILocator PhoneCells => _page.Locator("td.mat-column-phone");

        // Action buttons
        private ILocator EditButtons => _page.Locator("td.mat-column-action button:has(mat-icon:has-text('edit'))");
        private ILocator DeleteButtons => _page.Locator("td.mat-column-action button:has(mat-icon:has-text('delete'))");

        // Perform search
        public async Task SearchPatientAsync(string name)
        {
            await SearchInput.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
            await SearchInput.FillAsync(name);
            await SearchButton.ClickAsync();

            // Wait for rows to refresh
            await PatientRows.First.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Attached,
                Timeout = 10000
            });
        }

        // Get all displayed names
        public async Task<List<string>> GetDisplayedPatientNamesAsync()
        {
            var rowCount = await PatientRows.CountAsync();
            if (rowCount == 0) return new List<string>();

            var names = new List<string>();
            for (int i = 0; i < rowCount; i++)
            {
                var row = PatientRows.Nth(i);
                var name = (await row.Locator("td.mat-column-full_name").InnerTextAsync()).Trim();
                names.Add(name);
            }
            return names;
        }

        // Get details of the first row
        public async Task<(string Name, string Dob, string Gender, string Email, string Phone)> GetFirstPatientRowAsync()
        {
            var rowCount = await PatientRows.CountAsync();
            if (rowCount == 0)
                throw new InvalidOperationException("No patient rows are displayed on the page.");

            var row = PatientRows.First;

            var name = (await row.Locator("td.mat-column-full_name").InnerTextAsync()).Trim();
            var dob = (await row.Locator("td.mat-column-dob").InnerTextAsync()).Trim();
            var gender = (await row.Locator("td.mat-column-gender").InnerTextAsync()).Trim();
            var email = (await row.Locator("td.mat-column-email").InnerTextAsync()).Trim();
            var phone = (await row.Locator("td.mat-column-phone").InnerTextAsync()).Trim();

            return (name, dob, gender, email, phone);
        }

        // Check if a patient with specific name exists
        public async Task<bool> IsPatientDisplayedAsync(string name)
        {
            var names = await GetDisplayedPatientNamesAsync();
            return names.Any(n => n.Contains(name, StringComparison.OrdinalIgnoreCase));
        }

        // ✅ Open Appointments for the first patient row
        public async Task OpenFirstPatientAppointmentsAsync(int timeoutMs = 15000)
        {
            // Wait for rows
            await PatientRows.First.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Attached,
                Timeout = timeoutMs
            });

            var firstRow = PatientRows.First;

            // Find the appointment button in the first row
            var appointmentButton = firstRow.Locator("button[mattooltip='View Appointments']");

            if (await appointmentButton.CountAsync() == 0)
                appointmentButton = firstRow.Locator("button:has(mat-icon:has-text('event'))");

            if (await appointmentButton.CountAsync() == 0)
                throw new InvalidOperationException("No appointment button found in the first patient row.");

            // Ensure visible + click
            await appointmentButton.ScrollIntoViewIfNeededAsync();
            await appointmentButton.ClickAsync();

            // ✅ Wait for Appointments page to load
            await _page.Locator("mat-card-title")
                .Or(_page.GetByText("Patient Name:", new() { Exact = false }))
                .First
                .WaitForAsync(new LocatorWaitForOptions
                {
                    State = WaitForSelectorState.Visible,
                    Timeout = timeoutMs
                });
        }
    }
}

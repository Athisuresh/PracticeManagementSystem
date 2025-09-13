using Microsoft.Playwright;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace PracticeManagementSystem.Utils
{
    public class TestBase
    {
        protected static IPlaywright _playwright;
        protected static IBrowser _browser;
        protected IPage _page;
        protected IBrowserContext _context;

        [ClassInitialize(InheritanceBehavior.BeforeEachDerivedClass)]
        public static async Task ClassInit(TestContext context)
        {
            _playwright = await Playwright.CreateAsync();

            // Detect if running inside Azure Pipelines
            bool isCi = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("TF_BUILD"));

            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = isCi ? true : false,
                Args = isCi
                    ? new[] { "--no-sandbox", "--disable-setuid-sandbox" } 
                    : Array.Empty<string>() 
            });
        }

        [TestInitialize]
        public async Task TestInit()
        {
            _context = await _browser.NewContextAsync();
            _page = await _context.NewPageAsync();
        }

        [TestCleanup]
        public async Task TestCleanup()
        {
            await _context.CloseAsync();
        }

        [ClassCleanup(InheritanceBehavior.BeforeEachDerivedClass)]
        public static async Task ClassCleanup()
        {
            await _browser.CloseAsync();
            _playwright.Dispose();
        }
    }
}

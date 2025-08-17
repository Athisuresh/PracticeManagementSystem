using Microsoft.Extensions.Configuration;
using System.IO;

namespace PracticeManagementSystem.Utils
{
    public static class ConfigManager
    {
        private static IConfigurationRoot config;

        static ConfigManager()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("Config/appsettings.json", optional: false, reloadOnChange: true);
            config = builder.Build();
        }

        public static string BaseUrl => config["baseUrl"];
        public static string Username => config["username"];
        public static string Password => config["password"];
    }
}
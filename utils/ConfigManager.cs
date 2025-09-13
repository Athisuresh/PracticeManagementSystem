using Microsoft.Extensions.Configuration;
using System;
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

        // ✅ Default: appsettings.json, but allow pipeline override via BASE_URL env variable
        public static string BaseUrl
        {
            get
            {
                var envUrl = Environment.GetEnvironmentVariable("BASE_URL");
                return !string.IsNullOrEmpty(envUrl) ? envUrl : config["BaseUrl"];
            }
        }

        public static string Username => config["Username"];
        public static string Password => config["Password"];
    }
}

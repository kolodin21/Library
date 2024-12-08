using System.IO;
using Microsoft.Extensions.Configuration;

namespace Library.GUI.Configuration
{
    public class AdminSettings
    {
        public string AdminLogin { get; set; } = string.Empty;
        public string AdminPassword { get; set; } = string.Empty;
    }

    public static class AdminConfig
    {
        private static readonly IConfiguration Configuration;

        static AdminConfig()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(@"Configuration/appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }

        public static string Login => Configuration["AdminSettings:AdminLogin"];

        public static string Password => Configuration["AdminSettings:AdminPassword"];
    }
}

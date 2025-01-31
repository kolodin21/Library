using System.IO;
using Microsoft.Extensions.Configuration;

namespace Library.Client.GUI.Configuration
{
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

        public static string PathNlog => Configuration["AdminSettings:PathNlog"];
        //Todo
        // добавить проверки на null
    }
}

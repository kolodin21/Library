using Microsoft.Extensions.Configuration;

namespace Library.DAL.Configuration
{
    public static class DatabaseConfig 
    {
        private static readonly IConfiguration Configuration;

        static DatabaseConfig()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile(@"Configuration\appsetting.json", optional: false, reloadOnChange: true)
                .Build();
        }

        public static string ConnectionString => Configuration["ConnectionString:DefaultConnection"]
                                                 ?? throw new InvalidOperationException(
                                                     "ConnectionStrings not found in configuration.");

        public static string DatabaseSchema => Configuration["TableSchema"] ??
                                               throw new InvalidOperationException(
                                                   "ConnectionStrings not found in configuration.");
    }
}

using Library.BL.Service;
using Library.GUI.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using Library.Infrastructure;
using Library.Common;

namespace Library.GUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            IServiceCollection services = new ServiceCollection();

            ConfigureServices(services);

            services.AddInfrastructure();

            // Построение провайдера
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            var logger = serviceProvider.GetRequiredService<IMessageLogger>();

            var serviceManager = serviceProvider.GetRequiredService<ServiceManager>();

            ViewModelBase.Initialize(logger, serviceManager);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IMessageLogger, WpfLogger>();
            // Регистрация ViewModel
           
        }

        public class WpfLogger : IMessageLogger
        {
            public void Log(string message)
            {
                MessageBox.Show(message);
            }
        }
    }
}

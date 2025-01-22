using Library.BL.Service;
using Library.GUI.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using Library.Infrastructure;
using Library.Common;
using Library.GUI.View.LogInSystem;
using Library.GUI.ViewModels.LogInSystemVM;

namespace Library.GUI
{
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            IServiceCollection services = new ServiceCollection();

            ConfigureServices(services);

            services.AddInfrastructure();

            // Построение провайдера
            ServiceProvider = services.BuildServiceProvider();

            var logger = ServiceProvider.GetRequiredService<IMessageLogger>();

            var serviceManager = ServiceProvider.GetRequiredService<ServiceManager>();

            ViewModelBase.Initialize(logger, serviceManager);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IMessageLogger, WpfLogger>();

            services.AddViewWithViewModel<MainMenuPageView, MainMenuPageViewModel>();
            services.AddViewWithViewModel<AuthorizationPageView, AuthorizationPageViewModel>();

        }

        public class WpfLogger : IMessageLogger
        {
            public void Log(string message)
            {
                MessageBox.Show(message);
            }
        }
    }

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddViewWithViewModel<TView, TViewModel>(this IServiceCollection services)
            where TView : FrameworkElement, new()
            where TViewModel : class
        {
            services.AddSingleton<TViewModel>();
            services.AddSingleton<TView>(provider =>
            {
                var view = new TView
                {
                    DataContext = provider.GetRequiredService<TViewModel>()
                };
                return view;
            });
            return services;
        }
    }

}

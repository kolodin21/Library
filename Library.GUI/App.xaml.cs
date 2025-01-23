using Library.BL.Service;
using Library.GUI.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using Library.Infrastructure;
using Library.GUI.View.Admin;
using Library.GUI.View.LogInSystem;
using Library.GUI.View.User;
using Library.GUI.ViewModels.AdminVM;
using Library.GUI.ViewModels.LogInSystemVM;
using Library.GUI.ViewModels.UserVM;
using NLog;

namespace Library.GUI
{
    public partial class App : Application
    {
        //Сервис для работы с классами и их получения из DI
        public IServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            IServiceCollection services = new ServiceCollection();

            ConfigureServices(services);

            services.AddInfrastructure();

            // Построение провайдера
            ServiceProvider = services.BuildServiceProvider();

            var serviceManager = ServiceProvider.GetRequiredService<ServiceManager>();

            ViewModelBase.Initialize(serviceManager);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            
            services.AddViewWithViewModel<MainMenuPageView, MainMenuPageViewModel>();
            services.AddViewWithViewModel<AuthorizationPageView, AuthorizationPageViewModel>();
            services.AddViewWithViewModel<RegistrationPageView, RegistrationPageViewModel>();
            services.AddViewWithViewModel<AdminPageView, AdminPageViewModel>();
            services.AddViewWithViewModel<UserPageView, UserPageViewModel>();

        }

        //Todo
        //Добавить асинхронность

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

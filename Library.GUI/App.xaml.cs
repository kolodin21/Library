using System.Windows;
using Library.Client.GUI.Configuration;
using Library.Client.GUI.View.Admin;
using Library.Client.GUI.View.LogInSystem;
using Library.Client.GUI.View.User;
using Library.Client.GUI.ViewModels.AdminVM;
using Library.Client.GUI.ViewModels.LogInSystemVM;
using Library.Client.GUI.ViewModels.UserVM;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace Library.Client.GUI
{
    public partial class App : Application
    {
        //Сервис для работы с классами и их получения из DI
        public IServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            IServiceCollection services = new ServiceCollection();

            //Логгер
            LogManager.LoadConfiguration(AdminConfig.PathNlog);
            
            //Получение View и ViewModel
            ConfigureServices(services);

            // Построение провайдера
            ServiceProvider = services.BuildServiceProvider();

        }

        //Реализация View и ViewModel
        private static void ConfigureServices(IServiceCollection services)
        {
            
            services.AddViewWithViewModel<MainMenuPageView, MainMenuPageViewModel>();
            services.AddViewWithViewModel<AuthorizationPageView, AuthorizationPageViewModel>();
            services.AddViewWithViewModel<RegistrationPageView, RegistrationPageViewModel>();
            services.AddViewWithViewModel<AdminPageView, AdminPageViewModel>();
            services.AddViewWithViewModel<UserPageView, UserPageViewModel>();

        }
    }

    //Метод расширения для IServiceCollection для одновременного создадания View и ViewModel с привязкой DataContext
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

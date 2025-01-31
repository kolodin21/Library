using System.Windows;
using Library.Client.GUI.Configuration;
using Library.Client.GUI.View.Admin;
using Library.Client.GUI.View.LogInSystem;
using Library.Client.GUI.View.User;
using Library.Client.GUI.ViewModels;
using Library.Client.GUI.ViewModels.AdminVM;
using Library.Client.GUI.ViewModels.LogInSystemVM;
using Library.Client.GUI.ViewModels.UserVM;
using Library.Client.Http;
using Library.Models;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace Library.Client.GUI
{
    public partial class App
    {
        //Сервис для работы с классами и их получения из DI
        public IServiceProvider ServiceProvider { get;}
        public ManagerHttp ManagerHttp { get;}

        [Obsolete("Obsolete")]
        public App()
        {
            IServiceCollection services = new ServiceCollection();

            //Логгер
            LogManager.LoadConfiguration(AdminConfig.PathNlog);
            
            //Получение View и ViewModel
            ConfigureServices(services);

            // Построение провайдера
            ServiceProvider = services.BuildServiceProvider();
            ManagerHttp = ServiceProvider.GetRequiredService<ManagerHttp>();

            ViewModelBase.Initialize(ServiceProvider, ManagerHttp);
        }

        //Реализация View и ViewModel
        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<UserHttpClient>();
            services.AddSingleton<ManagerHttp>();

            services.AddViewWithViewModel<MainMenuPageView, MainMenuPageViewModel>();
            services.AddViewWithViewModel<AuthorizationPageView, AuthorizationPageViewModel>();
            services.AddViewWithViewModel<RegistrationPageView, RegistrationPageViewModel>();
            services.AddViewWithViewModel<AdminPageView, AdminPageViewModel>();
            services.AddViewWithViewModel<UserPageView, UserPageViewModel>();

            services.AddTransient<UserPageViewModel>(provider =>
            {
                var user = provider.GetRequiredService<User>(); // Получаем пользователя (позже передадим вручную)
                return new UserPageViewModel(user);
            });
            services.AddTransient<UserPageView>();
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
            services.AddTransient<TView>(provider =>
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

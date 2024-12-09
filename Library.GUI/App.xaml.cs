using Library.BL.Service;
using Library.BL;
using Library.Common;
using Library.DAL.Interface;
using Library.DAL.Models;
using Library.DAL.Repositories;
using Library.GUI.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Condition = Library.DAL.Models.Condition;
using System.Windows;

namespace Library.GUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            var services = new ServiceCollection();

            ConfigureServices(services);

            // Построение провайдера
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var logger = serviceProvider.GetRequiredService<IMessageLogger>();
            var serviceManager = serviceProvider.GetRequiredService<ServiceManager>();

            ViewModelBase.Initialize(logger, serviceManager);

        }

        private static void ConfigureServices(IServiceCollection services)
        {
            // Логгер (глобальный объект)
            services.AddSingleton<IMessageLogger, WpfLogger>();

            // Репозитории (всегда одинаковые)
            services.AddSingleton<IGetRepository, GetRepository>();
            services.AddSingleton<IModificationRepository, ModificationRepository>();
            services.AddSingleton<IRepositoryManager, RepositoryManager>();

            // SQL-провайдеры (глобальные)
            services.AddSingleton<ISqlUserProvider, SqlUserProvider>();
            services.AddSingleton<ISqlBookProvider, SqlBookProvider>();
            services.AddSingleton<ISqlTakeReturnBookProvider, SqlTakeReturnBookProvider>();
            services.AddSingleton<ISqlProvider<Author>, SqlAuthorProvider>();
            services.AddSingleton<ISqlProvider<Publisher>, SqlPublisherProvider>();
            services.AddSingleton<ISqlProvider<Condition>, SqlConditionProvider>();

            // Сервисы 
            services.AddSingleton<UserService>();
            services.AddSingleton<BookService>();
            services.AddSingleton<TakeReturnBookService>();
            services.AddSingleton<AuthorService>();
            services.AddSingleton<ConditionService>();
            services.AddSingleton<PublisherService>();
            services.AddSingleton<ServiceManager>();

            // Регистрация ViewModel
            //services.AddScoped<MainWindowVM>();
            //services.AddScoped<RegistrationWindow>();
            //services.AddScoped<AuthorizationWindow>();
        }
    }
}

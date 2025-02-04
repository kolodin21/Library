using Library.Models;
using Library.Server.BL;
using Library.Server.BL.Service;
using Library.Server.DAL.Interface;
using Library.Server.DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // Репозитории 
            services.AddSingleton<IGetRepository, GetRepository>();
            services.AddSingleton<IModificationRepository, ModificationRepository>();
            services.AddSingleton<IRepositoryManager, RepositoryManager>();

            // SQL-провайдеры 
            services.AddSingleton<ISqlUserProvider, SqlUserProvider>();
            services.AddSingleton<ISqlBookProvider, SqlBookProvider>();
            services.AddSingleton<ISqlTakeReturnBookProvider, SqlTakeReturnBookProvider>();
            services.AddSingleton<ISqlProvider<Author>, SqlAuthorProvider>();
            services.AddSingleton<ISqlProvider<Publisher>, SqlPublisherProvider>();
            services.AddSingleton<ISqlProvider<Condition>, SqlConditionProvider>();

            //Кэш
            services.AddMemoryCache();

            // Сервисы 
            services.AddSingleton<UserService>();
            services.AddSingleton<BookService>();
            services.AddSingleton<TakeReturnBookService>();
            services.AddSingleton<AuthorService>();
            services.AddSingleton<ConditionService>();
            services.AddSingleton<PublisherService>();
            services.AddSingleton<ServiceManager>();

            return services;
        }
    }
}

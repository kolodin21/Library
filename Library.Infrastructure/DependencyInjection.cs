using Library.BL;
using Library.BL.Models;
using Library.BL.Service;
using Library.DAL.Interface;
using Library.DAL.Repositories;
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

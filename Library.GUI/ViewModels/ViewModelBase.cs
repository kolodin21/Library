using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Windows;
using Library.BL;
using Library.BL.Service;
using Library.Common;
using Library.DAL.Interface;
using Library.DAL.Models;
using Library.DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Condition = Library.DAL.Models.Condition;

namespace Library.GUI.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        protected IMessageLogger Logger;
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;

            OnPropertyChanged(propertyName);

            return true;
        }
        #endregion
        protected ServiceManager ServiceManager { get; set; }

        protected ViewModelBase()
        {
            #region DI

            // Создание контейнера DI
            var services = new ServiceCollection();
            // Регистрация логгера
            services.AddScoped<IMessageLogger, WpfLogger>();

            // Регистрация репозиториев
            services.AddScoped<IGetRepository, GetRepository>();
            services.AddScoped<IModificationRepository, ModificationRepository>();
            services.AddScoped<IRepositoryManager, RepositoryManager>();

            services.AddSingleton<ISqlUserProvider, SqlUserProvider>();
            services.AddSingleton<ISqlBookProvider, SqlBookProvider>();
            services.AddSingleton<ISqlTakeReturnBookProvider, SqlTakeReturnBookProvider>();
            services.AddSingleton<ISqlProvider<Author>, SqlAuthorProvider>();
            services.AddSingleton<ISqlProvider<Publisher>, SqlPublisherProvider>();
            services.AddSingleton<ISqlProvider<Condition>, SqlConditionProvider>();

            // Регистрация сервисов
            services.AddScoped<UserService>();
            services.AddScoped<BookService>();
            services.AddScoped<TakeReturnBookService>();
            services.AddScoped<AuthorService>();
            services.AddScoped<ConditionService>();
            services.AddScoped<PublisherService>();
            services.AddScoped<ServiceManager>();

            // Построение провайдера
            var serviceProvider = services.BuildServiceProvider();
            // Получение ServiceManager
            ServiceManager = serviceProvider.GetRequiredService<ServiceManager>();
            Logger = serviceProvider.GetRequiredService<IMessageLogger>();
            #endregion

            //TODO
            // Перенести DI в App.cs
        }

        protected Dictionary<string, object?> ConvertToDictionary(params Expression<Func<object?>>[] expressions)
        {
            var result = new Dictionary<string, object?>();

            foreach (var expression in expressions)
            {
                if (expression.Body is MemberExpression member)
                {
                    var propertyName = member.Member.Name; // Имя свойства
                    var propertyValue = expression.Compile().Invoke(); // Значение свойства

                    result[propertyName] = propertyValue;
                }
                else if (expression.Body is UnaryExpression unary && unary.Operand is MemberExpression unaryMember)
                {
                    var propertyName = unaryMember.Member.Name; // Имя свойства
                    var propertyValue = expression.Compile().Invoke(); // Значение свойства

                    result[propertyName] = propertyValue;
                }
                else
                {
                    throw new ArgumentException("Expression must be a property access expression.");
                }
            }

            return result;
        }
    }

    public class WpfLogger : IMessageLogger
    {
        public void Log(string message)
        {
            MessageBox.Show(message);
        }
    }
}

using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;
using Library.BL.Service;
using Library.Common;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace Library.GUI.ViewModels
{
    public abstract class ViewModelBase : ReactiveObject
    {
        //Логгер
        protected static IMessageLogger Logger { get; private set; } = null!;

        //Сервис для работы с бизнес логикой 
        protected static ServiceManager ServiceManager { get; private set; } = null!;

        public static void Initialize(IMessageLogger logger, ServiceManager serviceManager)
        {
            Logger = logger;
            ServiceManager = serviceManager;
        }

        //Преобразование свойств в Dictionary
        protected Dictionary<string, object?> ConvertToDictionary(params Expression<Func<object?>>[] expressions)
        {
            var result = new Dictionary<string, object?>();

            foreach (var expression in expressions)
            {
                switch (expression.Body)
                {
                    case MemberExpression member:
                    {
                        var propertyName = member.Member.Name; // Имя свойства
                        var propertyValue = expression.Compile().Invoke(); // Значение свойства

                        result[propertyName] = propertyValue;
                        break;
                    }
                    case UnaryExpression unary when unary.Operand is MemberExpression unaryMember:
                    {
                        var propertyName = unaryMember.Member.Name; // Имя свойства
                        var propertyValue = expression.Compile().Invoke(); // Значение свойства

                        result[propertyName] = propertyValue;
                        break;
                    }
                    default:
                        throw new ArgumentException("Expression must be a property access expression.");
                }
            }

            return result;
        }

        //Событие для отображения нужной страницы
        public event Action<UserControl>? ContentChanged;

        protected void RaiseContentChanged(UserControl newContent)
        {
            ContentChanged?.Invoke(newContent);
        }
        //Получение выбранной страницы
        protected T GetService<T>() =>
            ((App)Application.Current).ServiceProvider.GetRequiredService<T>();

        //Закрытие приложения 
        protected static void ExecExit()
        {
            Application.Current.Shutdown();
        }
    }

}

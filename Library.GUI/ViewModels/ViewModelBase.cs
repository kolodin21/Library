using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using ReactiveUI;

namespace Library.Client.GUI.ViewModels
{
    public abstract class ViewModelBase : ReactiveObject
    {
        //Логгер
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

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

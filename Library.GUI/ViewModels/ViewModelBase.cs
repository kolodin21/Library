using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Library.BL.Service;
using Library.Common;
using ReactiveUI;

namespace Library.GUI.ViewModels
{
    public abstract class ViewModelBase : ReactiveObject
    {
        protected static IMessageLogger Logger { get; private set; } = null!;
        protected static ServiceManager ServiceManager { get; private set; } = null!;
        public static void Initialize(IMessageLogger logger, ServiceManager serviceManager)
        {
            Logger = logger;
            ServiceManager = serviceManager;
        }

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
    }

}

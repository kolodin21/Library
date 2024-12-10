using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Library.BL.Service;
using Library.Common;

namespace Library.GUI.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
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

}

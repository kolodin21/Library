using System.Windows;
using System.Windows.Input;
using Library.GUI.ViewModels.CommonClasses;

namespace Library.GUI.ViewModels.LogInSystemVM
{
    class AuthorizationWindowVM : ViewModelBase
    {

        private string? _login;
        public string? Login
        {
            get => _login;
            set => SetField(ref _login, value);
        }

        private string? _password;
        public string? Password
        {
            get => _password;
            set => SetField(ref _password, value);
        }

        public ICommand EnterCommand { get; }

        public AuthorizationWindowVM()
        {
            EnterCommand = new RelayCommand(ExecEnter,CanExecEnter);
        }

        private void ExecEnter(object? parameter = null)
        { 
          var param = ConvertToDictionary(() => Login,() =>Password );

          var user = ServiceManager.UserService.GetSingleEntityByParam(param!);

          Logger.Log(user != null ? "Пользователь найден" : "Такого пользователя нет");
        }

        private bool CanExecEnter(object? parameter = null)
        {
            return !string.IsNullOrEmpty(Login) && !string.IsNullOrEmpty(Password);
        }
    }
}

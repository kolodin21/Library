using System.Windows.Input;
using Library.GUI.Configuration;
using Library.GUI.View.Admin;
using Library.GUI.View.User;
using Library.GUI.ViewModels.CommonClasses;
using static Library.GUI.ViewModels.CommonClasses.WindowManager;

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
            EnterCommand = new RelayCommand(ExecEnter, CanExecEnter);
        }
       

        private void ExecEnter(object? parameter = null)
        {
            if (AdminConfig.Login == Login && AdminConfig.Password == Password)
            {
                var adminWindow = new AdminWindow();
                OpenNewWindow(adminWindow);
            }
            else
            {
                var param = ConvertToDictionary(() => Login, () => Password);

                var user = ServiceManager.UserService.GetSingleEntityByParam(param!);

                //TODO
                // Создать сервисы для инкапсуляции запросов в GUI

                if (user is null)
                    return;

                var userWindow = new UserWindow();
                OpenNewWindow(userWindow);
            }
        }

        private bool CanExecEnter(object? parameter = null)
        {
            return !string.IsNullOrEmpty(Login) && !string.IsNullOrEmpty(Password);
        }
    }
}

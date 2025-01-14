using System.Reactive;
using System.Windows.Input;
using Library.GUI.Configuration;
using Library.GUI.View.Admin;
using Library.GUI.View.User;
using Library.GUI.ViewModels.CommonClasses;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using static Library.GUI.ViewModels.CommonClasses.WindowManager;

namespace Library.GUI.ViewModels.LogInSystemVM
{
    class AuthorizationWindowVM : ViewModelBase
    {
        [Reactive] public string? Login { get; set; }
        [Reactive] public string? Password { get; set; }

        //public ReactiveCommand<Unit, Unit> EnterCommand { get; }

        public ICommand EnterCommand { get; }
        public AuthorizationWindowVM()
        {
            EnterCommand = new RelayCommand(ExecEnter, CanExecEnter);
        }
        
        private void ExecEnter(object? param = null)
        {
            if (AdminConfig.Login == Login && AdminConfig.Password == Password)
            {
                var adminWindow = new AdminWindow();
                OpenNewWindow(adminWindow);
            }
            else
            {
                var paramConvert = ConvertToDictionary(() => Login, () => Password);

                var user = ServiceManager.UserService.GetSingleEntityByParam(paramConvert!);

                //TODO
                // Создать сервисы для инкапсуляции запросов в GUI

                if (user is null)
                    return;

                //Тестовый вывод для проверки 
                Logger.Log($"{user}");

                OpenNewWindow(new UserWindow());
            }
        }
        private bool CanExecEnter(object? param = null) => !string.IsNullOrEmpty(Login) && !string.IsNullOrEmpty(Password);
    }
}

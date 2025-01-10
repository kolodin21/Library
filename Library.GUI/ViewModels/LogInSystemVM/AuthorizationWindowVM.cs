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
        public ReactiveCommand<Unit, Unit> EnterCommand { get; }
        public AuthorizationWindowVM()
        {
            EnterCommand = ReactiveCommand.Create(
                execute: ExecEnter,
                canExecute: this.WhenAnyValue
                (
                    vm => vm.Login,
                    vm => vm.Password,
                    (login, password) => !string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(password)
                ));
        }
        
        private void ExecEnter()
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

                //Тестовый вывод для проверки 
                Logger.Log($"{user}");

                var userWindow = new UserWindow();
                OpenNewWindow(userWindow);
            }
        }
    }
}

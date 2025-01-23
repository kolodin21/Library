using System.Reactive;
using Library.GUI.Configuration;
using Library.GUI.View.Admin;
using Library.GUI.View.User;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Library.GUI.ViewModels.LogInSystemVM
{
    public class AuthorizationPageViewModel : ViewModelBase
    {
        [Reactive] public string? Login { get; set; }
        [Reactive] public string? Password { get; set; }

        public ReactiveCommand<Unit, Unit> EnterCommand { get; }

        public AuthorizationPageViewModel()
        {
            EnterCommand = ReactiveCommand.Create(ExecEnter, CanExecEnter());
        }
        
        private void ExecEnter()
        {
            if (AdminConfig.Login == Login && AdminConfig.Password == Password)
            {
                RaiseContentChanged(GetService<AdminPageView>());
            }
            else
            {
                
                var paramConvert = ConvertToDictionary(() => Login, () => Password);

                var user = ServiceManager.UserService.GetSingleEntityByParam(paramConvert!);


                if (user is null)
                    return;

                //Тестовый вывод для проверки 
                Logger.Info($"{user}");

                RaiseContentChanged(GetService<UserPageView>());
            }
        }
        private IObservable<bool> CanExecEnter()
        {
            return this.WhenAnyValue(
                vm => vm.Login,
                vm => vm.Password,
                (login, password) => !string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(password)
            );
        }
    }
}

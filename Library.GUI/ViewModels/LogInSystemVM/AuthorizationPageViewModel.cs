using System.Reactive;
using Library.Client.GUI.Configuration;
using Library.Client.GUI.View.Admin;
using Library.Client.GUI.View.User;
using Library.Client.Http;
using NLog;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Library.Client.GUI.ViewModels.LogInSystemVM
{
    public class AuthorizationPageViewModel : ViewModelBase
    {
        //Логгер
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly ManagerHttp _managerHttp = new ManagerHttp(new UserHttpClient());

        [Reactive] public string? Login { get; set; }
        [Reactive] public string? Password { get; set; }

        public ReactiveCommand<Unit, Unit> EnterCommand { get; }

        public AuthorizationPageViewModel()
        {
            EnterCommand = ReactiveCommand.CreateFromTask(ExecEnterAsync, CanExecEnter());
        }
        
        private async Task ExecEnterAsync()
        {
            if (AdminConfig.Login == Login && AdminConfig.Password == Password)
            {
                RaiseContentChanged(GetService<AdminPageView>());
            }
            else
            {
                var paramConvert = ConvertToDictionary(() => Login, () => Password);

                //var user = await ServiceManager.UserService.GetSingleEntityByParamAsync(paramConvert!);
                var user = await _managerHttp.UserHttpClient.GetSingleUser(paramConvert!);
                await Task.Delay(300);


                if (user is null)
                    return;

                Logger.Info($"{user}");
                //Тестовый вывод для проверки 

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

//Todo
//Добавить логи в DAL слой
//Начать реализовывать графический интерфейс
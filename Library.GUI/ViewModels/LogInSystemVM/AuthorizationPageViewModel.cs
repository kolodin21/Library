using System.Reactive;
using Library.Client.GUI.Configuration;
using Library.Client.GUI.View.Admin;
using Library.Client.GUI.View.LogInSystem;
using Library.Client.GUI.View.User;
using Library.Client.GUI.ViewModels.UserVM;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Library.Client.GUI.ViewModels.LogInSystemVM
{
    public class AuthorizationPageViewModel : ViewModelBase
    {
        //Логгер
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        [Reactive] public string? Login { get; set; } = "Admin";
        [Reactive] public string? Password { get; set; } = "Admin";

        public ReactiveCommand<Unit, Unit> EnterCommand { get; }
        public ReactiveCommand<Unit, Unit> BackCommand { get; }

        public AuthorizationPageViewModel()
        {
            EnterCommand = ReactiveCommand.CreateFromTask(ExecEnterAsync, CanExecEnter());
            BackCommand = ReactiveCommand.Create(ExecBack);
        }
        
        private async Task ExecEnterAsync()
        {
            if (AdminConfig.Login == Login && AdminConfig.Password == Password)
            {
                Logger.Info($"Администратор:{Login} зашёл в систему");
                RaiseContentChanged(GetPage<AdminPageView>(),"Режим администратора");
            }
            else
            {
                var paramConvert = ConvertToDictionary(() => Login, () => Password);

                var user = await ManagerHttp.UserHttpClient.GetSingleUser(paramConvert!);
                await Task.Delay(300);

                if (user is null)
                    return;

                //  Создаем `UserPageViewModel` через DI, передав `User`
                var userViewModel = ActivatorUtilities.CreateInstance<UserPageViewModel>(ServiceProvider, user);

                //  Создаем страницу через DI и устанавливаем `DataContext`
                var userPage = GetPage<UserPageView>();
                userPage.DataContext = userViewModel;

                Logger.Info($"Пользователь: {user.Login} зашел в аккаунт.");

                RaiseContentChanged(userPage,"Библиотека");
            }
        }
        private void ExecBack()
        {
            RaiseContentChanged(GetPage<MainMenuPageView>(),"Главное меню");
        }

        private IObservable<bool> CanExecEnter()
        {
            return this.WhenAnyValue(
                vm => vm.Login,
                vm => vm.Password,
                (login, password) => 
                    !string.IsNullOrEmpty(login) && 
                    !string.IsNullOrEmpty(password)
            );
        }
    }
}   
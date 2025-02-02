using System.Net.Http;
using System.Reactive;
using System.Windows;
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

        [Reactive] public string? Login { get; set; } = "kolodin21";
        [Reactive] public string? Password { get; set; } = "978509qq";
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
                try
                {
                    var paramConvert = ConvertToDictionary(() => Login, () => Password);

                    var user = await ManagerHttp.UserHttpClient.GetSingleUser(paramConvert!);
                    await Task.Delay(300);

                    if (user is null)
                    {
                        MessageBox.Show($"Пользователь пол логином {Login} не найден.","Ошибка входа",MessageBoxButton.OK,MessageBoxImage.Error);
                        ClearFields();
                        return;
                    }

                    //  Создаем `UserPageViewModel` через DI, передав `User`
                    var userViewModel = ActivatorUtilities.CreateInstance<UserPageViewModel>(ServiceProvider, user);

                    //  Создаем страницу через DI и устанавливаем `DataContext`
                    var userPage = GetPage<UserPageView>();
                    userPage.DataContext = userViewModel;


                    Logger.Info($"Пользователь: {user.Login} зашел в аккаунт.");

                    ClearFields();
                    RaiseContentChanged(userPage,"Библиотека");
                }
                catch (HttpRequestException e)
                {
                    Logger.Error(e, "Ошибка подключения");
                    MessageBox.Show("Ошибка подключения к серверу. Проверьте интернет-соединение.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception e)
                {
                    Logger.Error(e,"Ошибка при входе в аккаунт");
                    MessageBox.Show("Произошла ошибка при входе в аккаунт: " + e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
        }
        private void ExecBack()
        {
            ClearFields();
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

        private void ClearFields()
        {
            Login = string.Empty;
            Password = string.Empty;
        }
    }
}   
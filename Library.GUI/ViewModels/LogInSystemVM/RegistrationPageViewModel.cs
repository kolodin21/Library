using System.Net.Http;
using System.Reactive;
using System.Windows;
using Library.Client.GUI.View.LogInSystem;
using Library.Models.ModelsDTO;
using NLog;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Library.Client.GUI.ViewModels.LogInSystemVM
{
    public class RegistrationPageViewModel: ViewModelBase
    {
        //Логгер
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        [Reactive] public string Surname {get; set; }
        [Reactive] public string Name { get; set; }
        [Reactive] public string? Patronymic {get; set; }
        [Reactive] public string Login {get; set;}
        [Reactive] public string OnePassword {get; set;}
        [Reactive] public string TwoPassword {get; set;}
        [Reactive] public string Phone {get; set;}
        [Reactive] public string Email {get; set;}

        public ReactiveCommand<Unit,Unit> RegistrationCommand { get; }
        public ReactiveCommand<Unit,Unit> BackCommand { get; }


        public RegistrationPageViewModel()
        {
            RegistrationCommand = ReactiveCommand.CreateFromTask(ExecRegistrationAsync, CanExecRegistration());
            BackCommand = ReactiveCommand.Create(ExecBack);
        }

        private async Task ExecRegistrationAsync()
        {
            try
            {
                var user = new UserAddDto(Surname, Name, Patronymic, Login, OnePassword, Phone, Email);

                await ManagerHttp.UserHttpClient.AddUser(user);

                ClearFields();
                MessageBox.Show("Регистрация прошла успешно!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (HttpRequestException ex)
            {
                Logger.Error(ex, "Ошибка подключения");
                MessageBox.Show("Ошибка подключения к серверу. Проверьте интернет-соединение.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при регистрации");
                MessageBox.Show("Произошла ошибка при регистрации: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private IObservable<bool> CanExecRegistration()
        {
            return this.WhenAnyValue(
                vm => vm.Surname,
                vm => vm.Name,
                vm => vm.Login,
                vm => vm.OnePassword,
                vm => vm.TwoPassword,
                vm => vm.Phone,
                vm => vm.Email,
                (surname,name,login,onePassword,twoPassword,phone,email) => 
                    !string.IsNullOrEmpty(surname) &&
                    !string.IsNullOrEmpty(name) &&
                    !string.IsNullOrEmpty(login) &&(
                    !string.IsNullOrEmpty(onePassword) &&
                    !string.IsNullOrEmpty(twoPassword) && 
                     onePassword == twoPassword) &&
                    !string.IsNullOrEmpty(phone) &&
                    !string.IsNullOrEmpty(email));
        }

        private void ExecBack()
        {
            ClearFields();
            RaiseContentChanged(GetPage<MainMenuPageView>(), "Главное меню");
        }

        private void ClearFields()
        {
            Surname = string.Empty;
            Name = string.Empty;
            Patronymic = string.Empty;
            Login = string.Empty;
            Phone = string.Empty;
            Email = string.Empty;
        }
    }
}

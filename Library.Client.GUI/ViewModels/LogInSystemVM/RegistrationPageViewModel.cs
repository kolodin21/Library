using System.Net.Http;
using System.Reactive;
using System.Windows;
using System.Windows.Documents;
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
                //var property = new List<string>{Login,Phone,Email};
                //for (var i = 0; i < property.Count; i++)
                //{
                //    var paramConvert = ConvertToDictionary(() => property[i]);
                //    var findUser = await ManagerHttp.UserHttpClient.GetSingleUser(paramConvert!);

                //    if (findUser != null)
                //    {
                //        MessageBox.Show("Такой пользователь уже зарегистриван");
                //        return;
                //    }
                //}
                var paramConvert = ConvertToDictionary(() => Login);
                var findUser = await ManagerHttp.UserHttpClient.GetSingleUser(paramConvert!);
                
                //TODO Доделать логику поиска пользователя по свойствам 


                if (findUser != null)
                {
                    MessageBox.Show("Такой пользователь уже зарегистрирован");
                    return;
                }

                var user = new UserAddDto(Surname, Name, Patronymic, Login, OnePassword, Phone, Email);
                await ManagerHttp.UserHttpClient.AddUser(user);

                ClearFields();
                Logger.Info($"Пользователь {Login} успешно зарегистирован!");
                MessageBox.Show("Регистрация прошла успешно!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (HttpRequestException e)
            {
                Logger.Error(e, "Ошибка подключения");
                MessageBox.Show("Ошибка подключения к серверу. Проверьте интернет-соединение.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception e)
            {
                Logger.Error(e, "Ошибка при регистрации");
                MessageBox.Show("Произошла ошибка при регистрации: " + e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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

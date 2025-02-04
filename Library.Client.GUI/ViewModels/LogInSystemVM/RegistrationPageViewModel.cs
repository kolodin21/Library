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

        [Reactive] public string Surname { get; set; } = "Иванов";
        [Reactive] public string Name { get; set; } = "Иван";
        [Reactive] public string? Patronymic { get; set; } = "Иванович";
        [Reactive] public string Login { get; set; } = "Ivanoovich";
        [Reactive] public string OnePassword { get; set; } = "123456";
        [Reactive] public string TwoPassword { get; set; } = "123456";
        [Reactive] public string Phone { get; set; } = "89211123312";
        [Reactive] public string Email { get; set; } = "Ivanovich@gmail.com";

        public ReactiveCommand<Unit,Unit> RegistrationCommand { get; }
        public ReactiveCommand<Unit,Unit> BackCommand { get; }


        public RegistrationPageViewModel()
        {
            RegistrationCommand = ReactiveCommand.CreateFromTask(ExecRegistrationAsync, CanExecRegistration());
            BackCommand = ReactiveCommand.Create(ExecBack);
        }
        //TODO Добавить проверки на ввод данных и подсвечивание для каждого поля отдельно

        private async Task ExecRegistrationAsync()
        {
            try
            {
                var prop = new List<Dictionary<string, object?>>
                {
                    ConvertToDictionary(() =>Login ),
                    ConvertToDictionary(() =>Phone ),
                    ConvertToDictionary(() =>Email )
                };

                //Проверка на уже существуещего пользователя
                foreach (var item in prop)
                {
                    var findUser = await ManagerHttp.UserHttpClient.GetSingleUser(item!);
                    if (findUser == null) 
                        continue;

                    if(findUser.Login == Login)
                        MessageBox.Show("Пользователь с таким логином уже зарегистирован");
                    if (findUser.Phone == Phone)
                        MessageBox.Show("Пользователь с таким телефоном уже зарегистирован");
                    if (findUser.Email == Email)
                        MessageBox.Show("Пользователь с такой почтой уже зарегистирован");
                    return;
                }

                //Регистарция нового пользователя
                var user = new UserAddDto(Surname, Name, Patronymic, Login, OnePassword, Phone, Email);
                await ManagerHttp.UserHttpClient.AddUser(user);

                ClearFields();
                Logger.Info($"Пользователь {Login} успешно зарегистирован!");
                MessageBox.Show("Регистрация прошла успешно!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                RaiseContentChanged(GetPage<MainMenuPageView>(), NamePage.MainMenu);
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
            RaiseContentChanged(GetPage<MainMenuPageView>(), NamePage.MainMenu);
        }

        private void ClearFields()
        {
            Surname = string.Empty;
            Name = string.Empty;
            Patronymic = string.Empty;
            Login = string.Empty;
            OnePassword = string.Empty;
            TwoPassword = string.Empty;
            Phone = string.Empty;
            Email = string.Empty;
        }
    }
}

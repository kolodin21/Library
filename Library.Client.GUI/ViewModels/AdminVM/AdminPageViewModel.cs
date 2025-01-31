using System.Collections.ObjectModel;
using System.Net.Http;
using System.Reactive;
using System.Windows;
using Library.Client.Http;
using Library.Models;
using NLog;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Library.Client.GUI.ViewModels.AdminVM
{
    public class AdminPageViewModel : ViewModelBase
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        //Заглушка чтобы проверить работоспособность 
        [Reactive]public ObservableCollection<User>? Users { get; set; }

        public ReactiveCommand<Unit, Unit> LoadUsersCommand { get; }

        public AdminPageViewModel()
        {
            LoadUsersCommand = ReactiveCommand.CreateFromTask(LoadUsersAsync);
            LoadUsersCommand.Execute().Subscribe();
        }

        private async Task LoadUsersAsync()
        {
            try
            {
                var users =await ManagerHttp.UserHttpClient.GetAllUsers();

                if (users != null) 
                {
                    Users = new ObservableCollection<User>(users);
                }
            }
            catch (HttpRequestException e)
            {
                Logger.Error(e, "Ошибка подключения");
                MessageBox.Show("Ошибка подключения к серверу. Проверьте интернет-соединение.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception e)
            {
                Logger.Info(e.Message);
            }
        }
    }
}
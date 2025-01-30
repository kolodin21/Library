using System.Collections.ObjectModel;
using System.Reactive;
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
                Logger.Info("Запрос отправлен");
                var users =await ManagerHttp.UserHttpClient.GetAllUsers();

                if (users != null) 
                {
                    Users = new ObservableCollection<User>(users);
                }
            }
            catch (Exception e)
            {
                Logger.Info(e.Message);
                throw;
            }
        }
    }
}
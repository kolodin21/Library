using System.Collections.ObjectModel;
using System.Reactive;
using Library.Client.Http;
using Library.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Library.Client.GUI.ViewModels.AdminVM
{
    public class AdminPageViewModel : ViewModelBase
    {
        private readonly ManagerHttp _managerHttp = new ManagerHttp(new UserHttpClient());

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
            var users =await _managerHttp.UserHttpClient.GetAllUsers();
            //var users = await Client.GetFromJsonAsync<IEnumerable<User>>(GetAllUsers);

            if (users != null) 
            {
                Users = new ObservableCollection<User>(users);
            }
        }
    }
}
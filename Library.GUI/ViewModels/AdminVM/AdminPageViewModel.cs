using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reactive;
using Library.Server.BL.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Library.Client.GUI.ViewModels.AdminVM
{
    public class AdminPageViewModel : ViewModelBase
    {
        private static readonly HttpClient Client = new();

        private static readonly Uri GetAllUsers = new Uri("http://localhost:5241/AllUsers");


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
            //var users = await ServiceManager.UserService.GetAllEntitiesAsync();
            var users = await Client.GetFromJsonAsync<IEnumerable<User>>(GetAllUsers);

            if (users != null) 
            {
                Users = new ObservableCollection<User>(users);
            }
        }
    }
}
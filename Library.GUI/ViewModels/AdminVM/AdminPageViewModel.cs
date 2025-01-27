using System.Collections.ObjectModel;
using System.Reactive;
using Library.BL.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Library.GUI.ViewModels.AdminVM
{
    class AdminPageViewModel : ViewModelBase
    {
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
            var users = await ServiceManager.UserService.GetAllEntitiesAsync();
            if (users != null)
            {
                Users = new ObservableCollection<User>(users);
            }
        }
    }
}

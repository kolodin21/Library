using System.Collections.ObjectModel;
using Library.BL.Models;

namespace Library.GUI.ViewModels.AdminVM
{
    class AdminWindowVM : ViewModelBase
    {
        //Заглушка чтобы проверить работоспособность 
        public ObservableCollection<User> Users { get; set; }

        public AdminWindowVM()
        {
            var users = ServiceManager.UserService.GetAllEntities();

            Users = new ObservableCollection<User>(users);
        }

    }
}

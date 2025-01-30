using Library.Models;
using NLog;
using ReactiveUI.Fody.Helpers;

namespace Library.Client.GUI.ViewModels.UserVM
{
    public class UserPageViewModel
    {
        //Логгер
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static User CurrentUser { get; set; }

        [Reactive]public string UserName { get; private set; }

        public UserPageViewModel(User user)
        {
            CurrentUser = user;
            UserName = CurrentUser.Name;

        }
        public UserPageViewModel()
        {
            
        }
    }
}

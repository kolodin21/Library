using System.Collections.ObjectModel;
using System.Net.Http;
using System.Reactive;
using Library.Models;
using NLog;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Library.Client.GUI.ViewModels.UserVM
{
    public class UserPageViewModel : ViewModelBase
    {
        //Логгер
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public User CurrentUser { get; set; }

        [Reactive] public string NameCurrentCollection { get; set; }
        [Reactive] public object? CurrentSelectedCollections { get; set; }

        [Reactive]public string UserName { get; private set; }

        public ReactiveCommand<Unit, Unit> LoadBooksCommand { get; }

        public UserPageViewModel(User user)
        {
            CurrentUser = user;
            UserName = CurrentUser.Name;
            LoadBooksCommand = ReactiveCommand.CreateFromTask(LoadActivityBooks);
            LoadBooksCommand.Execute().Subscribe();
        }

        public UserPageViewModel(){}


        private async Task LoadActivityBooks()
        {
            var userId = CurrentUser.Id;
            var paramConvert = ConvertToDictionary(() => userId); //Fixme

            CurrentSelectedCollections = await ManagerHttp.BookHttpClient.GetActivityBooks(paramConvert!);

        }
    }
}

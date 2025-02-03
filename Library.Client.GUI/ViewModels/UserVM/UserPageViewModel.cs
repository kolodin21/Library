using System.Collections.ObjectModel;
using System.Net.Http;
using System.Reactive;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Library.Client.GUI.View.LogInSystem;
using Library.Models;
using Library.Models.ModelsDTO;
using NLog;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Library.Client.GUI.ViewModels.UserVM
{
    public class UserPageViewModel : ViewModelBase
    {
        //Логгер
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public User CurrentUser { get;}

        [Reactive] public string NameCurrentCollection { get; set; }
        [Reactive] public object? CurrentSelectedCollections { get; set; }

        public string UserName { get; private set; }
        public int UserId { get; private set; }

        public ObservableCollection<DataGridColumn> Columns { get; set; } = [];

        public ReactiveCommand<Unit, Unit> LoadActivityBooksUserCommand { get; }
        public ReactiveCommand<Unit, Unit> LoadHistoryBooksUserCommand { get; }
        public ReactiveCommand<Unit, Unit> LoadActualBooksCommand { get; }
        public ReactiveCommand<Unit, Unit> BackCommand { get; }
        public ReactiveCommand<Unit, Unit> ExitCommand { get; }


        public UserPageViewModel(){}
        public UserPageViewModel(User user)
        {
            CurrentUser = user;
            UserName = CurrentUser.Name;
            UserId = CurrentUser.Id;

            LoadActivityBooksUserCommand = ReactiveCommand.CreateFromTask(LoadActivityBooks);
            LoadActivityBooksUserCommand.Execute().Subscribe();

            LoadActualBooksCommand = ReactiveCommand.CreateFromTask(LoadActualBooks);
            LoadHistoryBooksUserCommand = ReactiveCommand.CreateFromTask(LoadHistoryBooks);

            BackCommand = ReactiveCommand.Create(ExecBack);
            ExitCommand = ReactiveCommand.Create(ExecExit);
        }

        private async Task LoadActivityBooks()
        {
            var userParams = ConvertToDictionary(() => UserId);

            NameCurrentCollection = $"Активные книги: {UserName}";
            try
            {
                CurrentSelectedCollections = await ManagerHttp.BookHttpClient.GetActivityBooks(userParams!);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            InitColumns();
            Columns.Add(CreateColumn("Дата выдачи", "DateIssuance", new DataGridLength(1, DataGridLengthUnitType.Star)));
        }
        private async Task LoadHistoryBooks()
        {
            var userParams = ConvertToDictionary(() => UserId);

            NameCurrentCollection = $"История: {UserName}";
            CurrentSelectedCollections = await ManagerHttp.BookHttpClient.GetHistoryBooks(userParams!);

            InitColumns();
            Columns.Add(CreateColumn("Дата выдачи", "DateIssuance"));
            Columns.Add(CreateColumn("Дата возврата", "DateReturn", new DataGridLength(1, DataGridLengthUnitType.Star)));
        }
        private async Task LoadActualBooks()
        {
            NameCurrentCollection = "Доступные книги библиотеки";
            CurrentSelectedCollections = await ManagerHttp.BookHttpClient.GetActualBooksLibrary();

            InitColumns();
            Columns.Add(CreateColumn("Остаток книг", "BalanceBook", new DataGridLength(1, DataGridLengthUnitType.Star)));
        }


        private void ExecBack()
        {
            RaiseContentChanged(GetPage<MainMenuPageView>(),NamePage.MainMenu);
        }


        private DataGridTextColumn CreateColumn(string header, string? bindingPath, DataGridLength? width = null)
        {
            return new DataGridTextColumn
            {
                Header = header,
                Binding = bindingPath != null ? new Binding(bindingPath) : null,
                Width = width ?? DataGridLength.Auto // Если ширина не указана, используем Auto
            };
        }

        private void InitColumns()
        {
            Columns.Clear();
            Columns.Add(CreateColumn("Автор", "NameAuthor"));
            Columns.Add(CreateColumn("Название", "Title"));
            Columns.Add(CreateColumn("Год выпуска", "Year"));
            Columns.Add(CreateColumn("Состояние", "Condition"));
            Columns.Add(CreateColumn("Издательство", "Publisher"));
        }
    }
}

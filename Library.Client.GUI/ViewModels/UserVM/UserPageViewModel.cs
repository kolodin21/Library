using System.Collections.ObjectModel;
using System.Reactive;
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

        public string UserName { get; }
        public int UserId { get; }

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

        //Todo: Добавить проверки; Реализовать другие кнопки и функционал.
        //Todo 1. Реализовать функцию взятия книги в руки и обновления UI после.
        //Todo 2. Личный кабинет
        //Todo 3. Настройки 


        private async Task LoadBooksAsync<T>(
           Func<Task<IEnumerable<T>?>> getBooksFunc,
           string collectionName,
           Action? configureColumns = null)
        {
            try
            {
                NameCurrentCollection = collectionName;
                CurrentSelectedCollections = await getBooksFunc();

                InitColumns();
                configureColumns?.Invoke(); // Добавление специфических колонок
            }
            catch (Exception e)
            {
                Logger.Error(e, "Ошибка загрузки книг");
            }
        }

        // Вызовы вместо отдельных методов

        //Загрузка активных книг пользователя
        public Task LoadActivityBooks() =>
            LoadBooksAsync<BookUserActivityViewDto>(
                () => ManagerHttp.BookHttpClient.GetActivityBooks(ConvertToDictionary(() => UserId)!),
                $"Активные книги: {UserName}",
                () => Columns.Add(CreateColumn("Дата выдачи", "DateIssuance", new DataGridLength(1, DataGridLengthUnitType.Star)))
            );
        
        //Загрузка истории взятия книг
        public Task LoadHistoryBooks() =>
            LoadBooksAsync<BookUserHistoryViewDto>(
                () => ManagerHttp.BookHttpClient.GetHistoryBooks(ConvertToDictionary(() => UserId)!),
                $"История: {UserName}",
                () =>
                {
                    Columns.Add(CreateColumn("Дата выдачи", "DateIssuance"));
                    Columns.Add(CreateColumn("Дата возврата", "DateReturn", new DataGridLength(1, DataGridLengthUnitType.Star)));
                }
            );

        //Актуальные книги библиотеки
        public Task LoadActualBooks() =>
            LoadBooksAsync<Book>(
                ManagerHttp.BookHttpClient.GetActualBooksLibrary,
                "Доступные книги библиотеки",
                () => Columns.Add(CreateColumn("Остаток книг", "BalanceBook", new DataGridLength(1, DataGridLengthUnitType.Star)))
            );

        // Инициализация базовых колонок
        private void InitColumns()
        {
            Columns.Clear();
            var defaultColumns = new[]
            {
              ("Автор", "NameAuthor"),
              ("Название", "Title"),
              ("Год выпуска", "Year"),
              ("Состояние", "Condition"),
              ("Издательство", "Publisher")
            };

            foreach (var (header, binding) in defaultColumns)
            {
                Columns.Add(CreateColumn(header, binding));
            }
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

    }
}

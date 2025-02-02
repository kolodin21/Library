using System.Windows.Controls;
using Library.Client.GUI.View.LogInSystem;
using Library.Client.GUI.ViewModels.LogInSystemVM;
using Library.Client.GUI.ViewModels.UserVM;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using ReactiveUI.Fody.Helpers;

namespace Library.Client.GUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        //Логгер
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        [Reactive] public UserControl? CurrentContent { get; set; }
        [Reactive] public string Title { get; set; }

        public MainWindowViewModel()
        {
            CurrentContent = GetPage<MainMenuPageView>();
            Title = "Главное меню";

            // Инициализация и подписка на все ViewModel
            InitializeAndSubscribeToContentChanges<MainMenuPageViewModel>();
            InitializeAndSubscribeToContentChanges<AuthorizationPageViewModel>();
            InitializeAndSubscribeToContentChanges<RegistrationPageViewModel>();
            InitializeAndSubscribeToContentChanges<UserPageViewModel>();

            Logger.Info("Подписались на все события");
        }

        private void InitializeAndSubscribeToContentChanges<TViewModel>()
            where TViewModel : class
        {
            var viewModel = GetPage<TViewModel>();
            SubscribeToContentChanged(viewModel);
        }

        //Подписка на обновление данных
        private void SubscribeToContentChanged<TViewModel>(TViewModel viewModel)
            where TViewModel : class
        {
            if (viewModel is IContentChanger contentChanger)
            {
                contentChanger.ContentChanged += (newContent, newTitle) =>
                {
                    CurrentContent = newContent;
                    Title = newTitle; // Обновляем заголовок окна
                };
            }
        }
    }

    public class PageFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public PageFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public TView CreatePage<TView, TViewModel>(object? parameter = null)
            where TView : UserControl, new()
            where TViewModel : ViewModelBase
        {
            // Создаем ViewModel через DI и передаём параметр, если требуется
            var viewModel = ActivatorUtilities.CreateInstance<TViewModel>(_serviceProvider, parameter);

            // Создаём View и устанавливаем DataContext
            var view = new TView
            {
                DataContext = viewModel
            };

            return view;
        }
    }
}
using System.Windows.Controls;
using Library.Client.GUI.View.LogInSystem;
using Library.Client.GUI.ViewModels.LogInSystemVM;
using NLog;
using ReactiveUI.Fody.Helpers;

namespace Library.Client.GUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        //Логгер
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly MainMenuPageViewModel _mainMenuPageViewModel;
        private readonly AuthorizationPageViewModel _authorizationPageViewModel;

        [Reactive] public UserControl? CurrentContent { get; set; }

        public MainWindowViewModel()
        {
            CurrentContent = GetPage<MainMenuPageView>();

            // Инициализация начального представлений
            _mainMenuPageViewModel = GetPage<MainMenuPageViewModel>();
            _authorizationPageViewModel = GetPage<AuthorizationPageViewModel>();

            // Подписываемся на событие ContentChanged
            _mainMenuPageViewModel.ContentChanged += newContent
                => CurrentContent = newContent;

            _authorizationPageViewModel.ContentChanged += newContent
                => CurrentContent = newContent;

            Logger.Info("Подписались на все события");
        }
    }
}
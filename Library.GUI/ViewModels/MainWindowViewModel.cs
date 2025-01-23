using ReactiveUI.Fody.Helpers;
using System.Windows.Controls;
using Library.GUI.View.LogInSystem;
using Library.GUI.ViewModels.LogInSystemVM;

namespace Library.GUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly MainMenuPageViewModel _mainMenuPageViewModel;
        private readonly AuthorizationPageViewModel _authorizationPageViewModel;

        [Reactive] public UserControl? CurrentContent { get; set; }

        public MainWindowViewModel()
        {
            CurrentContent = GetService<MainMenuPageView>();

            // Инициализация начального представлений
            _mainMenuPageViewModel = GetService<MainMenuPageViewModel>();
            _authorizationPageViewModel = GetService<AuthorizationPageViewModel>();

            // Подписываемся на событие ContentChanged
            _mainMenuPageViewModel.ContentChanged += newContent
                => CurrentContent = newContent;

            _authorizationPageViewModel.ContentChanged += newContent
                => CurrentContent = newContent;

        }
    }
}

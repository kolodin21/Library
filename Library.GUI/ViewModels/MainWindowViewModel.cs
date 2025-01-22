using ReactiveUI.Fody.Helpers;
using System.Windows.Controls;
using Library.GUI.View.LogInSystem;
using Library.GUI.ViewModels.LogInSystemVM;

namespace Library.GUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly MainMenuPageViewModel _mainMenuPageViewModel;
        [Reactive] public UserControl? CurrentContent { get; set; }

        public MainWindowViewModel()
        {
            CurrentContent = GetService<MainMenuPageView>();

            // Инициализация представлений
            _mainMenuPageViewModel = GetService<MainMenuPageViewModel>();

            // Подписываемся на событие ContentChanged
            _mainMenuPageViewModel.ContentChanged += newContent
                => CurrentContent = newContent;
        }
    }
}

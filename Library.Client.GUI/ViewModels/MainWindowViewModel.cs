using System.Windows.Controls;
using Library.Client.GUI.View.LogInSystem;
using Library.Client.GUI.ViewModels.LogInSystemVM;
using Library.Client.GUI.ViewModels.UserVM;
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

            // Инициализация начального представлений
             var mainMenuPageViewModel = GetPage<MainMenuPageViewModel>();
             var authorizationPageViewModel = GetPage<AuthorizationPageViewModel>();
             var registrationPageViewModel = GetPage<RegistrationPageViewModel>();
             var userPageViewModel = GetPage<UserPageViewModel>();

            SubscribeToContentChanged(mainMenuPageViewModel);
            SubscribeToContentChanged(authorizationPageViewModel);
            SubscribeToContentChanged(registrationPageViewModel);
            SubscribeToContentChanged(userPageViewModel);

            Logger.Info("Подписались на все события");
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
}
using System.Windows.Controls;
using Library.Client.GUI.View.LogInSystem;
using Library.Client.GUI.ViewModels.LogInSystemVM;
using Library.Client.GUI.ViewModels.UserVM;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using ReactiveUI.Fody.Helpers;

namespace Library.Client.GUI.ViewModels
{

    public static class NamePage
    {
        public static string MainMenu => "Главное меню";
        public static string Authorization => "Авторизация";
        public static string Registration => "Регистрация";
        public static string Administrator => "Режим администратора";
        public static string User => "Библиотека";
    }


    public class MainWindowViewModel : ViewModelBase
    {
        //Логгер
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        [Reactive] public UserControl? CurrentContent { get; set; }
        [Reactive] public string Title { get; set; }


        public MainWindowViewModel()
        {
            Title = NamePage.MainMenu;
            CurrentContent = GetPage<MainMenuPageView>();

            //// Инициализация и подписка на все ViewModel
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
            SubscribeToContentChanged(viewModel, (newContent, newTitle) =>
            {
                CurrentContent = newContent;
                Title = newTitle;
            });
        }
    }
}
using Library.GUI.View.LogInSystem;
using ReactiveUI;
using System.Reactive;
using System.Windows;
using System.Windows.Input;
using Library.GUI.ViewModels.CommonClasses;
using static Library.GUI.ViewModels.CommonClasses.WindowManager;

namespace Library.GUI.ViewModels.LogInSystemVM
{
    // ReSharper disable once InconsistentNaming
    public class MainWindowVM : ViewModelBase
    {
        //public ReactiveCommand<Unit,Unit> LoginCommand { get; }
        //public ReactiveCommand<Unit, Unit> RegistrationCommand { get;  }
        //public ReactiveCommand<Unit, Unit> ExitCommand { get; }

        public ICommand LoginCommand { get;  }
        public ICommand RegistrationCommand { get; }
        public ICommand ExitCommand { get; }
        public MainWindowVM()
        {
            LoginCommand = new RelayCommand(ExecLogin);
            RegistrationCommand = new RelayCommand(ExecRegistration);
            ExitCommand = new RelayCommand(ExecExit);
        }

        private static void ExecLogin(object? param = null)
        {
            OpenNewWindow(new AuthorizationWindow());
        }

        private static void ExecRegistration(object? param = null)
        {
            OpenNewWindow(new RegistrationWindow());
        }

        private static void ExecExit(object? param = null)
        {
            Application.Current.Shutdown();
        }
    }
}
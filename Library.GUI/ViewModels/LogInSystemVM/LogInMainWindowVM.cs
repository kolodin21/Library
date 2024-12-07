using System.Windows;
using System.Windows.Input;
using Library.GUI.View.LogInSystem;
using Library.GUI.ViewModels.CommonClasses;
using static Library.GUI.ViewModels.CommonClasses.WindowManager;

namespace Library.GUI.ViewModels.LogInSystemVM
{
    // ReSharper disable once InconsistentNaming
    public class LogInMainWindowVM : ViewModelBase
    {
        public ICommand LoginCommand { get; }
        public ICommand RegistrationCommand { get;  }
        public ICommand ExitCommand { get; }

        public LogInMainWindowVM()
        {
            LoginCommand = new RelayCommand(ExecLogin);
            RegistrationCommand = new RelayCommand(ExecRegistration);
            ExitCommand = new RelayCommand(ExecExit);
        }
        private void ExecLogin(object? parameter = null)
        {
            var loginWindow = new AuthorizationWindow();
            OpenNewWindow(loginWindow);
        }
        private void ExecRegistration(object? parameter = null)
        {
            var registrationWindow = new RegistrationWindow();
            OpenNewWindow(registrationWindow);
        }

        private void ExecExit(object? parameter = null)
        {
            Application.Current.Shutdown();
        }

    }
}
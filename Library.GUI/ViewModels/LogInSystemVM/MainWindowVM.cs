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
        public ICommand OpenCommand { get; set; }
        public ReactiveCommand<Unit,Unit> LoginCommand { get; }
        public ReactiveCommand<Unit, Unit> RegistrationCommand { get;  }
        public ReactiveCommand<Unit, Unit> ExitCommand { get; }
        public MainWindowVM()
        {
            //OpenCommand = new RelayCommand(ExecLogin);

            LoginCommand = ReactiveCommand.Create(
                ExecLogin,
                outputScheduler: RxApp.MainThreadScheduler
            );
            RegistrationCommand = ReactiveCommand.Create(ExecRegistration);
            ExitCommand = ReactiveCommand.Create(ExecExit);
        }
        //Todo разобраться как работает реактивные команды и как сделать так,чтобы они работали в одном потоке

        private static void ExecLogin()
        {
            MessageBox.Show($"ExecLogin вызван из потока: {Thread.CurrentThread.ManagedThreadId}");

            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show($"Dispatcher поток: {Application.Current.Dispatcher.Thread.ManagedThreadId}");
                OpenNewWindow(new AuthorizationWindow());
            });
        }

        private static void ExecRegistration()
        {
            var registrationWindow = new RegistrationWindow();
            OpenNewWindow(registrationWindow);
        }

        private static void ExecExit()
        {
            Application.Current.Shutdown();
        }
    }
}
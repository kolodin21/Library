using ReactiveUI;
using System.Reactive;
using Library.GUI.View.LogInSystem;

namespace Library.GUI.ViewModels.LogInSystemVM
{
    public class MainMenuPageViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, Unit> LoginCommand { get; }
        public ReactiveCommand<Unit, Unit>? RegistrationCommand { get; }
        public ReactiveCommand<Unit, Unit>? ExitCommand { get; }

        public MainMenuPageViewModel()
        {
            // Реализация команд
            LoginCommand = ReactiveCommand.Create(() =>
                RaiseContentChanged(GetService<AuthorizationPageView>()));

            RegistrationCommand = ReactiveCommand.Create(()=>
                RaiseContentChanged(GetService<RegistrationPageView>()));

            ExitCommand = ReactiveCommand.Create(ExecExit);
        }
    }
}
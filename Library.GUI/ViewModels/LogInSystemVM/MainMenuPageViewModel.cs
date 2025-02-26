﻿using System.Reactive;
using Library.Client.GUI.View.LogInSystem;
using ReactiveUI;

namespace Library.Client.GUI.ViewModels.LogInSystemVM
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
                RaiseContentChanged(GetPage<AuthorizationPageView>()));

            RegistrationCommand = ReactiveCommand.Create(()=>
                RaiseContentChanged(GetPage<RegistrationPageView>()));

            ExitCommand = ReactiveCommand.Create(ExecExit);
        }
    }
}
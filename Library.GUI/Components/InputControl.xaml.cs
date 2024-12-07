﻿using System.Windows;
using System.Windows.Controls;

namespace Library.GUI.Components
{
    /// <summary>
    /// Логика взаимодействия для InputControl.xaml
    /// </summary>
    public partial class InputControl : UserControl
    {
        public static readonly DependencyProperty LabelProperty;
        public static readonly DependencyProperty InputTextProperty;
        public static readonly DependencyProperty IsReadOnlyProperty;

        static InputControl()
        {
            LabelProperty = DependencyProperty.Register(nameof(Label), typeof(string), typeof(InputControl));
            InputTextProperty = DependencyProperty.Register(nameof(InputText), typeof(string), typeof(InputControl));
            IsReadOnlyProperty = DependencyProperty.Register(nameof(IsReadOnly), typeof(bool), typeof(InputControl));
        }

        public string Label
        {
            get => (string)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }

        public string InputText
        {
            get => (string)GetValue(InputTextProperty);
            set => SetValue(InputTextProperty, value);
        }

        public bool IsReadOnly
        {
            get => (bool)GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, value);
        }

        public InputControl()
        {
            InitializeComponent();
        }
    }
}

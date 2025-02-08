using System.Reactive.Disposables;
using ReactiveUI;
using System.Windows;
using System.Windows.Controls;
using Library.Models;

namespace Library.Client.GUI.Components
{
    /// <summary>
    /// Логика взаимодействия для InputControl.xaml
    /// </summary>
    public partial class InputControl :UserControl
    {
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(nameof(Label), typeof(string), typeof(InputControl));

        public static readonly DependencyProperty InputTextProperty =
            DependencyProperty.Register(nameof(InputText), typeof(string), typeof(InputControl),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register(nameof(IsReadOnly), typeof(bool), typeof(InputControl));

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

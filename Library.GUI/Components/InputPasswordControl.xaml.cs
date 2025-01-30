using System.Windows;
using System.Windows.Controls;

namespace Library.Client.GUI.Components
{
    /// <summary>
    /// Логика взаимодействия для InputControl.xaml
    /// </summary>
    public partial class InputPasswordControl : UserControl
    {
        public static readonly DependencyProperty LabelProperty = 
            DependencyProperty.Register(nameof(Label), typeof(string), typeof(InputPasswordControl));

        public static readonly DependencyProperty InputPasswordProperty =
            DependencyProperty.Register(nameof(InputPassword), typeof(string), typeof(InputPasswordControl),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty IsReadOnlyProperty =
           DependencyProperty.Register(nameof(IsReadOnly), typeof(bool), typeof(InputPasswordControl));


        public string Label
        {
            get => (string)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }

        public string InputPassword
        {
            get => (string)GetValue(InputPasswordProperty);
            set => SetValue(InputPasswordProperty, value);
        }

        public bool IsReadOnly
        {
            get => (bool)GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, value);
        }

        public InputPasswordControl()
        {
            InitializeComponent();
        }
    }
}
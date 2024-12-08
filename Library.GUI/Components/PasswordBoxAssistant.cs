using System.Windows.Controls;
using System.Windows;

namespace Library.GUI.Components
{
    public static class PasswordBoxAssistant
    {
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached(
                "Password",
                typeof(string),
                typeof(PasswordBoxAssistant),
                new PropertyMetadata(string.Empty, OnBoundPasswordChanged));

        public static readonly DependencyProperty BindPasswordProperty =
            DependencyProperty.RegisterAttached(
                "BindPassword",
                typeof(bool),
                typeof(PasswordBoxAssistant),
                new PropertyMetadata(false, OnBindPasswordChanged));

        public static string GetPassword(DependencyObject obj) => (string)obj.GetValue(PasswordProperty);
        public static void SetPassword(DependencyObject obj, string value) => obj.SetValue(PasswordProperty, value);

        public static bool GetBindPassword(DependencyObject obj) => (bool)obj.GetValue(BindPasswordProperty);
        public static void SetBindPassword(DependencyObject obj, bool value) => obj.SetValue(BindPasswordProperty, value);

        private static void OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not PasswordBox passwordBox)
                return;

            passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;
            if (!GetBindPassword(passwordBox))
                return;

            var newPassword = (string)e.NewValue;
            if (passwordBox.Password != newPassword)
            {
                passwordBox.Password = newPassword;
            }

            passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
        }

        private static void OnBindPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not PasswordBox passwordBox)
                return;

            var isBindingEnabled = (bool)e.NewValue;

            if (isBindingEnabled)
            {
                passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
            }
            else
            {
                passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;
            }
        }

        private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                SetPassword(passwordBox, passwordBox.Password);
            }
        }
    }
}

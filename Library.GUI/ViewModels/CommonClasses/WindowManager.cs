using System.Windows;

namespace Library.GUI.ViewModels.CommonClasses;

public class WindowManager
{
    public static void OpenNewWindow(Window newWindow)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            newWindow.Show();
            if (Application.Current.MainWindow != null)
            {
                Application.Current.MainWindow.Close();
            }
            Application.Current.MainWindow = newWindow;
        });
    }
}

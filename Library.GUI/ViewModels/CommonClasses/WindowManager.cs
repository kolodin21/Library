using System.Windows;

namespace Library.GUI.ViewModels.CommonClasses;

public class WindowManager
{
    public static void OpenNewWindow(Window newWindow)
    {
        try
        {
            newWindow.Show();
            if (Application.Current.MainWindow != null)
                Application.Current.MainWindow.Close();
            Application.Current.MainWindow = newWindow;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка при открытии окна: {ex.Message}");
        }
    }
}
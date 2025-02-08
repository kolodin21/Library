using System.Windows;
using System.Windows.Controls;
using Library.Client.GUI.ViewModels.UserVM;

namespace Library.Client.GUI.View.User
{
    /// <summary>
    /// Логика взаимодействия для UserPageView.xaml
    /// </summary>
    public partial class UserPageView : UserControl
    {
        public UserPageView()
        {
            InitializeComponent();
        }

        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is not DataGrid dataGrid || DataContext is not UserPageViewModel viewModel)
                return;

            // Очистка и добавление начальных колонок
            dataGrid.Columns.Clear();
            foreach (var column in viewModel.Columns)
            {
                dataGrid.Columns.Add(column);
            }

            // Подписка на изменение коллекции колонок
            viewModel.Columns.CollectionChanged += (_, __) =>
            {
                dataGrid.Columns.Clear();
                foreach (var column in viewModel.Columns)
                {
                    dataGrid.Columns.Add(column);
                }
            };

        }
    }
}

using System.Windows;
using LibrariesManageSystem.Extensions;

namespace LibrariesManageSystem.SystemWindows
{
    public partial class BooksViewList : Window
    {
        public BooksViewList()
        {
            InitializeComponent();
        }

        private void ButtonViewBackTo_OnClick(object sender, RoutedEventArgs e)
        {
            var system = new SystemMainWindow
            {
                WindowStartupLocation = WindowStartupLocation.Manual,
                Left = Left,
                Top = Top
            };
            system.Show();
            this?.Close();
        }

        private void ButtonRefreshBooksListInformation_OnClick(object sender, RoutedEventArgs e)
        {
            var mysqlCmd = $"select * from {MySqlConnectDataGrid.ConnectBooks};";
            var booksDataTable = new MySqlConnectDataGrid().ExecuteQuery(mysqlCmd);
            DataGridBooksList.ItemsSource = booksDataTable.DefaultView;
        }

        private void BooksViewList_OnLoaded(object sender, RoutedEventArgs e)
        {
            var mysqlCmd = $"select * from {MySqlConnectDataGrid.ConnectBooks};";
            var booksDataTable = new MySqlConnectDataGrid().ExecuteQuery(mysqlCmd);
            DataGridBooksList.ItemsSource = booksDataTable.DefaultView;
        }
    }
}
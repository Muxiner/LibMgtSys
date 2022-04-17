using System.Windows;
using LibrariesManageSystem.Extensions;

namespace LibrariesManageSystem.SystemWindows.BooksManagement
{
    public partial class BooksView : Window
    {
        public BooksView()
        {
            InitializeComponent();
        }

        private void ButtonRefreshBooksInformation_OnClick(object sender, RoutedEventArgs e)
        {
            var mysqlCmd = $"select * from {MySqlConnectDataGrid.ConnectBooks};";
            var booksDataTable = new MySqlConnectDataGrid().ExecuteQuery(mysqlCmd);
            DataGridBooks.ItemsSource = booksDataTable.DefaultView;
        }

        private void BooksView_OnLoaded(object sender, RoutedEventArgs e)
        {
            var mysqlCmd = $"select * from {MySqlConnectDataGrid.ConnectBooks};";
            var booksDataTable = new MySqlConnectDataGrid().ExecuteQuery(mysqlCmd);
            DataGridBooks.ItemsSource = booksDataTable.DefaultView;
        }

        private void ButtonBooksViewBackTo_OnClick(object sender, RoutedEventArgs e)
            => Close();
    }
}
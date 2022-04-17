using System.Windows;
using LibrariesManageSystem.Extensions;
using LibrariesManageSystem.Users;

namespace LibrariesManageSystem.SystemWindows.AccountManage
{
    public partial class Record : Window
    {
        public Record()
        {
            InitializeComponent();
        }

        private void ButtonRecordBackTo_OnClick(object sender, RoutedEventArgs e)
            => Close();

            private void ButtonLoadRecords_OnClick(object sender, RoutedEventArgs e)
        {
            var mysqlCmd = $"select * from {MySqlConnectDataGrid.ConnectRecords};";
            var booksDataTable = new MySqlConnectDataGrid().ExecuteQuery(mysqlCmd);
            DataGridBooks.ItemsSource = booksDataTable.DefaultView;
        }
    }
}
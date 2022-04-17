using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using LibrariesManageSystem.Extensions;
using LibrariesManageSystem.Users;
using MySql.Data.MySqlClient;

namespace LibrariesManageSystem.SystemWindows
{
    public partial class BooksReturn : Window
    {
        public BooksReturn()
        {
            InitializeComponent();
        }

        private void ButtonReturnBookBackTo_OnClick(object sender, RoutedEventArgs e)
        {
            var system = new SystemMainWindow
            {
                WindowStartupLocation = WindowStartupLocation.Manual,
                Top = Top,
                Left = Left
            };
            system.Show();
            this?.Close();
        }

        private void ButtonLoadRecords_OnClick(object sender, RoutedEventArgs e)
        {
            var mysqlCmd = $"select * from {MySqlConnectDataGrid.ConnectRecords} where UserID = '{Managers.ManagerId}';";
            var booksDataTable = new MySqlConnectDataGrid().ExecuteQuery(mysqlCmd);
            DataGridBooks.ItemsSource = booksDataTable.DefaultView;
        }

        private async void ButtonReturnBook_OnClick(object sender, RoutedEventArgs e)
        {
            var returnBookNumber = TextBoxBooksReturn.Text.Trim();
            if (!string.IsNullOrWhiteSpace(returnBookNumber))
            {
                var mysqlCon = new MySqlConnection(MySqlConnectDataGrid.MySqlConnect);
                mysqlCon.Open();
                if (new MySqlCommand(
                    $"select RenewalStatus from {MySqlConnectDataGrid.ConnectRecords} where UserID = '{Managers.ManagerId}' and " +
                    $"BookNumber = '{returnBookNumber}' and ReturnDate is NULL;",
                    mysqlCon).ExecuteScalar().ToString() == "借出")
                {
                    var recordID = new MySqlCommand(
                        $"select RecordID from {MySqlConnectDataGrid.ConnectRecords} " +
                        $"where UserID = '{Managers.ManagerId}' and BookNumber = '{returnBookNumber}' and ReturnDate is NULL;",
                        mysqlCon).ExecuteScalar().ToString();
                    var returnDateTime = DateTime.Now.ToString("y-M-d");
                    new MySqlCommand($"update {MySqlConnectDataGrid.ConnectRecords} set ReturnDate = '{returnDateTime}'," +
                                     $"RenewalStatus = '已还' where RecordID = '{recordID}';", mysqlCon).ExecuteScalar();
                    new MySqlCommand(
                        $"update {MySqlConnectDataGrid.ConnectBooks} " + 
                        $"set Status = '在馆' " +
                        $"where BookNumber = '{returnBookNumber}';", mysqlCon).ExecuteScalar();
                    LabelBookDescription.Visibility = Visibility.Visible;
                    LabelBookDescription.Foreground = Brushes.LawnGreen;
                    LabelBookDescription.Content = "成功退还书籍";
                    await Task.Delay(2000);
                    LabelBookDescription.Visibility = Visibility.Hidden;
                }
                else
                {
                    LabelBookDescription.Visibility = Visibility.Visible;
                    LabelBookDescription.Foreground = Brushes.Red;
                    LabelBookDescription.Content = "输入了错误的书号或未借阅该书籍";
                    await Task.Delay(2000);
                    LabelBookDescription.Visibility = Visibility.Hidden;
                }

                await mysqlCon.CloseAsync();
            }
            else
            {
                LabelBookDescription.Visibility = Visibility.Visible;
                LabelBookDescription.Foreground = Brushes.Yellow;
                LabelBookDescription.Content = "请输入需要退还书籍的书号";
                await Task.Delay(2000);
                LabelBookDescription.Visibility = Visibility.Hidden;
            }
        }
    }
}
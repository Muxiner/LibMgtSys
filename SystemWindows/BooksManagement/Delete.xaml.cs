using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using LibrariesManageSystem.Extensions;
using MySql.Data.MySqlClient;

namespace LibrariesManageSystem.SystemWindows.BooksManagement
{
    public partial class Delete : Window
    {
        public Delete()
        {
            InitializeComponent();
        }

        private void ButtonLoadBooksInformation_OnClick(object sender, RoutedEventArgs e)
        {
            var mysqlCmd = $"select * from {MySqlConnectDataGrid.ConnectBooks};";
            var booksDataTable = new MySqlConnectDataGrid().ExecuteQuery(mysqlCmd);
            DataGridBooks.ItemsSource = booksDataTable.DefaultView;
        }
        
        private void TextBoxBookNumberDelete_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TextBoxBookNumberDelete.Text.Trim()))
            {
                var mysqlCon = new MySqlConnection(MySqlConnectDataGrid.MySqlConnect);
                mysqlCon.Open();
                var mysqlCmd =
                    $"select exists (select * from {MySqlConnectDataGrid.ConnectBooks} " +
                    $"where BookNumber = '{TextBoxBookNumberDelete.Text.Trim()}');";
                if (Convert.ToInt32(new MySqlCommand(mysqlCmd, mysqlCon).ExecuteScalar()) == 0)
                {
                    LabelDescription.Content = "输入了不存在的书号";
                    LabelDescription.Foreground = Brushes.Red;
                }
                else
                {
                    LabelDescription.Content = "";
                }
                mysqlCon.Close();
            }
            else
            {
                LabelDescription.Content = "请输入需要删除的书籍的书号";
                LabelDescription.Foreground = Brushes.Yellow;
            }
        }

        private void ButtonDeleteBackTo_OnClick(object sender, RoutedEventArgs e) => Close();
        

        private async void ButtonDelete_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var bookNumber = TextBoxBookNumberDelete.Text.Trim();
            var mysqlCon = new MySqlConnection(MySqlConnectDataGrid.MySqlConnect);
            mysqlCon.Open();
            if ( !string.IsNullOrWhiteSpace(bookNumber) && 
                 Convert.ToInt32(new MySqlCommand(
                     $"select exists (select * from {MySqlConnectDataGrid.ConnectBooks} where BookNumber = '{bookNumber}');", 
                     mysqlCon).ExecuteScalar()) != 0)
            {
                var deleteCmd = $"delete from {MySqlConnectDataGrid.ConnectBooks} where BookNumber = '{bookNumber}';";
                //var deleteCmd = $"delete from books where BookNumber = '{bookNumber}';";
                new MySqlCommand(deleteCmd, mysqlCon).ExecuteScalar();
                ButtonDelete.Foreground = Brushes.PaleVioletRed;
                ButtonDelete.Content = "删除成功";
            }
            else
            {
                ButtonDelete.Foreground = Brushes.OrangeRed;
                ButtonDelete.Content = "书号不存在";
            }

            await Task.Delay(2000);
            await mysqlCon.CloseAsync();
            ButtonDelete.Foreground = Brushes.Gray;
            ButtonDelete.Content = "双击确认删除";
        }
    }
}
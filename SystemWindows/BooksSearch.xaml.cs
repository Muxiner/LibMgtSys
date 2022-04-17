using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using LibrariesManageSystem.Extensions;
using MySql.Data.MySqlClient;

namespace LibrariesManageSystem.SystemWindow
{
    public partial class BooksSearch : Window
    {
        private string _bookNumber = "";
        private string _title = "";
        private string _category = "";
        private string _author = "";
        private string _description = "";
        private string _status = "";
        private int _borrowingNumber = 0;
        public BooksSearch()
        {
            InitializeComponent();
        }

        private void ButtonBackToMain_OnClick(object sender, RoutedEventArgs e)
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

        private async void ButtonSearch_OnClick(object sender, RoutedEventArgs e)
        {
            var searchString = TextBoxSearch.Text.Trim();
            var searchChoose = ComboBoxSearchCategory.Text.Trim() == "书号" ? "BookNumber" : "Title";
            if (!string.IsNullOrWhiteSpace(searchString)) 
            {
                var mysqlCon = new MySqlConnection(MySqlConnectDataGrid.MySqlConnect);
                mysqlCon.Open();
                if (Convert.ToInt32(new MySqlCommand(
                    $"select exists (select * from {MySqlConnectDataGrid.ConnectBooks} where {searchChoose} = '{searchString}');",
                    mysqlCon).ExecuteScalar()) != 0)
                {
                    var searchCmd = new MySqlCommand(
                        $"select * from {MySqlConnectDataGrid.ConnectBooks} where {searchChoose} = '{searchString}'",
                        mysqlCon).ExecuteReader();
                    while (searchCmd.Read())
                    {
                        _bookNumber = searchCmd["BookNumber"].ToString();
                        _title = searchCmd["Title"].ToString();
                        _category = searchCmd["Category"].ToString();
                        _author = searchCmd["Author"].ToString();
                        _description = searchCmd["Introduction"].ToString();
                        _borrowingNumber = Convert.ToInt32(searchCmd["BorrowingNumber"]);
                        _status = searchCmd["Status"].ToString();
                    }
                    LabelSearchTips.Visibility = Visibility.Visible;
                    LabelSearchTips.Foreground = Brushes.Green;
                    LabelSearchTips.Content = "查询成功";
                    await Task.Delay(1000);
                    LabelSearchTips.Visibility = Visibility.Hidden;
                    
                    LabelTitle.Content = _title;
                    LabelBookNumber.Content = _bookNumber;
                    LabelCategory.Content = _category;
                    LabelAuthor.Content = _author;
                    TextBlockDescription.Text = _description;
                    LabelBorrowingNumber.Content = _borrowingNumber;
                    LabelStatus.Content = _status;
                }
                else
                {
                    LabelSearchTips.Visibility = Visibility.Visible;
                    LabelSearchTips.Foreground = Brushes.OrangeRed;
                    LabelSearchTips.Content = "查询内容不存在，请重新输入";
                    await Task.Delay(2000);
                    LabelSearchTips.Visibility = Visibility.Hidden;
                }

                await mysqlCon.CloseAsync();
            }
            else
            {
                LabelSearchTips.Visibility = Visibility.Visible;
                LabelSearchTips.Foreground = Brushes.Yellow;
                LabelSearchTips.Content = "请输入内容";
                await Task.Delay(2000);
                LabelSearchTips.Visibility = Visibility.Hidden;
            }
        }
    }
}
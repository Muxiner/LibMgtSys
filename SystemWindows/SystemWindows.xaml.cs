using System.Windows;
using LibrariesManageSystem.Extensions;
using LibrariesManageSystem.SystemWindow;
using LibrariesManageSystem.SystemWindows;
using LibrariesManageSystem.SystemWindows.AccountManage;
using LibrariesManageSystem.SystemWindows.BooksManagement;
using LibrariesManageSystem.Users;
using MySql.Data.MySqlClient;

namespace LibrariesManageSystem
{
    public partial class SystemMainWindow : Window
    {
        public const string MysqlData = "data source = localhost;" +
                                        "port = 3306;" + 
                                        "user = root;" +
                                        "password = ;" +
                                        "database = booksmanagement;";

        public SystemMainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 退出系统
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonExitSystem_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(0);
        }

        private void ButtonBooksSearch_OnClick(object sender, RoutedEventArgs e)
        {
            var search = new BooksSearch
            {
                WindowStartupLocation = WindowStartupLocation.Manual,
                Left = Left,
                Top = Top
            };
            search.Show();
            this?.Close();
        }

        private void ButtonBooksBorrow_OnClick(object sender, RoutedEventArgs e)
        {
            var borrow = new BooksBorrow{
                WindowStartupLocation = WindowStartupLocation.Manual,
                Left = Left,
                Top = Top
            };
            borrow.Show();
            this?.Close();
        }

        private void ButtonBooksManagement_OnClick(object sender, RoutedEventArgs e)
        {
            var bookManagement = new Management{
                WindowStartupLocation = WindowStartupLocation.Manual,
                Left = Left,
                Top = Top
            };
            bookManagement.Show();
            this?.Close();
        }

        private void ButtonAccountManagement_OnClick(object sender, RoutedEventArgs e)
        {
            var accountManagement = new AccountManagement{
                WindowStartupLocation = WindowStartupLocation.Manual,
                Left = Left,
                Top = Top
            };
            accountManagement.Show();
            this?.Close();
        }

        private void ButtonBooksReturn_OnClick(object sender, RoutedEventArgs e)
        {
            var booksReturn = new BooksReturn{
                WindowStartupLocation = WindowStartupLocation.Manual,
                Left = Left,
                Top = Top
            };
            booksReturn.Show();
            this?.Close();
        }

        private void ButtonBooksViewList_OnClick(object sender, RoutedEventArgs e)
        {
            var booksView = new BooksViewList
            {
                WindowStartupLocation = WindowStartupLocation.Manual,
                Left = Left,
                Top = Top
            };
            booksView.Show();
            this?.Close();
        }

        private void SystemMainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            var mysqlCon = new MySqlConnection(MySqlConnectDataGrid.MySqlConnect);
            mysqlCon.Open();
            var userInfo = new MySqlCommand(
                $"select * from {MySqlConnectDataGrid.ConnectUsers} where UserName = '{Managers.ManagerName}';", 
                mysqlCon).ExecuteReader();
            while (userInfo.Read())
            {
                Managers.ManagerId = userInfo["ID"].ToString();
                Managers.ManagerStudentName = userInfo["StudentName"].ToString();
                Managers.ManagerStudentNumber = userInfo["StudentNumber"].ToString();
            }
            mysqlCon.Close();
        }
    }
}
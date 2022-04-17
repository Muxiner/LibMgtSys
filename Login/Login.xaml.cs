using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using LibrariesManageSystem.Extensions;
using LibrariesManageSystem.Users;
using MySql.Data.MySqlClient;

namespace LibrariesManageSystem.Login
{
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 返回开始界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonBackStartWindow_OnClick(object sender, RoutedEventArgs e)
        {
            var start = new StartWindow
            {
                // 设置新窗口位置跟随旧窗口
                WindowStartupLocation = WindowStartupLocation.Manual,
                Top = Top,
                Left = Left
            };
            start.Show();
            this?.Hide();
        }
        /// <summary>
        /// 登录操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ButtonManagerLogin_OnClick(object sender, RoutedEventArgs e)
        {
            /********* 连接数据库 *******/
            const string usersData = "data source = localhost;" +
                                     "port = 3306;" + 
                                     "user = root;" +
                                     "password = ;" +
                                     "database = booksmanagement;";
            var userName = TextBoxUserName.Text.Trim();
            var userPassword = BoxUserPassWord.Password.Trim();

            if (userName.Equals(""))
            {
                LabelUserPasswordDescription.Content = UserInformationErrorType.UserPasswordErrorType[0];
                LabelUserNameDescription.Content = UserInformationErrorType.UserNameErrorType[4];
                LabelUserNameDescription.Foreground = Brushes.Red;
                LabelUserNameDescription.Visibility = Visibility.Visible;
            }
            else
            {
                // 建立连接
                var  usersConnection = new MySqlConnection(usersData);
                try
                {
                    // 打开数据库
                    usersConnection.Open();
                    // var usersMySql = $"select exists(select * from users where BINARY UserName = '{userName}' and Password = '{userPassword}');";
                    var userNameMySqlCommand =
                        $"select exists(select * from users where BINARY UserName = '{userName}');";
                    var nameCmd = new MySqlCommand(userNameMySqlCommand, usersConnection);
                    var userNameExist = Convert.ToInt32(nameCmd.ExecuteScalar());
                    var userPasswordMySqlCommand =
                        $"select exists(select * from users where BINARY Password = '{userName + userPassword}');";
                    var passwordCmd = new MySqlCommand(userPasswordMySqlCommand, usersConnection);
                    var userPasswordExist = Convert.ToInt32(passwordCmd.ExecuteScalar());
                    //await usersConnection.CloseAsync();
                    if (userNameExist == 0)
                    {
                        LabelUserPasswordDescription.Content = UserInformationErrorType.UserPasswordErrorType[0];
                        LabelUserNameDescription.Content = UserInformationErrorType.UserNameErrorType[3];
                        LabelUserNameDescription.Foreground = Brushes.Red;
                        LabelUserNameDescription.Visibility = Visibility.Visible;
                    }
                    else if (userPasswordExist == 0)
                    {
                        if (userPassword.Equals(""))
                        {
                            LabelUserPasswordDescription.Foreground = Brushes.Red;
                            LabelUserPasswordDescription.Content =
                                UserInformationErrorType.UserPasswordErrorType[2];
                        }
                        else
                        {
                            LabelUserNameDescription.Content = UserInformationErrorType.UserNameErrorType[0];
                            LabelUserPasswordDescription.Content =
                                UserInformationErrorType.UserPasswordErrorType[3];
                            LabelUserPasswordDescription.Foreground = Brushes.Red;
                            LabelUserPasswordDescription.Visibility = Visibility.Visible;
                        }
                    }
                    else
                    {
                        LabelUserNameDescription.Content = UserInformationErrorType.UserNameErrorType[1];
                        LabelUserNameDescription.Foreground = Brushes.Green; 
                        LabelUserNameDescription.Visibility = Visibility.Visible;
                        LabelUserPasswordDescription.Content = 
                            UserInformationErrorType.UserPasswordErrorType[1];
                        LabelUserPasswordDescription.Foreground = Brushes.Green;
                        LabelUserPasswordDescription.Visibility = Visibility.Visible;
                        //await usersConnection.OpenAsync();
                        // 记录用户信息
                        var userInfo = new MySqlCommand(
                            $"select * from {MySqlConnectDataGrid.ConnectUsers} where UserName = '{userName}';",
                            usersConnection).ExecuteReader();
                        Managers.ManagerName = userName;
                        Managers.ManagerPassword = userPassword;
                        while (userInfo.Read())
                        {
                            Managers.ManagerId = userInfo["ID"].ToString();
                            Managers.ManagerStudentName = userInfo["StudentName"].ToString();
                            Managers.ManagerStudentNumber = userInfo["StudentNumber"].ToString();
                        }
                        var system = new SystemMainWindow
                        {
                            WindowStartupLocation = WindowStartupLocation.Manual, 
                            Left = Left, 
                            Top = Top
                        };

                        // 使用async/await 延时 1 秒
                        await Task.Delay(1000); 
                        await usersConnection.CloseAsync(); 
                        this?.Close(); 
                        system.Show();
                    }
                }
                catch (Exception ex)
                {
                    Debugger.Break();
                }
            }
        }
        
        /// <summary>
        /// 进入注册页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonRegister_OnClick(object sender, RoutedEventArgs e)
        {
            var register = new Register
            {
                WindowStartupLocation = WindowStartupLocation.Manual,
                Top = Top,
                Left = Left
            };
            register.Show();
            this?.Hide();
        }
    }
}
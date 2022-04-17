using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using LibrariesManageSystem.Extensions;
using LibrariesManageSystem.Users;
using MySql.Data.MySqlClient;

namespace LibrariesManageSystem.SystemWindows.AccountManage
{
    public partial class NewPassword : Window
    {
        private const string _password = "";

        public NewPassword()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private async void ButtonPasswordModify_OnClick(object sender, RoutedEventArgs e)
        {
            var mysqlCon = new MySqlConnection(MySqlConnectDataGrid.MySqlConnect);
            mysqlCon.Open();
            if (PasswordBoxNew.Password.Trim() == BoxConfirm.Password.Trim() && !string.IsNullOrWhiteSpace(PasswordBoxNew.Password.Trim()))
            {
                new MySqlCommand($"update {MySqlConnectDataGrid.ConnectUsers} set Password = '{Managers.ManagerName + _password}' where UserName = '{Managers.ManagerName}';", mysqlCon).ExecuteScalar();
                await mysqlCon.CloseAsync();
                ButtonPasswordModify.Foreground = Brushes.Green;
                ButtonPasswordModify.Content = "修改成功";
                await Task.Delay(1000);
                ButtonPasswordModify.Foreground = Brushes.Yellow;
                ButtonPasswordModify.Content = "4秒后自动退出登录";
                await Task.Delay(1000);
                ButtonPasswordModify.Content = "3秒后自动退出登录";
                await Task.Delay(1000);
                ButtonPasswordModify.Content = "2秒后自动退出登录";
                await Task.Delay(1000);
                ButtonPasswordModify.Content = "1秒后自动退出登录";
                await Task.Delay(1000);
                ButtonPasswordModify.Content = "0秒后自动退出登录";
                var login = new Login.Login
                {
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };
                login.Show();
                this?.Close();
            }
            await mysqlCon.CloseAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private async void PasswordBoxOld_OnLostFocus(object sender, RoutedEventArgs e)
        {
            var password = PasswordBoxOld.Password.Trim();
            if (Managers.ManagerPassword != password && !string.IsNullOrWhiteSpace(password))
            {
                LabelOldPassword.Visibility = Visibility.Visible;
                LabelOldPassword.Foreground = Brushes.Crimson;
                LabelOldPassword.Content = "旧密码错误";
                await Task.Delay(2000);
                LabelOldPassword.Visibility = Visibility.Hidden;
            }
            else if (string.IsNullOrWhiteSpace(password))
            {
                LabelOldPassword.Visibility = Visibility.Visible;
                LabelOldPassword.Foreground = Brushes.Yellow;
                LabelOldPassword.Content = "请输入旧密码";
                await Task.Delay(2000);
                LabelOldPassword.Visibility = Visibility.Hidden;
            }
            else if (Managers.ManagerPassword == password)
            {
                LabelOldPassword.Visibility = Visibility.Visible;
                LabelOldPassword.Foreground = Brushes.Green;
                LabelOldPassword.Content = "OK";
                await Task.Delay(2000);
                LabelOldPassword.Visibility = Visibility.Hidden;
            }
        }

        private void ButtonPasswordBackTo_OnClick(object sender, RoutedEventArgs e)
        {
            var accountM = new AccountManagement
            {
                WindowStartupLocation = WindowStartupLocation.Manual,
                Left = Left + (1000 - Width) / 2,
                Top = Top + (600 - Height) / 2
            };
            accountM.Show();
            this?.Close();
        }
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private async void BoxConfirm_OnLostFocus(object sender, RoutedEventArgs e)
        {
            var newPassword = PasswordBoxNew.Password.Trim();

            var isLegal = Regex.IsMatch(newPassword, "^(?![0-9]+$)(?![a-zA-Z]+$)[0-9A-Za-z]{6,20}$");
            if (string.IsNullOrWhiteSpace(newPassword))
            {
                LabelNewPassword.Visibility = Visibility.Visible;
                LabelNewPassword.Foreground = Brushes.Yellow;
                LabelNewPassword.Content = "请输入新密码";
                await Task.Delay(2000);
                LabelNewPassword.Visibility = Visibility.Hidden;
            }
            else if (newPassword == Managers.ManagerPassword)
            {
                LabelNewPassword.Visibility = Visibility.Visible;
                LabelNewPassword.Foreground = Brushes.Yellow;
                LabelNewPassword.Content = "与旧密码相同";
                await Task.Delay(2000);
                LabelNewPassword.Visibility = Visibility.Hidden;
            }
            else if (!isLegal)
            {
                LabelNewPassword.Visibility = Visibility.Visible;
                LabelNewPassword.Foreground = Brushes.Yellow;
                LabelNewPassword.Content = "输入了不合法的密码，请重新输入";
                await Task.Delay(2000);
                LabelNewPassword.Visibility = Visibility.Hidden;
            }
            else
            {
                LabelNewPassword.Visibility = Visibility.Visible;
                LabelNewPassword.Foreground = Brushes.Green;
                LabelNewPassword.Content = "OK";
                await Task.Delay(2000);
                LabelNewPassword.Visibility = Visibility.Hidden;
            }
        }

        private async void PasswordBoxNew_OnLostFocus(object sender, RoutedEventArgs e)
        {
            var confirmPassword = BoxConfirm.Password.Trim();

            if (string.IsNullOrWhiteSpace(confirmPassword))
            {
                LabelConfirm.Visibility = Visibility.Visible;
                LabelConfirm.Foreground = Brushes.Yellow;
                LabelConfirm.Content = "请再次输入新密码";
                await Task.Delay(2000);
                LabelConfirm.Visibility = Visibility.Hidden;
            }
            else if (confirmPassword == _password)
            {
                LabelConfirm.Visibility = Visibility.Visible;
                LabelConfirm.Foreground = Brushes.Green;
                LabelConfirm.Content = "OK";
                await Task.Delay(2000);
                LabelConfirm.Visibility = Visibility.Hidden;
            }
            else if (confirmPassword != _password)
            {
                LabelConfirm.Visibility = Visibility.Visible;
                LabelConfirm.Foreground = Brushes.Red;
                LabelConfirm.Content = "两次新密码不一致";
                await Task.Delay(2000);
                LabelConfirm.Visibility = Visibility.Hidden;
            }
        }

        private async void PasswordBoxNew_OnGotFocus(object sender, RoutedEventArgs e)
        {
            LabelNewPassword.Visibility = Visibility.Visible;
            LabelNewPassword.Foreground = Brushes.Black;
            LabelNewPassword.Content = "密码至少包含数字和英文，长度6-20";
            await Task.Delay(2000);
            LabelNewPassword.Visibility = Visibility.Hidden;
        }
    }
}
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using LibrariesManageSystem.Extensions;
using MySql.Data.MySqlClient;

namespace LibrariesManageSystem.Login
{
    public partial class Register : Window
    {
        private string _password = "";
        // 数据连接
        private static string _usersData = "data source = localhost;" +
                                         "port = 3306;" + 
                                         "user = root;" +
                                         "password = ;" +
                                         "database = booksmanagement;";
        
        private MySqlConnection _userConnection = new MySqlConnection(_usersData);
        public Register()
        {
            // 打开数据库
            _userConnection.Open();
            InitializeComponent();
        }

        private void ButtonRegisterBackTo_OnClick(object sender, RoutedEventArgs e)
        {
            var login = new LibrariesManageSystem.Login.Login
            {
                WindowStartupLocation = WindowStartupLocation.Manual,
                Left = Left,
                Top = Top
            };
            login.Show();
            _userConnection.Close();
            this?.Close();
        }
        
        private void TextBoxUserName_OnLostFocus(object sender, RoutedEventArgs e)
        {
            var userName = TextBoxUserName.Text.Trim();
            if (!userName.Equals(""))
            {
                var userNameMysqlCmd = $"select exists(select * from users where BINARY UserName = '{userName}');";
                var userNameExist = new MySqlCommand(userNameMysqlCmd, _userConnection);
                // 输入的用户名存在
                if (Convert.ToInt32(userNameExist.ExecuteScalar()) != 0)
                {
                    LabelUserNameDescription.Content = UserInformationErrorType.UserNameErrorType[5];
                    LabelUserNameDescription.Foreground = Brushes.Red;
                }
                // 收入的用户名不存在，即用户名可用
                else
                {
                    LabelUserNameDescription.Content = UserInformationErrorType.UserNameErrorType[1];
                    LabelUserNameDescription.Foreground = Brushes.Green; 
                }
            }
            else
            {
                // 输入框为空，提示请输入用户名
                LabelUserNameDescription.Foreground = Brushes.Yellow;
                LabelUserNameDescription.Content = UserInformationErrorType.UserNameErrorType[4];
            }
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ButtonUserRegister_OnClick(object sender, RoutedEventArgs e)
        {
            var userId = 100000;
            var userName = TextBoxUserName.Text.Trim();
            var userPassword = PasswordBoxUser.Password.Trim();
            var studentName = TextBoxStudentName.Text.Trim();
            var studentNumber = TextBoxUserStudentNumber.Text.Trim();
            var usersNumberMysqlCmd = "select count(ID) from users;";
            var userNumberCmd = new MySqlCommand(usersNumberMysqlCmd, _userConnection);
            var usersNumbers = Convert.ToInt32(userNumberCmd.ExecuteScalar());
            var userInformationMysqlCmd = 
                $"insert into users(ID, UserName, StudentName, Password, StudentNumber, IsManager) values ('{userId + usersNumbers}', '{userName}', '{studentName}', '{userName + userPassword}', '{studentNumber}', '1');";
            var userInformationCmd = new MySqlCommand(userInformationMysqlCmd, _userConnection);
            if (!userName.Equals("") && !userPassword.Equals("") && !studentName.Equals("") && !studentNumber.Equals(""))
            {
                // 数据插入数据库
                userInformationCmd.ExecuteScalar();
                var login = new LibrariesManageSystem.Login.Login
                {
                    WindowStartupLocation = WindowStartupLocation.Manual,
                    Left = Left,
                    Top = Top
                };
                ButtonUserRegister.Background = Brushes.PowderBlue;
                ButtonUserRegister.Content = "注册成功";
                await Task.Delay(1500);
                await _userConnection.CloseAsync();
                login.Show();
                this?.Close();
            }
            else
            {
                ButtonUserRegister.Foreground = Brushes.Red;
                ButtonUserRegister.Content = "注册失败";
            }
        }

        /// <summary>
        /// 学号输入框获取焦点时，显示信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxUserStudentNumber_OnGotFocus(object sender, RoutedEventArgs e)
        {
            LabelStudentNumberDescription.Foreground = Brushes.DodgerBlue;
            LabelStudentNumberDescription.Content = "请输入您的学号（11位数字）";
        }
        
        /// <summary>
        /// 学号输入框失去焦点时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void TextBoxUserStudentNumber_OnLostFocus(object sender, RoutedEventArgs e)
        {
            var userStudentNumber = TextBoxUserStudentNumber.Text.Trim();
            
            // 使用正则表达式匹配学号
            // 条件：
            // 1. 学号是共有 11位纯数字的数字串
            // 2. 前 2 位为年级，仅限 18 - 24
            // 3. 中间 6 位为任意数字的组合（还没有摸清学号各位数字的意思，所以笼统的定义为随机数组合）
            // 4. 后 3 位是学生在班级的排名（按照姓氏笔画排序，范围 001-100）
            if (!string.IsNullOrWhiteSpace(userStudentNumber) && Regex.IsMatch(userStudentNumber, "^(18|19|20|21|22|23|24)\\d{6}((0\\d[0-9])|100)$"))
            {
                var userStudentNumberMysqlCmd = $"select exists(select * from users where BINARY StudentNumber = '{userStudentNumber}');";
                var userStudentNumberExist = new MySqlCommand(userStudentNumberMysqlCmd, _userConnection);
                // 学号存在，不可用
                if (Convert.ToInt32(userStudentNumberExist.ExecuteScalar()) != 0)
                {
                    LabelStudentNumberDescription.Foreground = Brushes.Red;
                    LabelStudentNumberDescription.Content = "该学号已被注册，请重新输入";
                }
                // 学号不存在，可用
                else
                {
                    LabelStudentNumberDescription.Foreground = Brushes.Green;
                    LabelStudentNumberDescription.Content = "OK";
                }
            }
            // 输入框为空
            else if (userStudentNumber.Equals(""))
            {
                LabelStudentNumberDescription.Foreground = Brushes.Yellow;
                LabelStudentNumberDescription.Content = "请输入您的学号";
            }
            // 无法匹配正则表达式
            else if (!Regex.IsMatch(userStudentNumber, "^(18|19|20|21|22|23|24)\\d{6}((0\\d[0-9])|100)$"))
            {
                LabelStudentNumberDescription.Foreground = Brushes.Red;
                LabelStudentNumberDescription.Content = "输入了错误的学号，请重新输入";
            }
            
        }

        /// <summary>
        /// 姓名输入框获取焦点时，显示信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void TextBoxStudentName_OnGotFocus(object sender, RoutedEventArgs e)
        {
            LabelStudentNameDescription.Foreground = Brushes.DodgerBlue;
            LabelStudentNameDescription.Content = "请输入您的姓名（仅限2-4个汉字）";
        }

        /// <summary>
        /// 姓名输入框失去焦点时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void TextBoxStudentName_OnLostFocus(object sender, RoutedEventArgs e)
        {
            var studentName = TextBoxStudentName.Text.Trim();
            
            // 使用正则表达式匹配姓名
            // 条件：
            // 仅限 2 - 4 个汉字
            if (!studentName.Equals("") && Regex.IsMatch(studentName, "^[\u4e00-\u9fa5]{2,4}$"))
            {
                var studentNameMysqlCmd = $"select exists(select * from users where BINARY StudentName = '{studentName}');";
                var studentNameExist = new MySqlCommand(studentNameMysqlCmd, _userConnection);
                // 学号存在，不可用
                if (Convert.ToInt32(studentNameExist.ExecuteScalar()) != 0)
                {
                    LabelStudentNameDescription.Foreground = Brushes.Red;
                    LabelStudentNameDescription.Content = "该姓名已被注册，请重新输入";
                }
                // 学号不存在，可用
                else
                {
                    LabelStudentNameDescription.Foreground = Brushes.Green;
                    LabelStudentNameDescription.Content = "OK";
                }
                
            }
            // 输入框为空
            else if (studentName.Equals(""))
            {
                LabelStudentNameDescription.Foreground = Brushes.Yellow;
                LabelStudentNameDescription.Content = "请输入您的姓名（仅限2-4个汉字）";
            }
            // 无法匹配正则表达式
            else if (!Regex.IsMatch(studentName, "^[\u4e00-\u9fa5]{2,4}$"))
            {
                LabelStudentNameDescription.Foreground = Brushes.Red;
                LabelStudentNameDescription.Content = "输入了不合法的姓名，请重新输入";
            }
            
        }

        /// <summary>
        /// 密码框获取焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void PasswordBoxUser_OnGotFocus(object sender, RoutedEventArgs e)
        {
            LabelPasswordDescription.Foreground = Brushes.DodgerBlue;
            LabelPasswordDescription.Content = "密码至少包含数字和英文，长度6-20";
        }

        /// <summary>
        /// 密码框失去焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void PasswordBoxUser_OnLostFocus(object sender, RoutedEventArgs e)
        {
            var password = PasswordBoxUser.Password.Trim();
            
            // 使用正则表达式匹配密码
            if (!password.Equals("") && Regex.IsMatch(password, "^(?![0-9]+$)(?![a-zA-Z]+$)[0-9A-Za-z]{6,20}$"))
            {
                LabelPasswordDescription.Foreground = Brushes.Green;
                _password = password;
                LabelPasswordDescription.Content = "OK";
            }
            // 输入框为空
            else if (password.Equals(""))
            {
                LabelPasswordDescription.Foreground = Brushes.Yellow;
                LabelPasswordDescription.Content = "请输入密码";
            }
            // 无法匹配正则表达式
            else if (!Regex.IsMatch(password, "^(?![0-9]+$)(?![a-zA-Z]+$)[0-9A-Za-z]{6,20}$"))
            {
                LabelStudentNameDescription.Foreground = Brushes.Red;
                LabelStudentNameDescription.Content = "输入了不合法的密码，请重新输入";
            }
        }

        /// <summary>
        /// 再次输入密码框获取焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void PasswordBoxConfirm_OnGotFocus(object sender, RoutedEventArgs e)
        {
            LabelConfirmDescription.Foreground = Brushes.DodgerBlue;
            LabelConfirmDescription.Content = "请再次输入密码";
        }

        /// <summary>
        /// 再次输入密码框失去焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void PasswordBoxConfirm_OnLostFocus(object sender, RoutedEventArgs e)
        {
            var confirmPassword = PasswordBoxConfirm.Password.Trim();
            
            if (!confirmPassword.Equals("") && confirmPassword.Equals(_password))
            {
                LabelConfirmDescription.Foreground = Brushes.Green;
                LabelConfirmDescription.Content = "OK";
            }
            // 输入框为空
            else if (confirmPassword.Equals(""))
            {
                LabelConfirmDescription.Foreground = Brushes.Yellow;
                LabelConfirmDescription.Content = "请输入密码";
            }
            // 无法匹配正则表达式
            else if (!confirmPassword.Equals(_password))
            {
                LabelConfirmDescription.Foreground = Brushes.Red;
                LabelConfirmDescription.Content = "两次密码不一致，请重新输入";
            }
        }
    }
}
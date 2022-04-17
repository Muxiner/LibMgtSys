using System.Windows;
using LibrariesManageSystem.Login;

namespace LibrariesManageSystem.SystemWindows.AccountManage
{
    public partial class AccountManagement : Window
    {
        public AccountManagement()
        {
            InitializeComponent();
        }

        private void ButtonAccountBackTo_OnClick(object sender, RoutedEventArgs e)
        {
            var system = new SystemMainWindow()
            {
                WindowStartupLocation = WindowStartupLocation.Manual,
                Left = Left,
                Top = Top
            };
            system.Show();
            this?.Close();
        }

        private void ButtonSignOut_OnClick(object sender, RoutedEventArgs e)
        {
            var start = new StartWindow()
            {
                WindowStartupLocation = WindowStartupLocation.Manual,
                Left = Left + (Width - 700) / 2,
                Top = Top + (Height - 400) / 2
            };
            start.Show();
            this?.Close();
        }

        private void ButtonRecords_OnClick(object sender, RoutedEventArgs e)
        {
            var records = new Record
            {
                WindowStartupLocation = WindowStartupLocation.Manual,
                Left = Left + (Width - 800) / 2,
                Top = Top + (Height - 450) / 2
            };
            records.Show();
        }

        private void ButtonModifyPassword_OnClick(object sender, RoutedEventArgs e)
        {
            var password = new NewPassword
            {
                WindowStartupLocation = WindowStartupLocation.Manual,
                Left = Left + (Width - 800) / 2,
                Top = Top + (Height - 450) / 2
            };
            password.Show();
            this?.Close();
        }

        private void ButtonAccountInfo_OnClick(object sender, RoutedEventArgs e)
        {
            var account = new AccountInformation
            {
                WindowStartupLocation = WindowStartupLocation.Manual,
                Left = Left + (Width - 800) / 2,
                Top = Top + (Height - 450) / 2
            };
            account.Show();
        }
    }
}
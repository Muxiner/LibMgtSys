using System.Windows;
using LibrariesManageSystem.Users;

namespace LibrariesManageSystem.SystemWindows.AccountManage
{
    public partial class AccountInformation : Window
    {
        public AccountInformation()
        {
            InitializeComponent();
        }

        private void ButtonInfoBackTo_OnClick(object sender, RoutedEventArgs e) => Close();

        private void AccountInformation_OnLoaded(object sender, RoutedEventArgs e)
        {
            LabelUserName.Content = Managers.ManagerName;
            LabelStudentNumber.Content = Managers.ManagerStudentNumber;
            LabelName.Content = Managers.ManagerStudentName;
            LabelUserID.Content = Managers.ManagerId;
        }
    }
}
using System.Windows;
using LibrariesManageSystem.Extensions;

namespace LibrariesManageSystem.Login
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class StartWindow
    {
        public StartWindow()
        {
            if (GlobalVariable.WindowsFirstOpen)
            {
                // 屏幕中间打开窗口
                WindowStartupLocation = WindowStartupLocation.CenterScreen;
                GlobalVariable.WindowsFirstOpen = false;
            }
            InitializeComponent();
        }

        private void ButtonManagerMode_OnClick(object sender, RoutedEventArgs e)
        {
            var login = new LibrariesManageSystem.Login.Login
            {
                // login 窗口位置跟随父窗口
                WindowStartupLocation = WindowStartupLocation.Manual,
                Left = Left,
                Top = Top
            };
            this?.Hide();
            login.Show();
        }

        private void ButtonAboutSystem_OnClick(object sender, RoutedEventArgs e) => this?.Close();
    }
}
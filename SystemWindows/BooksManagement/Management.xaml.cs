using System.Windows;

namespace LibrariesManageSystem.SystemWindows.BooksManagement
{
    public partial class Management : Window
    {
        public Management()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonManagementBackTo_OnClick(object sender, RoutedEventArgs e)
        {
            var window = new SystemMainWindow
            {
                WindowStartupLocation = WindowStartupLocation.Manual,
                Left = Left,
                Top = Top
            };
            window.Show();
            this?.Close();
        }

        /// <summary>
        /// 添加图书类别
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void ButtonAddCategory_OnClick(object sender, RoutedEventArgs e)
        {
            var addCategory = new CategoryAddiction
            {
                WindowStartupLocation = WindowStartupLocation.Manual,
                Left = Left + (Width - 800)/2,
                Top = Top + (Height - 450) / 2
            };
            addCategory.Show();
        }
        
        /// <summary>
        /// 修改图书类别
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void ButtonCategoryModification_OnClick(object sender, RoutedEventArgs e)
        {
            var modifyCategory = new CategoryModification
            {
                WindowStartupLocation = WindowStartupLocation.Manual,
                Left = Left + (Width - 800)/2,
                Top = Top + (Height - 450) / 2
            };
            modifyCategory.Show();
        }

        /// <summary>
        /// 添加图书信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonAddBooks_OnClick(object sender, RoutedEventArgs e)
        {
            var addBooks = new BooksAddiction
            {
                WindowStartupLocation = WindowStartupLocation.Manual,
                Left = Left + (Width - 800)/2,
                Top = Top + (Height - 450) / 2
            };
            addBooks.Show();
        }

        /// <summary>
        /// 修改图书信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonModifyBooks_OnClick(object sender, RoutedEventArgs e)
        {
            var booksModification = new BooksModification
            {
                WindowStartupLocation = WindowStartupLocation.Manual,
                Left = Left + (Width - 800)/2,
                Top = Top + (Height - 450) / 2
            };
            booksModification.Show();
        }

        /// <summary>
        /// 删除图书
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonDeleteBooks_OnClick(object sender, RoutedEventArgs e)
        {
            var deleteWindow = new Delete
            {
                WindowStartupLocation = WindowStartupLocation.Manual,
                Left = Left + (Width - 800) / 2,
                Top = Top + (Height - 450) / 2
            };
            deleteWindow.Show();
        }

        private void ButtonBooksView_OnClick(object sender, RoutedEventArgs e)
        {
            var booksView = new BooksView
            {
                WindowStartupLocation = WindowStartupLocation.Manual,
                Left = Left + (Width - 800) / 2,
                Top = Top + (Height - 450) / 2
            };
            booksView.Show();
        }
    }
}
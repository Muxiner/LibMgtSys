using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using MySql.Data.MySqlClient;

namespace LibrariesManageSystem.SystemWindows.BooksManagement
{
    public partial class BooksAddiction : Window
    {
        private readonly MySqlConnection _data = new MySqlConnection(SystemMainWindow.MysqlData);
        public BooksAddiction()
        {
            InitializeComponent();
        }

        private void ButtonBookAddBackTo_OnClick(object sender, RoutedEventArgs e) => Close();

        private void TextBoxTitle_OnLostFocus(object sender, RoutedEventArgs e)
        {
            _data.Open();
            var title = TextBoxTitle.Text.Trim();
            if (title.Equals(""))
            {
                LabelTitleDescription.Foreground = Brushes.Yellow;
                LabelTitleDescription.Content = "请输入书名";
            }
            else
            {
                if (Convert.ToInt32(new MySqlCommand($"select exists(select * from books where Title = '{title}');",_data).ExecuteScalar()) != 0)
                {
                    LabelTitleDescription.Foreground = Brushes.Red;
                    LabelTitleDescription.Content = "书籍已存在";
                }
            }
            _data.Close();
        }

        private async void ButtonAddBookInformation_OnClick(object sender, RoutedEventArgs e)
        {
            var title = TextBoxTitle.Text.Trim();
            var category = ComboBoxCategory.SelectionBoxItem.ToString();
            var author = TextBoxAuthor.Text.Trim();
            var description = !string.IsNullOrWhiteSpace(TextBoxDescription.Text.Trim()) ? TextBoxDescription.Text.Trim() : "无";
            const int borrowTimes = 0;
            if (!title.Equals("") && !category.Equals("") && !author.Equals(""))
            {
                _data.Open();
                var bookNumber = 10000000 + 1 + Convert.ToInt32(new MySqlCommand("select count(Title) from books;", _data).ExecuteScalar());
                new MySqlCommand("insert into books (BookNumber, Category, Title, Author, Introduction, BorrowingNumber, status) " +
                                 "values " +
                                 $"('{bookNumber}','{category}','{title}','{author}','{description}','{borrowTimes}','在馆');", _data).ExecuteScalar();
                ButtonAddBookInformation.Foreground = Brushes.MediumSeaGreen;
                ButtonAddBookInformation.Content = "添加成功";
                await Task.Delay(1500);
                await _data.CloseAsync();
                ButtonAddBookInformation.Foreground = Brushes.Gray;
                ButtonAddBookInformation.Content = "添加";
                TextBoxAuthor.Text = "";
                TextBoxDescription.Text = "";
                TextBoxTitle.Text = "";
            }
        }

        private void BooksAddiction_OnLoaded(object sender, RoutedEventArgs e)
        {   
            ComboBoxCategory.Items.Clear();
            _data.Open();
            var booksCategory = "select * from category;";
            var categoryCmd = new MySqlCommand(booksCategory, _data);
            var category = categoryCmd.ExecuteReader();
            while (category.Read())
            {
                ComboBoxCategory.Items.Add(category["CateGoryName"]);
            }
            _data.Close();
        }
    }
}
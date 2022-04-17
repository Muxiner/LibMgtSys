using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using MySql.Data.MySqlClient;

namespace LibrariesManageSystem.SystemWindows.BooksManagement
{
    public partial class BooksModification : Window
    {
        private readonly MySqlConnection _data = new MySqlConnection(SystemMainWindow.MysqlData);
        private string _title = "";
        private string _description = "";
        private string _category = "";
        private string _author = "";
        private bool _updated;
        public BooksModification()
        {
            InitializeComponent();
            _data.Open();
            var categoryCmd = new MySqlCommand("select CategoryName from category;", _data).ExecuteReader();
            while (categoryCmd.Read())
            {
                ComboBoxCategoryModify.Items.Add(categoryCmd["CategoryName"]);
            }
            _data.Close();
        }

        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonBookModify_OnClick(object sender, RoutedEventArgs e) => Close();

        /// <summary>
        /// 检查书名是否重复
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void TextBoxTitleModify_OnLostFocus(object sender, RoutedEventArgs e)
        {
            
            if (CheckBoxTitleModify.IsChecked != null && CheckBoxTitleModify.IsChecked.Value && !string.IsNullOrWhiteSpace(TextBoxTitleModify.Text.Trim()))
            {
                _data.Open();
                if (Convert.ToInt32(new MySqlCommand($"select exists(select * from books where Title = '{TextBoxTitleModify.Text.Trim()}');", _data).ExecuteScalar()) != 0)
                {
                    CheckBoxTitleModify.Foreground = Brushes.Red;
                    CheckBoxTitleModify.ToolTip = "书名已存在";
                    await Task.Delay(5000);
                    CheckBoxTitleModify.Foreground = Brushes.White;
                    CheckBoxTitleModify.ToolTip = "勾选表示需要修改";
                }
                await _data.CloseAsync();
            }
        }

        /// <summary>
        /// 检索按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private async void ButtonSearchBook_OnClick(object sender, RoutedEventArgs e)
        {
            var searchText = TextBoxCategory.Text.Trim();
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                _data.Open();
                var searchCategory = ComboBoxChoose.Text.Trim() == "书号" ? "BookNumber" : "Title";
                var searchCategoryValue = TextBoxCategory.Text.Trim();
                var existMysqlCmd =
                    $"select exists (select * from books where {searchCategory} = '{searchCategoryValue}');";
                if (Convert.ToInt32(new MySqlCommand(existMysqlCmd, _data).ExecuteScalar()) == 1)
                {
                    var searchBookMysqlCmd = $"select * from books where {searchCategory} = '{searchCategoryValue}';";
                    var searchCmd = new MySqlCommand(searchBookMysqlCmd, _data);
                    var booksInformation = searchCmd.ExecuteReader();
                    while (booksInformation.Read())
                    {
                        _title = booksInformation["Title"].ToString();
                        _author = booksInformation["Author"].ToString();
                        _description = booksInformation["Introduction"].ToString();
                        _category = booksInformation["Category"].ToString();
                    }
                    TextBoxTitleModify.Text = _title;
                    TextBoxAuthorModify.Text = _author;
                    TextBoxDescriptionModify.Text = _description;
                    ComboBoxCategoryModify.Text = _category;
                }
                else
                {
                    ButtonSearchBook.Content = "输入内容不存在";
                    ButtonSearchBook.Foreground = Brushes.Red;
                    await Task.Delay(1500);
                    ButtonSearchBook.Content = "检索";
                    ButtonSearchBook.Foreground = Brushes.Gray;
                    
                }
                await _data.CloseAsync();
            }
            else
            {
                ButtonSearchBook.Content = "请输入查询内容";
                ButtonSearchBook.Foreground = Brushes.Red;
                await Task.Delay(1500);
                ButtonSearchBook.Content = "检索";
                ButtonSearchBook.Foreground = Brushes.Gray;
            }
        }

        // 
        private void CheckBoxTitleModify_OnChecked(object sender, RoutedEventArgs e)
            => TextBoxTitleModify.IsEnabled = true;
        private void CheckBoxTitleModify_OnUnchecked(object sender, RoutedEventArgs e)
        {
            TextBoxTitleModify.IsEnabled = false;
            TextBoxTitleModify.Text = _title;
        }
        private void CheckBoxCategoryModify_OnChecked(object sender, RoutedEventArgs e)
            => ComboBoxCategoryModify.IsEnabled = CheckBoxCategoryModify.IsChecked.Value;
        private void CheckBoxCategoryModify_OnUnchecked(object sender, RoutedEventArgs e)
        {
            Debug.Assert(CheckBoxCategoryModify.IsChecked != null, "CheckBoxCategoryModify.IsChecked != null");
            ComboBoxCategoryModify.IsEnabled = CheckBoxCategoryModify.IsChecked.Value;
            ComboBoxCategoryModify.Text = _category;
        }
        private void CheckBoxAuthorModify_OnChecked(object sender, RoutedEventArgs e)
            => TextBoxAuthorModify.IsEnabled = CheckBoxAuthorModify.IsChecked.Value;
        private void CheckBoxAuthorModify_OnUnchecked(object sender, RoutedEventArgs e)
        {
            Debug.Assert(CheckBoxAuthorModify.IsChecked != null, "CheckBoxAuthorModify.IsChecked != null");
            TextBoxAuthorModify.IsEnabled = CheckBoxAuthorModify.IsChecked.Value;
            TextBoxAuthorModify.Text = _author;
        }
        private void CheckBoxDescriptionModify_OnChecked(object sender, RoutedEventArgs e)
            => TextBoxDescriptionModify.IsEnabled = CheckBoxDescriptionModify.IsChecked.Value;
        private void CheckBoxDescriptionModify_OnUnchecked(object sender, RoutedEventArgs e) 
        {
            Debug.Assert(CheckBoxDescriptionModify.IsChecked != null, "CheckBoxDescriptionModify.IsChecked != null");
            TextBoxDescriptionModify.IsEnabled = CheckBoxDescriptionModify.IsChecked.Value;
            TextBoxDescriptionModify.Text = _description;
        }

        /// <summary>
        /// 确定修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ButtonModifyBook_OnClick(object sender, RoutedEventArgs e)
        {
            _data.Open();
            var search = ComboBoxChoose.Text.Trim() == "书号" ? "BookNumber" : "Title";
            var bookNumber = new MySqlCommand($"select BookNumber from books where {search} = '{TextBoxCategory.Text.Trim()}';", _data).ExecuteScalar().ToString();
            if (CheckBoxTitleModify.IsChecked != null && CheckBoxTitleModify.IsChecked.Value)
            {
                new MySqlCommand($"UPDATE books SET Title = '{TextBoxTitleModify.Text.Trim()}' where BookNumber = '{bookNumber}';", _data).ExecuteScalar();
                _updated = true;
            }

            if (CheckBoxAuthorModify.IsChecked != null && CheckBoxAuthorModify.IsChecked.Value)
            {
                new MySqlCommand($"UPDATE books SET Author = '{TextBoxAuthorModify.Text.Trim()}' where BookNumber = '{bookNumber}';", _data).ExecuteScalar();
                _updated = true;
            }

            if (CheckBoxCategoryModify.IsChecked != null && CheckBoxCategoryModify.IsChecked.Value)
            {
                new MySqlCommand($"UPDATE books SET Category = '{ComboBoxCategoryModify.Text.Trim()}' where BookNumber = '{bookNumber}';", _data).ExecuteScalar();
                _updated = true;
            }

            if (CheckBoxDescriptionModify.IsChecked != null && CheckBoxDescriptionModify.IsChecked.Value)
            {
                new MySqlCommand($"UPDATE books SET Introduction = '{TextBoxDescriptionModify.Text.Trim()}' where BookNumber = '{bookNumber}';", _data).ExecuteScalar();
                _updated = true;
            }

            if (_updated)
            {
                ButtonModifyBook.Content = "修改成功";
                ButtonModifyBook.Foreground = Brushes.MediumSeaGreen;
                await Task.Delay(1500);
                ButtonModifyBook.Content = "修改图书信息";
                ButtonModifyBook.Foreground = Brushes.Gray;
                _updated = false;
            }
            else
            {
                ButtonModifyBook.Content = "信息不作更新";
                ButtonModifyBook.Foreground = Brushes.Yellow;
                await Task.Delay(1500);
                ButtonModifyBook.Content = "修改图书信息";
                ButtonModifyBook.Foreground = Brushes.Gray;
            }
            await _data.CloseAsync();
        }
    }
}
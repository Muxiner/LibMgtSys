using System;
using System.Windows;
using System.Windows.Media;
using MySql.Data.MySqlClient;

namespace LibrariesManageSystem.SystemWindows.BooksManagement
{
    public partial class CategoryModification : Window
    {
        private MySqlConnection _data = new MySqlConnection(SystemMainWindow.MysqlData);

        public CategoryModification()
        {
            InitializeComponent();
        }

        private void ButtonModifyBackTo_OnClick(object sender, RoutedEventArgs e) => this?.Close();

        private void ButtonLoadCategory_OnClick(object sender, RoutedEventArgs e)
        {
            ListViewCategory.Items.Clear();
            ComboBoxChoseCategory.Items.Clear();
            _data.Open();
            var booksCategory = "select * from category;";
            var categoryCmd = new MySqlCommand(booksCategory, _data);
            var category = categoryCmd.ExecuteReader();
            while (category.Read())
            {
                ListViewCategory.Items.Add(category["CateGoryName"]);
                ComboBoxChoseCategory.Items.Add(category["CateGoryName"]);
            }
            _data.Close();
        }

        private void ButtonModify_OnClick(object sender, RoutedEventArgs e)
        {
            var odlCategory = ComboBoxChoseCategory.SelectionBoxItem.ToString();
            var newCategory = TextBoxCategory.Text.Trim();
            var id = 0;
            
            if (newCategory.Equals(""))
            {
                LabelCategoryDescription.Foreground = Brushes.Red;
                LabelCategoryDescription.Content = "请输入类别";
            }
            else if (!newCategory.Equals(""))
            {
                _data.Open();
                
                var categoryUpdate = $"select exists(select * from category where BINARY CategoryName = '{newCategory}');";
                var categoryUpdateCmd = new MySqlCommand(categoryUpdate, _data);
                if (Convert.ToInt32(categoryUpdateCmd.ExecuteScalar()) == 0)
                {
                    id = Convert.ToInt32(new MySqlCommand(
                        $"select ID from category where CategoryName = '{odlCategory}'", _data).ExecuteScalar());
                    var insertCategoryMysqlCmd =
                        $"update category set CategoryName = '{newCategory}' where ID = '{id}'";
                    var insertCmd = new MySqlCommand(insertCategoryMysqlCmd, _data);
                    // 插入数据库
                    insertCmd.ExecuteScalar();
                    LabelCategoryDescription.Foreground = Brushes.MediumSpringGreen;
                    LabelCategoryDescription.Content = "类别已修改";
                    _data.Close();
                }
                else
                {
                    LabelCategoryDescription.Foreground = Brushes.Red;
                    LabelCategoryDescription.Content = "类别已存在";
                    _data.Close();
                }
            }
        }
    }
}
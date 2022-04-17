using System;
using System.Windows;
using System.Windows.Media;
using MySql.Data.MySqlClient;

namespace LibrariesManageSystem.SystemWindows.BooksManagement
{
    public partial class CategoryAddiction : Window
    {
        private MySqlConnection _data = new MySqlConnection(SystemMainWindow.MysqlData);
        public CategoryAddiction()
        {
            InitializeComponent();
            
            
        }

        private void ButtonAddCategoryBackTo_OnClick(object sender, RoutedEventArgs e)
        {
            // 关闭连接？
            this?.Close();
        }


        private void ButtonAddCategory_OnClick(object sender, RoutedEventArgs e)
        {
            var category = TextBoxCategory.Text.Trim();
            var id = 1;
            if (category.Equals(""))
            {
                LabelCategoryDescription.Foreground = Brushes.Red;
                LabelCategoryDescription.Content = "请输入类别";
            }
            else if (!category.Equals(""))
            {
                _data.Open();

                var categoryNumber = "select count(ID) from category;";
                id += Convert.ToInt32(new MySqlCommand(categoryNumber, _data).ExecuteScalar());
                var categoryExist = $"select exists(select * from category where BINARY CategoryName = '{category}');";
                var categoryExistCmd = new MySqlCommand(categoryExist, _data);
                if (Convert.ToInt32(categoryExistCmd.ExecuteScalar()) == 0)
                {
                    var insertCategoryMysqlCmd =
                        $"insert into category (ID, CategoryName) values ('{id}', '{category}');";
                    var insertCmd = new MySqlCommand(insertCategoryMysqlCmd, _data);
                    // 插入数据库
                    insertCmd.ExecuteScalar();
                    LabelCategoryDescription.Foreground = Brushes.MediumSpringGreen;
                    LabelCategoryDescription.Content = "类别已添加";
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

        private void TextBoxCategory_OnGotFocus(object sender, RoutedEventArgs e)
            => LabelCategoryDescription.Content = "";

        private void ButtonLoadCategory_OnClick(object sender, RoutedEventArgs e)
        {
            ListViewCategory.Items.Clear();
            _data.Open();
            var booksCategory = "select * from category;";
            var categoryCmd = new MySqlCommand(booksCategory, _data);
            var category = categoryCmd.ExecuteReader();
            while (category.Read())
            {
                ListViewCategory.Items.Add(category["CateGoryName"]);
            }
            _data.Close();
        }
    }
}
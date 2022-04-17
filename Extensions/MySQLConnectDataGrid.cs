using System.Data;
using MySql.Data.MySqlClient;

namespace LibrariesManageSystem.Extensions
{
    public class MySqlConnectDataGrid
    {
        public const string MySqlConnect = "data source = localhost;" +
                                        "port = 3306;" + 
                                        "user = root;" +
                                        "password = ;" +
                                        "database = booksmanagement;";

        public const string ConnectBooks = "books";
        public const string ConnectUsers = "users";
        public const string ConnectCategory = "category";
        public const string ConnectRecords = "records";

        public DataTable ExecuteQuery(string mysqlConnectCmd)
        {
            var mysqlCon = new MySqlConnection(MySqlConnect);
            mysqlCon.Open();
            var mysqlCmd = new MySqlCommand(mysqlConnectCmd, mysqlCon);
            mysqlCmd.CommandType = CommandType.Text;
            var mysqlDataTable = new DataTable();
            new MySqlDataAdapter(mysqlCmd).Fill(mysqlDataTable);
            mysqlCon.Close();
            return mysqlDataTable;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoService.Models;

namespace AutoService.Tools
{

    class UserDataStorage
    {
        private static SqlConnection _cn;

        internal UserDataStorage(string filepath)
        {
            _cn = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = D:\Study\3 year\architectura\AutoService\AutoService\DataBase\User.mdf; Integrated Security = True");
            if (_cn.State != ConnectionState.Open) _cn.Open();
            //createUserTable();
            //AddUser(new User("superUser", "cisco", "kurl@gmail.com", "Vasyl"));
            //User us = GetUserByLogin("superUser");
        }

        private void createUserTable()
        {
           // if (!isTableExists("user"))
            {
                string create_table_query = "CREATE TABLE user (" + 
                                     "user_id AUTOINCREMENT NOT NULL," + 
                                     "user_name VARCHAR(15) NOT NULL," +
                                     "password VARCHAR(16) NOT NULL," +
                                     "emal VARCHAR(20) NOT NULL," +
                                     "PRIMARY KEY (user_id));";
                Execute_SQL(create_table_query);
            }
            //AddUser(new User("superUser","cisco","kurl@gmail.com", "Vasyl"));
            //GetUserByLogin("pikos777");
        }

        void AddUser(User user)
        {
            string quer = "INSERT INTO [Table] ([user_name], [email], [password])"+
            " VALUES ('" + user.Login +  "', '" + user.Email + "', '" + user.Password + "');";
            Execute_SQL(quer);
        }

        internal void closeConnection() 
        { 
            if (_cn.State != ConnectionState.Closed) _cn.Close(); 
        }

        internal bool UserExists(string login)
        {
            string quer = "SELECT * FROM [Table] WHERE [user_name]='" + login + "';";
            SqlCommand cmd_Command = new SqlCommand(quer, _cn);
            SqlDataReader reader = cmd_Command.ExecuteReader();
            bool res = reader.Read();
            reader.Close();
            return res;
        }

        public static void Execute_SQL(string SQL_Text)
        {
            SqlCommand cmd_Command = new SqlCommand(SQL_Text, _cn);
            cmd_Command.ExecuteNonQuery();
        }

        internal User GetUserByLogin(string login)
        {
            string quer = "SELECT * FROM [Table] WHERE [user_name]='" + login + "';";
            SqlCommand cmd_Command = new SqlCommand(quer, _cn);
            SqlDataReader reader = cmd_Command.ExecuteReader();
            reader.Read();
            object ob1 =  reader.GetValue(0);
            object ob2 = reader.GetValue(1);
            object ob3 = reader.GetValue(2);
            object ob4 = reader.GetValue(3);
            User res = new User((string)reader.GetValue(1), (string)reader.GetValue(3), (string)reader.GetValue(2));
            reader.Close();
            return res;
        }
    }
}

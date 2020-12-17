using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

        internal void AddUser(User user)
        {
            string quer = "INSERT INTO [user] ([login], [password], [email])" +
            " VALUES ('" + user.Login + "', '" + user.Password + "', '" + user.Email + "');";
            Execute_SQL(quer);
        }

        internal void CloseConnection()
        {
            if (_cn.State != ConnectionState.Closed) _cn.Close();
        }

        internal bool UserExists(string login)
        {
            string quer = "SELECT * FROM [user] WHERE [login]='" + login + "';";
            SqlCommand cmd_Command = new SqlCommand(quer, _cn);
            SqlDataReader reader = cmd_Command.ExecuteReader();
            bool res = reader.Read();
            reader.Close();
            return res;
        }

        internal List<Guid> AllCarsInRent()
        {
            string quer = "SELECT [car_id] FROM [rent] WHERE [rent_end]>'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "';";
            SqlCommand cmd_Command = new SqlCommand(quer, _cn);
            SqlDataReader reader = cmd_Command.ExecuteReader();
            List<Guid> res = new List<Guid>();
            while (reader.Read())
                res.Add(new Guid((string)reader.GetValue(0)));
            reader.Close();
            return res;
        }
        internal void InsertCarRent(Guid carId, long userId, DateTime timeBegin, DateTime timeEnd)
        {
            string quer = "INSERT INTO [rent] ([car_id], [user_id], [rent_begin], [rent_end])" +
            " VALUES ('" + carId.ToString() + "', '" + userId + "', '" 
            + timeBegin.ToString("yyyy-MM-dd HH:mm:ss.fff") + "', '" + timeEnd.ToString("yyyy-MM-dd HH:mm:ss.fff") + "');";
            Execute_SQL(quer);
        }

        internal bool RentAvail(long userId)
        {
            string quer = "SELECT * FROM [rent] WHERE [user_id]='" + userId + "';";
            SqlCommand cmd_Command = new SqlCommand(quer, _cn);
            SqlDataReader reader = cmd_Command.ExecuteReader();
            bool res = !reader.Read();
            reader.Close();
            return res;
        }

        internal bool CarRentExists(Guid carId)
        {
            string quer = "SELECT * FROM [rent] WHERE [car_id]='" + carId.ToString() + "';";
            SqlCommand cmd_Command = new SqlCommand(quer, _cn);
            SqlDataReader reader = cmd_Command.ExecuteReader();
            bool res = reader.Read();
            reader.Close();
            return res;
        }

        internal void UpdateCarRent(Guid carId, long userId, DateTime timeBegin, DateTime timeEnd)
        {
            string quer = "UPDATE [rent] SET [user_id]='" + userId + "', " +
                "[rent_begin]='" + timeBegin.ToString("yyyy-MM-dd HH:mm:ss.fff") + "', " +
                "[rent_end]='" + timeEnd.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' WHERE [car_id]='" + carId.ToString() + "';";
            Execute_SQL(quer);
        }

        internal int LoginEmailFree(string login, string email)
        {
            string quer = "SELECT * FROM [user] WHERE [login]='" + login + "' OR [email]='" + email + "';";
            SqlCommand cmd_Command = new SqlCommand(quer, _cn);
            SqlDataReader reader = cmd_Command.ExecuteReader();
            bool res = reader.Read();
            if (!res)
            {
                reader.Close();
                return 0;
            }
            if (((string)reader.GetValue(1)).Equals(login))
            {
                reader.Close();
                return 1;
            }
            reader.Close();
            return 2;
        }


        public static void Execute_SQL(string SQL_Text)
        {
            SqlCommand cmd_Command = new SqlCommand(SQL_Text, _cn);
            cmd_Command.ExecuteNonQuery();
        }

        internal User GetUserByLogin(string login)
        {
            string quer = "SELECT * FROM [user] WHERE [login]='" + login + "';";
            SqlCommand cmd_Command = new SqlCommand(quer, _cn);
            SqlDataReader reader = cmd_Command.ExecuteReader();
            reader.Read();
            User res = new User((long)reader.GetValue(0),(string)reader.GetValue(1), (string)reader.GetValue(2), (string)reader.GetValue(3));
            reader.Close();
            return res;
        }
    }
}

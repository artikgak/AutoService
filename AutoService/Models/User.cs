using System.Text.RegularExpressions;
using AutoService.Exceptions;
using MongoDB.Bson.Serialization.Attributes;

namespace AutoService.Models
{
    internal class User
    {

        #region Fields
        private long _id;
        private string _login;
        private string _password;
        private string _email;
        #endregion

        #region Constructors
        public User(long id, string login, string password, string email)
        {
            _id = id;
            _login = login;
            _password = password;
            if (!IsEmailValid(email))
                throw new EmailDuplicateException();
            _email = email;
        }

        public User(string login, string password, string email)
        {
            _login = login;
            _password = password;
            if (!IsEmailValid(email))
                throw new EmailDuplicateException();
            _email = email;
        }

        public User(string login)
        {
            _login = login;
        }

        #endregion

        #region Properties
        public string Login
        {
            get { return _login; }
            set { _login = value; }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        [BsonIgnore]
        public string Password
        {
            get { return _password; }
        }

        public long ID
        {
            get { return _id; }
        }
        #endregion

        public override string ToString()
        {
            return $"User: {Login}\nEmail: {Email}";
        }

        private bool IsEmailValid(string email)
        {
            return Regex.IsMatch(email, "\\w+@(\\w+.)+[a-z]{2,4}", RegexOptions.IgnoreCase);
        }

    }
}

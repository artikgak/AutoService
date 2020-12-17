using System.Text.RegularExpressions;
using AutoService.Exceptions;

namespace AutoService.Models
{
    internal class User
    {
        private long _id;
        private string _login;
        private string _password;
        private string _email;

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

        private bool IsEmailValid(string email)
        {
            return Regex.IsMatch(email, "\\w+@(\\w+.)+[a-z]{2,4}", RegexOptions.IgnoreCase);
        }

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

        public string Password
        {
            get { return _password; }
        }

        public long ID
        {
            get { return _id; }
        }

        private void SetPassword(string password)
        {
            //TODO Add encription
            _password = password;
        }

        internal bool CheckPassword(string password)
        {
            //TODO Compare encrypted passwords
            return _password == password;
        }

        public override string ToString()
        {
            return $"User: {Login}\nEmail: {Email}";
        }

    }
}

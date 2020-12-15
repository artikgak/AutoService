using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoService.Exceptions;

namespace AutoService.Models
{
    internal class User
    {
        private string _login;
        private string _password;
        private string _email;

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

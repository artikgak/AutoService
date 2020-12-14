using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            _email = email;
        }

        public User(string login, string password)
        {
            _login = login;
            SetPassword(password);
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
            return $"{Login}\n{Email}";
        }

    }
}

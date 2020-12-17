using System;

namespace AutoService.Exceptions
{
    internal class LoginException : Exception
    {
        public LoginException() { }
        public LoginException(string login, string password) : base("Error. Login password not match\nlogin:"
            + login + "\npassword:" + password)
        { }
        public LoginException(string login, string password, Exception inner) : base("Error. Login password not match\nlogin:"
            + login + "\npassword:" + password, inner)
        { }
    }
}

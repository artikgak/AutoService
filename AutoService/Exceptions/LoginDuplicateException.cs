using System;

namespace AutoService.Exceptions
{
    class LoginDuplicateException : Exception
    {
        public LoginDuplicateException() { }
        public LoginDuplicateException(string login) : base("Error. Login:" + login + " already exists")
        { }
        public LoginDuplicateException(string login, Exception inner) : base("Error. Login:" + login + " already exists", inner)
        { }
    }
}

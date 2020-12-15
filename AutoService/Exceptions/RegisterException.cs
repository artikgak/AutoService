using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.Exceptions
{
    internal class RegisterException : Exception
    {
        public RegisterException() { }
        public RegisterException(string login, string email, string message) : base("Error. Register fail login:" 
            + login + "email:" + email + "Reason:" + message)
        { }
        public RegisterException(string login, string email, string message, Exception inner) : 
            base("Error. Register fail login:" + login + "email:" + email + "Reason:" + message, inner)
        { }
    }
}

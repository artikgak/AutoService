using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.Exceptions
{
    internal class EmailDuplicateException : Exception
    {
        public EmailDuplicateException() { }
        public EmailDuplicateException(string email) : base("Error. Email:" + email + " already exists")
        { }
        public EmailDuplicateException(string email, Exception inner) : base("Error. Email:" + email + " already exists", inner)
        { }
    }
}

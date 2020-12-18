using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoService.Models;

namespace AutoService.Logs
{
    internal class LogRegLog : ILoggable
    {
        /*
        private User _user;
        private DateTime _datetime;

        internal LogRegLog(User us, DateTime dt)
        {
            _user = us;
            _datetime = dt;
        }*/
        public string Message
        {
            set; get;
        }

        public User User
        {
            set; get;
        }

        public DateTime LogDateTime
        {
            set; get;
        }


    }
}

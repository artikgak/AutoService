using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoService.Models;

namespace AutoService.Logs
{
    internal class RentLog: ILoggable
    {
        public string Message
        {
            set; get;
        }

        public User User
        {
            set; get;
        }

        public Car Car
        { get; set; }

        public DateTime RentBegin { get; set; }

        public DateTime RentEnd { get; set; }

        public DateTime LogDateTime
        {
            set; get;
        }
    }
}

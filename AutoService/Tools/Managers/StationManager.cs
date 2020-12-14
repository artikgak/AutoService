using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoService.Exceptions;
using AutoService.Models;
using AutoService.Tools.DataStorage;

namespace AutoService.Tools.Managers
{
    class StationManager
    {

        private static UserDataStorage _userdataStorage;
        private static CarDataStorage _cardataStorage;
        private static StationManager _instance;
        private static readonly object Locker = new object();

        internal void Initialize(string fileName)
        {
            string filePath = FileFolderHelper.CreateFile(fileName);
            _userdataStorage = new UserDataStorage(filePath);
        }

        internal static StationManager Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;
                lock (Locker)
                {
                    return _instance ?? (_instance = new StationManager());
                }
            }
        }

        internal static User CurrentUser { get; set; }

        internal static void Login(string login, string password)
        {
            if(_userdataStorage.UserExists(login))
            {
                User us = _userdataStorage.GetUserByLogin(login);
                if (us.Password.Equals(password))
                {
                    CurrentUser = us;
                    return;
                }
            }
            throw new LoginException();
        }

        internal static UserDataStorage DataStorage
        {
            get { return _userdataStorage; }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        internal void Initialize(string userDBfileName, string carDBfileName)
        {
            string filePath = FileFolderHelper.CreateFile(userDBfileName);
            _userdataStorage = new UserDataStorage(filePath);

            _cardataStorage = new CarDataStorage(carDBfileName);
            //_cardataStorage.InsertRecord("Autos", new Car ( "lada","sedan" ));
            //_cardataStorage.InsertRecord("Autos", new Car ("mersedes", "c3"));
            //_cardataStorage.InsertRecord("Autos", new Car ("lanos", "s5"));
            var recs = _cardataStorage.LoadRecords<Car>("Autos");
            Car oneRec = _cardataStorage.LoadRecordById<Car>("Autos", new Guid("f2291801-9ba7-4c28-90fe-4802a72507f8"));
            //_cardataStorage.UpsertRecord("Autos", oneRec.Guid, oneRec);
            //_cardataStorage.DeleteRecord<Car>("Autos", oneRec.Guid);
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

        internal static void LogOut()
        {
            Debug.Assert(CurrentUser!=null);
            CurrentUser = null;
        }

        internal static void Login(string login, string password)
        {
            Debug.Assert(CurrentUser == null);
            if (_userdataStorage.UserExists(login))
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

        internal static void Register(User us)
        {
            int response = _userdataStorage.LoginEmailFree(us.Login, us.Email);
            if (response == 1)
                throw new LoginDuplicateException(us.Login);
            if (response == 2)
                throw new EmailDuplicateException(us.Email);
            // if exists with same email throw exception
            _userdataStorage.AddUser(us);         
        }

        internal static UserDataStorage DataStorage
        {
            get { return _userdataStorage; }
        }

    }
}

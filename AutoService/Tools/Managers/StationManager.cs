﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using AutoService.Exceptions;
using AutoService.Logs;
using AutoService.Models;
using AutoService.Tools.DataStorage;
using AutoService.ViewModels;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AutoService.Tools.Managers
{
    class StationManager
    {

        #region Fields
        private static UserDataStorage _userdataStorage;
        private static MongoDataStorage _mongodataStorage;
        private static StationManager _instance;
        private static readonly object Locker = new object();
        #endregion

        internal void Initialize(string userDBfileName, string carDBfileName)
        {
            string filePath = FileFolderHelper.CreateFile(userDBfileName);
            _userdataStorage = new UserDataStorage(filePath);
            _mongodataStorage = new MongoDataStorage(carDBfileName);
            //FillCarsMethod();
        }

        private static void FillCarsMethod()
        {
            //_cardataStorage.InsertRecord("Autos", new Car("AUDI", "Q7", "AUTO", 55, "/Resources/AudiQ7.jpg"));
            //_cardataStorage.InsertRecord("Autos", new Car("AUDI", "100", "ROBOT", 131, "/Resources/Audi100.jpg"));
            //_cardataStorage.InsertRecord("Autos", new Car("AUDI", "e-tron", "MECHANICS", 75, "/Resources/Audie-tron.jpg"));
            //_cardataStorage.InsertRecord("Autos", new Car("BMW", "4-series", "AUTO", 37, "/Resources/BMW4-series.jpg"));
            //_cardataStorage.InsertRecord("Autos", new Car("BMW", "X5", "ROBOT", 45, "/Resources/BMWX5.jpg"));
            //_cardataStorage.InsertRecord("Autos", new Car("BMW", "Z8", "AUTO", 61, "/Resources/BMWZ8.jpg"));
            //_cardataStorage.InsertRecord("Autos", new Car("MOSKVYCH", "2141", "MECHANICS", 125, "/Resources/MOSKVYCH2141.jpg"));
            //_cardataStorage.InsertRecord("Autos", new Car("LANOS", "T150", "MECHANICS", 93, "/Resources/LanosT150.jpg"));
            //_cardataStorage.InsertRecord("Autos", new Car("VOLVO", "244", "VARIATOR", 37, "/Resources/VOLVO244.jpg"));
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

        internal static UserDataStorage DataStorage
        {
            get { return _userdataStorage; }
        }

        #region Cars MongoDB
        internal static List<Car> getAllCars() { return _mongodataStorage.LoadRecords<Car>("Autos"); }

        internal static List<string> getAllMarks()
        {
          return _mongodataStorage.GetCollection<Car>("Autos")
                .Distinct<string>("Mark", FilterDefinition<Car>.Empty).ToList();
        }

        internal static List<string> getAllGearBoxes()
        {
            //var filter1 = Builders<Car>.Filter.Eq("Mark", mark);
            //var filter2 = Builders<Car>.Filter.Eq("Model", model);
            //var filterAnd = Builders<Car>.Filter.And(new List<FilterDefinition<Car>> { filter1, filter2 });

            return _mongodataStorage.GetCollection<Car>("Autos")
                  .Distinct<string>("GearBox", FilterDefinition<Car>.Empty).ToList();
        }

        internal static int getMinPrice()
        {
            var sort = Builders<Car>.Sort.Ascending(x => x.Price);
            var coll = _mongodataStorage.GetCollection<Car>("Autos");
            return coll.Find(new BsonDocument()).Sort(sort).Limit(1).ToList()[0].Price;
        }

        internal static int getMaxPrice()
        {
            var sort = Builders<Car>.Sort.Descending(x => x.Price);
            var coll = _mongodataStorage.GetCollection<Car>("Autos");
            return coll.Find(new BsonDocument()).Sort(sort).Limit(1).ToList()[0].Price;
        }

        internal static List<string> getAllModels(string mark)
        {
            var filter = Builders<Car>.Filter.Eq("Mark", mark);
            return _mongodataStorage.GetCollection<Car>("Autos")
                .Distinct<string>("Model", filter).ToList();
        }

        internal static ObservableCollection<CarViewModel> getCars(string mark, string model, 
            string gearBox, int priceFrom, int priceTo)
        {
            var coll = _mongodataStorage.GetCollection<Car>("Autos");

            BsonDocument bmark = mark.Equals("Not Selected") ? new BsonDocument() : new BsonDocument("Mark", mark);
            BsonDocument bmodel = model.Equals("Not Selected") ? new BsonDocument() : new BsonDocument("Model", model);
            BsonDocument bgearbox = gearBox.Equals("Not Selected") ? new BsonDocument() : new BsonDocument("GearBox", gearBox);
            BsonDocument bPrFrom = new BsonDocument("Price", new BsonDocument("$gte", priceFrom));
            BsonDocument bPrTo = new BsonDocument("Price", new BsonDocument("$lte", priceTo));
            var alreadyInRent = AllCarsInRent();
            var notRented = Builders<Car>.Filter.Nin("_id", alreadyInRent);
            /*
            var filter = new BsonDocument("$and", new BsonArray{
                bmark, bmodel, bgearbox, bPrFrom, bPrTo
            });*/

            var filter = Builders<Car>.Filter.And(new List<FilterDefinition<Car>> {
                bmark, bmodel, bgearbox, bPrFrom, bPrTo, notRented
            });

            List<Car> carlist = coll.Find(filter).ToList();
            ObservableCollection<CarViewModel> os = new ObservableCollection<CarViewModel>();
            for (int i = 0; i < carlist.Count; ++i)
                os.Add(new CarViewModel(carlist[i]));
            return os;
        }
        #endregion

        #region Cars Rent

        internal static void AddCarRent(Guid carId, long userId, DateTime timeBegin, DateTime timeEnd)
        {
            Debug.Assert(CurrentUser != null);
            if (_userdataStorage.CarRentExists(carId))
                _userdataStorage.UpdateCarRent(carId, userId, timeBegin, timeEnd);
            else
                _userdataStorage.InsertCarRent(carId, userId, timeBegin, timeEnd);
        }

        internal static bool RentAvail()
        {
            return _userdataStorage.RentAvail(CurrentUser.ID);
        }

        internal static Car CarRented()
        {
            Guid? carGuid = _userdataStorage.CarRented(CurrentUser.ID);
            if (carGuid == null) return null;
            return _mongodataStorage.LoadRecordById<Car>("Autos", (Guid)carGuid);
        }

        internal static List<Guid> AllCarsInRent()
        {
            Debug.Assert(CurrentUser != null);
            return _userdataStorage.AllCarsInRent();
        }

        internal static List<DateTime> RentTime(Guid carId)
        {
            return _userdataStorage.RentTime(carId);
        }

        #endregion

        #region User

        internal static void LogOut()
        {
            Debug.Assert(CurrentUser != null);
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

        #endregion

        #region Logs

        internal static void Log(ILoggable log)
        {
            _mongodataStorage.InsertRecord<ILoggable>("Logs", log);
        }

        #endregion

    }
}

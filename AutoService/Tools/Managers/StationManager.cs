using System.Collections.Generic;
using System.Diagnostics;
using AutoService.Exceptions;
using AutoService.Models;
using AutoService.Tools.DataStorage;
using MongoDB.Bson;
using MongoDB.Driver;

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
            //var recs = _cardataStorage.LoadRecords<Car>("Autos");
            //Car oneRec = _cardataStorage.LoadRecordById<Car>("Autos", new Guid("f2291801-9ba7-4c28-90fe-4802a72507f8"));
            //_cardataStorage.UpsertRecord("Autos", oneRec.Guid, oneRec);
            //_cardataStorage.DeleteRecord<Car>("Autos", oneRec.Guid);
            //NewMethod();
        }

        private static void NewMethod()
        {
            _cardataStorage.InsertRecord("Autos", new Car("AUDI", "Q7", "AUTO", 55));
            _cardataStorage.InsertRecord("Autos", new Car("AUDI", "100", "ROBOT", 131));
            _cardataStorage.InsertRecord("Autos", new Car("AUDI", "e-tron", "MECHANICS", 75));
            _cardataStorage.InsertRecord("Autos", new Car("BMW", "4-series", "AUTO", 37));
            _cardataStorage.InsertRecord("Autos", new Car("BMW", "X5", "ROBOT", 45));
            _cardataStorage.InsertRecord("Autos", new Car("BMW", "Z8", "AUTO", 61));
            _cardataStorage.InsertRecord("Autos", new Car("MOSKVYCH", "2141", "MECHANICS", 125));
            _cardataStorage.InsertRecord("Autos", new Car("LANOS", "T150", "MECHANICS", 93));
            _cardataStorage.InsertRecord("Autos", new Car("VOLVO", "244", "VARIATOR", 37));
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

        internal static List<Car> getAllCars() { return _cardataStorage.LoadRecords<Car>("Autos"); }

        internal static List<string> getAllMarks()
        {
          return _cardataStorage.GetCollection<Car>("Autos")
                .Distinct<string>("Mark", FilterDefinition<Car>.Empty).ToList();
        }

        internal static List<string> getAllGearBoxes()
        {
            //var filter1 = Builders<Car>.Filter.Eq("Mark", mark);
            //var filter2 = Builders<Car>.Filter.Eq("Model", model);
            //var filterAnd = Builders<Car>.Filter.And(new List<FilterDefinition<Car>> { filter1, filter2 });

            return _cardataStorage.GetCollection<Car>("Autos")
                  .Distinct<string>("GearBox", FilterDefinition<Car>.Empty).ToList();
        }

        internal static int getMinPrice()
        {
            var sort = Builders<Car>.Sort.Ascending(x => x.Price);
            var coll = _cardataStorage.GetCollection<Car>("Autos");
            return coll.Find(new BsonDocument()).Sort(sort).Limit(1).ToList()[0].Price;
        }

        internal static int getMaxPrice()
        {
            var sort = Builders<Car>.Sort.Descending(x => x.Price);
            var coll = _cardataStorage.GetCollection<Car>("Autos");
            return coll.Find(new BsonDocument()).Sort(sort).Limit(1).ToList()[0].Price;
        }

        internal static List<string> getAllModels(string mark)
        {
            var filter = Builders<Car>.Filter.Eq("Mark", mark);

               
            return _cardataStorage.GetCollection<Car>("Autos")
                .Distinct<string>("Model", filter).ToList();
        }

        internal static List<Car> getCars(string mark, string model, string gearBox, int priceFrom, int priceTo)
        {
            var coll = _cardataStorage.GetCollection<Car>("Autos");
            BsonDocument bmark = mark.Equals("Not Selected") ? new BsonDocument() : new BsonDocument("Mark", mark);
            BsonDocument bmodel = model.Equals("Not Selected") ? new BsonDocument() : new BsonDocument("Model", model);
            BsonDocument bgearbox = gearBox.Equals("Not Selected") ? new BsonDocument() : new BsonDocument("GearBox", gearBox);
            BsonDocument bPrFrom = new BsonDocument("Price", new BsonDocument("$gte", priceFrom));
            BsonDocument bPrTo = new BsonDocument("Price", new BsonDocument("$lte", priceTo));

            var filter = new BsonDocument("$and", new BsonArray{
                bmark, bmodel, bgearbox, bPrFrom, bPrTo
            });

            return coll.Find(filter).ToList();
        }

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

        internal static UserDataStorage DataStorage
        {
            get { return _userdataStorage; }
        }

    }
}

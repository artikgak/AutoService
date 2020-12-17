using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AutoService.ViewModels;
using MongoDB.Bson.Serialization.Attributes;

namespace AutoService.Models
{
    public class Car : INotifyPropertyChanged
    {

        #region Fields

        private Guid _id;
        internal string _mark;
        private string _model;
        private string _gearBox;
        private int _price;

        private string _carInfo;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        #endregion

        #region Properties
        [BsonId]
        public Guid Guid
        {
            get { return _id; }
            set 
            {
                _id = value;
            }
        }
        public string Mark
        {
            get { return _mark; }
            set 
            { 
                _mark = value;
                OnPropertyChanged();
            }
        }

        public string Model
        {
            get { return _model; }
            set 
            { 
                _model = value;
                CarInfo = ToString();
                OnPropertyChanged();
            }
        }

        public string GearBox
        {
            get { return _gearBox; }
            set 
            { 
                _gearBox = value;
                CarInfo = ToString();
                OnPropertyChanged();
            }
        }

        public int Price
        {
            get { return _price; }
            set 
            { 
                _price = value;
                OnPropertyChanged();
            }
        }

        public string CarInfo
        { 
            get 
            {
                return _carInfo; 
            }
            set
            {
                _carInfo = value;
                OnPropertyChanged();
            }
        }

        public string PriceInfo
        { get { return $"{Price}grn"; } }

        public override string ToString()
        {
            return $"Model: {Model}\nGearBox: {GearBox}";
        }

        public Car() 
        {
            _mark = "";
            _model = "";
            _gearBox = "";
            _price = 0;
            CarInfo = ToString();
        }

        public Car(string mark, string model, string gearbox, int price)
        {
            _mark = mark;
            _model = model;
            _gearBox = gearbox;
            _price = price;
            CarInfo = ToString();
        }
        #endregion

    }
}

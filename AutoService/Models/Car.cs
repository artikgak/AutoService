using System;
using MongoDB.Bson.Serialization.Attributes;

namespace AutoService.Models
{
    public class Car
    {

        #region Fields

        private Guid _id;
        private string _mark;
        private string _model;
        private string _gearBox;
        private int _price;
        #endregion

        #region Properties
        [BsonId]
        public Guid Guid
        {
            get { return _id; }
            set { _id = value; }
        }
        public string Mark
        {
            get { return _mark; }
            set { _mark = value; }
        }

        public string Model
        {
            get { return _model; }
            set { _model = value; }
        }

        public string GearBox
        {
            get { return _gearBox; }
            set { _gearBox = value; }
        }

        public int Price
        {
            get { return _price; }
            set { _price = value; }
        }

        public string CarInfo
        { get { return ToString(); } }

        public string PriceInfo
        { get { return $"Price hour: {Price}grn"; } }

        public override string ToString()
        {
            return $"Model: {Model}\nGearBox: {GearBox}";
        }

        public Car(string mark, string model, string gearbox, int price)
        {
            _mark = mark;
            _model = model;
            _gearBox = gearbox;
            _price = price;
        }
        #endregion

    }
}

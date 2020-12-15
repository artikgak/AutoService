using System;
using MongoDB.Bson.Serialization.Attributes;

namespace AutoService.Models
{
    public class Car
    {

        #region Fields
        
        private Guid _id;
        private string _mark;
        private string _type;
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

         public string Type 
         {
             get { return _type; }
             set { _type = value; }
         }
        
         public Car(string mark, string type)
         {
             this._mark = mark;
             this._type = type;
         }
        #endregion

    }
}

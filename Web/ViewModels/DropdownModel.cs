using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels
{
    public class DropdownModel
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId _id { get; set; }
        //public object MyProperty { get; set; }
    }
}

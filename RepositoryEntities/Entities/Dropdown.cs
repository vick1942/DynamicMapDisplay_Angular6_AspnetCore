using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Entities.Entities
{
    public class Dropdown
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId _id { get; set; }
    }
}

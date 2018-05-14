using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Entities
{
    public class Group
    {
         [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId _id { get; set; }
        [BsonElement("id")]
        public string GroupId { get; set; }
        [BsonElement("Organization")]
        public Organization Organization { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public List<Plan> Plans { get; set; }
    }
}

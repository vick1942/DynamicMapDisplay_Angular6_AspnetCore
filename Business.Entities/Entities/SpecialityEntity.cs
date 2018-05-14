using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Entities.Entities
{
    public class SpecialityEntity
    {
        [BsonElement("_id")]
        public ObjectId Id { get; set; }
        public string CodeNumber { get; set; }
        public string Speciality { get; set; }
    }
}

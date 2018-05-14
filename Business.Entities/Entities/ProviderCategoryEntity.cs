using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Entities.Entities
{
    public class ProviderCategoryEntity
    {
        [BsonElement("_id")]
        public ObjectId _id { get; set; }
        public string Code { get; set; }
        public string ProviderCategory { get; set; }
        public string Type { get; set; }
    }
}

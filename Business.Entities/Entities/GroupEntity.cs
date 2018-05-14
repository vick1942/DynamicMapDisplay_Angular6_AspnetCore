using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Entities
{
    public class GroupEntity
    {
        [BsonId]
        public ObjectId _id { get; set; }
        [BsonElement("id")]
        public string GroupId { get; set; }
        [BsonElement("Number")]
        public string Number { get; set; }
        [BsonElement("Name")]
        public string Name { get; set; }
        [BsonElement("Organization")]
        public Organization Organization { get; set; }
        [BsonElement("Plans")]
        public List<PlanEntity> Plans { get; set; }
    }
}



using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Business.Entities
{
    public class PlanEntity
    {
        public string Number { get; set; }
        public string Name { get; set; }
        [BsonElement("Networks")]
        public List<NetworkEntity> Networks { get; set; }
    }
}
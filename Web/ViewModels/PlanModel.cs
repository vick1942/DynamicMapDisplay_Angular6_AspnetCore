using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Web
{
    public class PlanModel
    {
        [BsonElement("Number")]
        public string Number { get; set; }
        [BsonElement("Name")]
        public string Name { get; set; }
        [BsonElement("Networks")]
        public List<Network> Networks { get; set; }
    }
}
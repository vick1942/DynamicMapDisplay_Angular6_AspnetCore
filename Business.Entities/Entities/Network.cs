using MongoDB.Bson.Serialization.Attributes;

namespace Business.Entities
{
    public class NetworkEntity
    {
        [BsonElement("Code")]
        public string Code { get; set; }
        [BsonElement("Level")]
        public int Level { get; set; }
    }
}
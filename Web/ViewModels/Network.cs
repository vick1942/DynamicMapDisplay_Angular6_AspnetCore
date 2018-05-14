using MongoDB.Bson.Serialization.Attributes;

namespace Web
{
    public class Network
    {
        [BsonElement("Code")]
        public string Code { get; set; }
        [BsonElement("Level")]
        public int Level { get; set; }
    }
}
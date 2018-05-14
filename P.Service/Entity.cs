using MongoDB.Bson;

namespace P.Service
{
    internal class Entity : BsonDocument
    {
        public string Name { get; set; }
    }
}

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace Business.Entities

{
    public class Organization
    {
        [BsonElement("Id")]
        public string OrganizationId { get; set; }
    }
}
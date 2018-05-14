using MongoDB.Bson.Serialization.Attributes;

namespace Repository.Entities
{
    public class Organization
    {
        [BsonElement("Id")]
        public string OrganizationId { get; set; }
    }
}
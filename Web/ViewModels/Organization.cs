using MongoDB.Bson.Serialization.Attributes;

namespace Web
{
    public class Organization
    {
        [BsonElement("Id")]
        public string OrganizationId { get; set; }
    }
}
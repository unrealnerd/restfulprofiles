using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProfileApi.Models
{
    public class Profile
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("age")]
        public string Age { get; set; }

        [BsonElement("email")]
        public decimal Email { get; set; }

        [BsonElement("phoneNumber")]
        public string PhoneNumber { get; set; }

        [BsonElement("home")]
        public Address Address { get; set; }
    }
}
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProfileApi.Models
{
    public class Address
    {
        [BsonElement("homeType")]
        public string Type { get; set; }

        [BsonElement("address")]
        public string Line1 { get; set; }

        [BsonElement("state")]
        public string State { get; set; }

        [BsonElement("zipcode")]
        public string ZipCode { get; set; }
    }
}

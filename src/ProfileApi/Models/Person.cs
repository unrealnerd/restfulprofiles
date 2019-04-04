using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProfileApi.Models
{
    public class Person
    {
        [BsonElement("firstName")]
        public string FirstName { get; set; }

        [BsonElement("lastName")]
        public string LastName { get; set; }

        [BsonElement("age")]
        public int Age { get; set; }
    }
}
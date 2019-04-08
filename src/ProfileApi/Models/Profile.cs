using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace ProfileApi.Models
{
    [Serializable]    
    public class Profile: IIdentifable
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]// this one makes it possible to keep Id property as string
        [BsonIgnoreIfDefault]
        public string Id { get; set; }

        [BsonElement("userId")]
        public int UserId { get; set; }

        [BsonElement("age")]
        public int Age { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("phoneNumber")]
        public string PhoneNumber { get; set; }

        [BsonElement("home")]// this attribute is to tell mongodb to map Address from home property
        [JsonProperty(PropertyName = "home")]// this attribute makes sure when you post/put home is mapped to address
        public Address Address { get; set; }

        [BsonElement("kids")]
        [BsonIgnoreIfNull]
        public IEnumerable<Person> Kids { get; set; }
    }
}
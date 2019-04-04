using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace ProfileApi.Models
{
    [Serializable]        
    public class Address
    {
        [BsonElement("homeType")]
        [JsonProperty(PropertyName = "homeType")]
        [BsonIgnoreIfNull]
        public string Type { get; set; }

        [BsonElement("address")]
        [JsonProperty(PropertyName = "address")]
        public string Line1 { get; set; }

        [BsonElement("state")]
        public string State { get; set; }

        [BsonElement("zipcode")]
        public string ZipCode { get; set; }
    }
}

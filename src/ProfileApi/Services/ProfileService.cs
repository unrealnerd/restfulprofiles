using System.Collections.Generic;
using System.Linq;
using ProfileApi.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace ProfileApi.Services
{
    public class ProfileService
    {
        private readonly IMongoCollection<Profile> _profiles;

        public ProfileService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("ProfilesDb"));
            var database = client.GetDatabase("profilesdb");
            _profiles = database.GetCollection<Profile>("profiles");
        }

        public List<Profile> Get()
        {
            return _profiles.Find(profile => true).ToList();
        }

        public Profile Get(string id)
        {
            return _profiles.Find<Profile>(profile => profile.Id == id).FirstOrDefault();
        }

        public Profile Create(Profile profile)
        {
            _profiles.InsertOne(profile);
            return profile;
        }

        public void Update(string id, Profile profileIn)
        {
            _profiles.ReplaceOne(profile => profile.Id == id, profileIn);
        }

        public void Remove(Profile profileIn)
        {
            _profiles.DeleteOne(profile => profile.Id == profileIn.Id);
        }

        public void Remove(string id)
        {
            _profiles.DeleteOne(profile => profile.Id == id);
        }
    }
}
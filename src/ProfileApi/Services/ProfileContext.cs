using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ProfileApi.Models;

namespace ProfileApi.Services
{
    public class ProfileContext : IDataContext<Profile>
    {
        private readonly IMongoDatabase _database = null;

        public ProfileContext(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<Profile> Collection => _database.GetCollection<Profile>("profiles");

    }
}
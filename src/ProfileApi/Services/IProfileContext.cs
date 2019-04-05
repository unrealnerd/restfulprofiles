using MongoDB.Driver;
using ProfileApi.Models;

namespace ProfileApi.Services
{
    public interface IProfileContext
    {
        IMongoCollection<Profile> Profiles { get; }
    }
}
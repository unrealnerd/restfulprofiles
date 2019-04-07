using MongoDB.Driver;
using ProfileApi.Models;

namespace ProfileApi.Services
{
    public interface IDataContext<T>
    {
        IMongoCollection<T> Collection { get; } 
    }
}
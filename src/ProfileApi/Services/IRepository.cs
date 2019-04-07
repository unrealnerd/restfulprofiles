using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using ProfileApi.Models;

namespace ProfileApi.Services
{
    public interface IRepository<T>
    {
        Task<List<T>> Get();
        Task<T> Get(string id);
        Task<T> Create(T inputData);
        Task Update(string id, T inputData);
        Task Remove(T inputData);
        Task Remove(string id);
        Task<List<T>> Query(FilterDefinition<T> filter);
    }
}
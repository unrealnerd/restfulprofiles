using System.Collections.Generic;
using System.Linq;
using ProfileApi.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using ProfileApi.Models.Query;

namespace ProfileApi.Services
{
    public class Repository<T> : IRepository<T> where T : class, IIdentifable
    {
        private readonly IDataContext<T> _context = null;

        public Repository(IDataContext<T> context)
        {
            _context = context;
        }

        public async Task<List<T>> Get()
        {
            return await _context.Collection.Find<T>(profile => true).ToListAsync<T>();
        }

        public async Task<T> Get(string id)
        {
            return await _context.Collection.Find<T>(profile => profile.Id == id).FirstOrDefaultAsync();
        }

        public async Task<T> Create(T profile)
        {
            try
            {
                await _context.Collection.InsertOneAsync(profile);
            }
            catch (System.Exception)
            {
                throw;
            }
            return profile;
        }

        public async Task Update(string id, T profileIn)
        {
            await _context.Collection.ReplaceOneAsync<T>(profile => profile.Id == id, profileIn);
        }

        public async Task Remove(T profileIn)
        {
            await _context.Collection.DeleteOneAsync(profile => profile.Id == profileIn.Id);
        }

        public async Task Remove(string id)
        {
            await _context.Collection.DeleteOneAsync(profile => profile.Id == id);
        }

        public async Task<List<T>> Query(FilterDefinition<T> filter)
        {            
            return await  _context.Collection.Find(filter).ToListAsync();
        }
    }
}
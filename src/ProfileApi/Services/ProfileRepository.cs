using System.Collections.Generic;
using System.Linq;
using ProfileApi.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace ProfileApi.Services
{
    public class ProfileRepository: IRepository<Profile>
    {
        private readonly IProfileContext _context = null;

        public ProfileRepository(IProfileContext context)
        {
            _context = context;
        }

        public async Task<List<Profile>> Get()
        {
            return await _context.Profiles.Find(profile => true).ToListAsync();
        }

        public async Task<Profile> Get(string id)
        {
            return await _context.Profiles.Find<Profile>(profile => profile.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Profile> Create(Profile profile)
        {
            await _context.Profiles.InsertOneAsync(profile);
            return  profile;
        }

        public async Task Update(string id, Profile profileIn)
        {
           await _context.Profiles.ReplaceOneAsync(profile => profile.Id == id, profileIn);
        }

        public async Task Remove(Profile profileIn)
        {
            await _context.Profiles.DeleteOneAsync(profile => profile.Id == profileIn.Id);
        }

        public async Task Remove(string id)
        {
            await _context.Profiles.DeleteOneAsync(profile => profile.Id == id);
        }
    }
}
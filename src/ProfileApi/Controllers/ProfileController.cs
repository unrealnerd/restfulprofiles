using System.Collections.Generic;
using ProfileApi.Models;
using ProfileApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ProfileApi.Models.Query;

namespace ProfileApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private readonly IRepository<Profile> _profileRepository;
        private readonly QueryBuilder _queryBuilder;

        public ProfilesController(IRepository<Profile> profileRepository, QueryBuilder queryBuilder)
        {
            _profileRepository = profileRepository;
            _queryBuilder = queryBuilder;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var profiles = await _profileRepository.Get();

            if (profiles == null)
            {
                return NotFound();
            }

            return Ok(profiles);
        }

        //since the IDs are managed by mongodb and is of length 24
        [HttpGet("{id:length(24)}", Name = "GetProfile")]
        public async Task<ActionResult> Get(string id)
        {
            var profile = await _profileRepository.Get(id);

            if (profile == null)
            {
                return NotFound();
            }

            return Ok(profile);
        }

        [HttpPost(Name = "QueryProfile")]
        [Route("query")]// since we have multiple post actions need to mention route explicitly
        public async Task<ActionResult> Query(Query query)
        {
            var profile = await _profileRepository.Query(_queryBuilder.Build(query));

            if (profile == null)
            {
                return NotFound();
            }

            return Ok(profile);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Profile profile)
        {
            var returnedProfile = await _profileRepository.Create(profile);

            if (returnedProfile == null)
            {
                return Conflict();
            }
            else
            {
                return CreatedAtRoute("GetProfile", new { id = returnedProfile.Id.ToString() }, returnedProfile);
            }
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Profile profile)
        {
            var profileFromRepo = _profileRepository.Get(id);

            if (profileFromRepo == null)
            {
                return NotFound();
            }

            await _profileRepository.Update(id, profile);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var profile = await _profileRepository.Get(id);

            if (profile == null)
            {
                return NotFound();
            }

            await _profileRepository.Remove(profile.Id);

            return NoContent();
        }
    }
}
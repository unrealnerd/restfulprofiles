using System.Collections.Generic;
using ProfileApi.Models;
using ProfileApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ProfileApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private readonly IRepository<Profile> _profileRepository;

        public ProfilesController(IRepository<Profile> profileRepository)
        {
            _profileRepository = profileRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Profile>>> Get()
        {
            return await _profileRepository.Get();
        }

        [HttpGet("{id:length(24)}", Name = "GetProfile")]
        public async Task<ActionResult<Profile>> Get(string id)
        {
            var profile = await _profileRepository.Get(id);

            if (profile == null)
            {
                return NotFound();
            }

            return profile;
        }

        [HttpPost]
        public async Task<ActionResult<Profile>> Create(Profile profile)
        {
            await _profileRepository.Create(profile);

            return CreatedAtRoute("GetProfile", new { id = profile.Id.ToString() }, profile);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Profile profileIn)
        {
            var profile = _profileRepository.Get(id);

            if (profile == null)
            {
                return NotFound();
            }

            await _profileRepository.Update(id, profileIn);

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
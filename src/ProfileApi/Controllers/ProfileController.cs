using System.Collections.Generic;
using ProfileApi.Models;
using ProfileApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ProfileApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private readonly ProfileService _profileService;

        public ProfilesController(ProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet]
        public ActionResult<List<Profile>> Get()
        {
            return _profileService.Get();
        }

        [HttpGet("{id:length(24)}", Name = "GetProfile")]
        public ActionResult<Profile> Get(string id)
        {
            var profile = _profileService.Get(id);

            if (profile == null)
            {
                return NotFound();
            }

            return profile;
        }

        [HttpPost]
        public ActionResult<Profile> Create(Profile profile)
        {
            _profileService.Create(profile);

            return CreatedAtRoute("GetProfile", new { id = profile.Id.ToString() }, profile);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Profile profileIn)
        {
            var profile = _profileService.Get(id);

            if (profile == null)
            {
                return NotFound();
            }

            _profileService.Update(id, profileIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var profile = _profileService.Get(id);

            if (profile == null)
            {
                return NotFound();
            }

            _profileService.Remove(profile.Id);

            return NoContent();
        }
    }
}
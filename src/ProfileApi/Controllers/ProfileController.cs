using ProfileApi.Models;
using ProfileApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ProfileApi.Models.Query;
using ProfileApi.Services.QueryService;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using ProfileApi.Services.LoggerService;

namespace ProfileApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private readonly IRepository<Profile> _Repository;
        private readonly IQueryBuilder<Profile> _queryBuilder;
        private readonly ILogger _logger;

        public ProfilesController(IRepository<Profile> Repository, IQueryBuilder<Profile> queryBuilder, ILogger<ProfilesController> logger)
        {
            _Repository = Repository;
            _queryBuilder = queryBuilder;
            _logger = logger;
        }

        //Gets all the profiles form the repo
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation(LoggingEvents.GetItem, "Getting all the Profiles");
            var profiles = await _Repository.Get();

            if (profiles == null)
            {
                _logger.LogError(LoggingEvents.GetItem, "Something must have gone wrong in the profiles repo since its not an empty list");                
            }
            else if(profiles.Count == 0)
            {
                return NotFound();
            }

            return Ok(profiles);
        }

        //since the IDs are managed by mongodb and is of length 24
        [HttpGet("{id:length(24)}", Name = "GetProfile")]
        public async Task<ActionResult> Get(string id)
        {
            _logger.LogInformation(LoggingEvents.GetItem, "Getting specified Profile: {id}", id);
            var profile = await _Repository.Get(id);

            if (profile == null)
            {
                _logger.LogInformation(LoggingEvents.GetItem, "Profile: {id} Not Found", id);                
                return NotFound();
            }

            return Ok(profile);
        }

        [HttpPost(Name = "QueryProfile")]        
        [Route("search")]// since we have multiple post actions need to mention route explicitly
        public async Task<ActionResult> Query(IList<Expression> expressions)
        {
            var profile = await _Repository.Query(_queryBuilder.Build(expressions));

            if (profile == null)
            {
                return NotFound();
            }

            return Ok(profile);
        }

        [HttpPost]
        [Route("create")]// since we have multiple post actions need to mention route explicitly        
        public async Task<ActionResult> Create(Profile profile)
        {
            var returnedProfile = await _Repository.Create(profile);

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
        [Route("update/{id}")]        
        public async Task<IActionResult> Update(string id, Profile profile)
        {
            var profileFromRepo = _Repository.Get(id);

            if (profileFromRepo == null)
            {
                return NotFound();
            }

            await _Repository.Update(id, profile);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var profile = await _Repository.Get(id);

            if (profile == null)
            {
                return NotFound();
            }

            await _Repository.Remove(profile.Id);

            return NoContent();
        }
    }
}
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
using Newtonsoft.Json;
using System;

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
            try
            {
                _logger.LogInformation(LoggingEvents.ReadItem, "Getting all the Profiles");

                var profiles = await _Repository.Get();

                if (profiles == null)
                {
                    _logger.LogError(LoggingEvents.ReadItem, "Something must have gone wrong in the profiles repo since its not an empty list");
                    return NotFound();
                }
                else if (profiles.Count == 0)
                {
                    return NotFound();
                }

                return Ok(profiles);
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.ReadItem, "There was an error while getting all Profiles");
                return StatusCode(500, ex.ToErrorResponse(LoggingEvents.ReadItemFailed));
            }
        }

        //since the IDs are managed by mongodb and is of length 24
        [HttpGet("{id:length(24)}", Name = "GetProfile")]
        public async Task<ActionResult> Get(string id)
        {
            try
            {
                _logger.LogInformation(LoggingEvents.ReadItem, "Getting specified Profile: {id}", id);

                var profile = await _Repository.Get(id);

                if (profile == null)
                {
                    _logger.LogInformation(LoggingEvents.ReadItem, "Profile: {id} Not Found", id);
                    return NotFound();
                }

                return Ok(profile);
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.ReadItemFailed, "There was an error while getting all Profiles");
                return StatusCode(500, ex.ToErrorResponse(LoggingEvents.ReadItemFailed));
            }
        }

        [HttpPost(Name = "QueryProfile")]
        [Route("search")]// since we have multiple post actions need to mention route explicitly
        public async Task<ActionResult> Query(IList<Expression> expressions)
        {
            try
            {
                _logger.LogInformation(LoggingEvents.SearchItem, "Searching for profile");

                var profile = await _Repository.Query(_queryBuilder.Build(expressions));

                if (profile == null)
                {
                    _logger.LogError(LoggingEvents.SearchItem, "Something must have gone wrong in the profiles repo since its not an empty list is returned");
                }
                else if (profile.Count == 0)
                {
                    return NotFound();
                }

                return Ok(profile);
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.SearchItemFailed, "Error Executing the search query: {expressions}", JsonConvert.SerializeObject(expressions));
                return StatusCode(500, ex.ToErrorResponse(LoggingEvents.SearchItemFailed));
            }
        }

        [HttpPost]
        [Route("create")]// since we have multiple post actions need to mention route explicitly        
        public async Task<ActionResult> Create(Profile profile)
        {
            try
            {
                _logger.LogInformation(LoggingEvents.CreateItem, "Creating a new profile");

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
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.CreateItemFailed, "There was an error when creating a profile");
                return StatusCode(500, ex.ToErrorResponse(LoggingEvents.CreateItemFailed));
            }
        }

        [HttpPut("{id:length(24)}")]
        [Route("update/{id}")]
        public async Task<IActionResult> Update(string id, Profile profile)
        {
            try
            {
                _logger.LogInformation(LoggingEvents.UpdateItem, "Updating a profile");

                var profileFromRepo = _Repository.Get(id);

                if (profileFromRepo == null)
                {
                    return NotFound();
                }

                await _Repository.Update(id, profile);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.UpdateItemFailed, "There was an error when updating a profile");
                return StatusCode(500, ex.ToErrorResponse(LoggingEvents.UpdateItemFailed));
            }
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                _logger.LogInformation(LoggingEvents.DeleteItem, "Deleting a profile");

                var profile = await _Repository.Get(id);

                if (profile == null)
                {
                    return NotFound();
                }

                await _Repository.Remove(profile.Id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.DeleteItemFailed, "There was an error when deleting a profile");
                return StatusCode(500, ex.ToErrorResponse(LoggingEvents.DeleteItemFailed));
            }
        }
    }
}
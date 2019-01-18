using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using graphApiService.Dtos.User;
using graphApiService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.Azure.ActiveDirectory.GraphClient.Extensions;

namespace graphApiService.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IGraphClientService _graphClient;
        private readonly IMapper _mapper;

        public UsersController(IGraphClientService graphClient, IMapper mapper)
        {
            _graphClient = graphClient;
            _mapper = mapper;
        }

        // GET api/users
        [HttpGet]
        public async Task<ActionResult<List<UserProfileDto>>> Get()
        {
            IPagedCollection<IUser> users = _graphClient.GetAllUsers();

            List<UserProfileDto> result = new List<UserProfileDto>();

            do
            {
                foreach (var user in users.CurrentPage)
                {
                    result.Add(_mapper.Map<UserProfileDto>(user));
                }

                users = await users.GetNextPageAsync();
            } while (users?.MorePagesAvailable == true);

            return result;
        }
        // GET api/users/5
        [HttpGet("{objectId}", Name = "User")]
        public UserProfileDto Get(string objectId)
        {
            IUser user = _graphClient.GetUserByObjectId(objectId);

            return _mapper.Map<UserProfileDto>(user);
        }

        // POST api/users
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User newUserProfile)
        {
            UserProfileDto userToResponse;

            try
            {
                IUser user = await _graphClient.CreateUserAsync(newUserProfile);
                userToResponse = _mapper.Map<UserProfileDto>(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return Created("users/" + userToResponse.ObjectId, userToResponse);
        }

        // PUT api/users/5
        [HttpPatch("{objectId}")]
        public async Task<IActionResult> Patch(string objectId, [FromBody] UserProfileEditableDto userToUpdate)
        {
            try
            {
                UserProfileDto userToResponse =
                    _mapper.Map<UserProfileDto>(await _graphClient.UpdateUserByObjectId(objectId, userToUpdate));
                return CreatedAtRoute("User", userToResponse.ObjectId, userToResponse);
            }
            catch (ObjectNotFoundException ex)
            {
                return NotFound(ex);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}

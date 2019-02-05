using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using graphApiService.Dtos.User;
using graphApiService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ActiveDirectory.GraphClient;

namespace graphApiService.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IGraphClientService _graphClient;

        public UsersController(IGraphClientService graphClient)
        {
            _graphClient = graphClient;
        }

        /// <summary>
        /// Returns collection of users from Active Directory
        /// </summary>
        /// <returns>Collection of users</returns>
        /// <response code="200">If users fetched successfully</response>
        /// <response code="401">If authorization token is invalid</response>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<IUser>))]
        public async Task<IActionResult> Get()
        {
            IEnumerable<UserProfileDto> users = await _graphClient.GetAllUsers();
            return Ok(users);
        }

        /// <summary>
        /// Get user by his objectId or userPrincipalName
        /// </summary>
        /// <param name="objectId">Azure AD B2C user uniq identifier. Can be objectId of userPrincipalName</param>
        /// <returns>User with passed identifier, or not found response</returns>
        /// <response code="200">If user fetched successfully</response>
        /// <response code="401">If authorization token is invalid</response>
        /// <response code="404">If user was not found</response>
        [HttpGet("{objectId}", Name = "User")]
        [ProducesResponseType(200, Type = typeof(UserProfileDto))]
        public async Task<IActionResult> Get(string objectId)
        {
            try
            {
                var user = await _graphClient.GetUserByObjectId(objectId);
                return Ok(user);
            }
            catch (ObjectNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Creates new user
        /// </summary>
        /// <param name="userCreatableDto">User DTO to create</param>
        /// <returns>URL to newly created user</returns>
        /// <response code="201">If user fetched successfully</response>
        /// <response code="401">If authorization token is invalid</response>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(UserProfileDto))]
        public async Task<IActionResult> Post([FromBody] [Required] UserProfileCreatableDto userCreatableDto)
        {
            UserProfileDto userToResponse = await _graphClient.CreateUserAsync(userCreatableDto);
            return Ok(userToResponse);
            //return CreatedAtRoute("User", new { objectId = userToResponse.ObjectId }, userToResponse);
        }

        /// <summary>
        /// Update user properties by objectId or userPrincipalName
        /// </summary>
        /// <param name="objectId">objectId or userPrincipalName of Azure AD B2C User</param>
        /// <param name="userToUpdate">User DTO to update</param>
        /// <returns>URL to updated user</returns>
        /// <response code="201">If user updated successfully</response>
        /// <response code="401">If authorization token is invalid</response>
        /// <response code="404">If user was not found</response>
        [HttpPatch("{objectId}")]
        public async Task<IActionResult> Patch(string objectId, [FromBody] [Required] UserProfileEditableDto userToUpdate)
        {
            try
            {
                UserProfileDto userToResponse = await _graphClient.UpdateUserByObjectId(objectId, userToUpdate);
                return Ok(userToResponse);
                //return CreatedAtRoute("User", new { objectId = userToResponse.ObjectId }, userToResponse);
            }
            catch (ObjectNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xyzies.TWC.Public.Api.Managers.Relation;

namespace Xyzies.TWC.Public.Api.Controllers
{
    /// <summary>
    /// Users controller
    /// </summary>
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly IRelationService _relationService;

        /// <summary>
        /// Constructor with DI
        /// </summary>
        /// <param name="relationService">Relation service</param>
        public UsersController(IRelationService relationService)
        {
            _relationService = relationService ?? throw new ArgumentNullException(nameof(relationService));
        }

        /// <summary>
        /// Get user on call by operator's Cable Portal id
        /// </summary>
        /// <param name="cpUserId">Operator's Cable Portal id</param>
        /// <returns></returns>
        [HttpGet("user-on-call/{cpUserId}")]
        public async Task<IActionResult> GetUserOnCallWith(int cpUserId)
        {
            try
            {
                var user = await _relationService.GetUserOnCallWithIdAsync(cpUserId);
                return Ok(user);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Xyzies.TWC.Public.Api.Managers.Interfaces;

namespace Xyzies.TWC.Public.Api.Controllers
{
    /// <summary>
    /// tenant settings controller
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("migration")]
    public class MigrationController : Controller
    {
        private readonly IMigrationManager _migrationManager;

        public MigrationController(IMigrationManager migrationManager)
        {
            _migrationManager = migrationManager;

        }

        /// <summary>
        /// Update
        /// </summary>
        /// <returns></returns>
        [HttpPut("{tenantId}")]
        [ProducesResponseType(StatusCodes.Status200OK  /* 200 */)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized /* 401 */)]
        [SwaggerOperation(Tags = new[] { "Migration API" })]
        public async Task<IActionResult> AssignCompaniesToTenant(Guid tenantId)//only for Spectrum
        {
            try
            {
                await _migrationManager.AssignToTenant(tenantId);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Xyzies.TWC.Public.Api.Managers.Interfaces;
using Xyzies.TWC.Public.Api.Models;

namespace Xyzies.TWC.Public.Api.Controllers
{
    /// <summary>
    /// tenant settings controller
    /// </summary>
    [ApiController]
    [Route("tenant-settings")]
    public class TenantSettingsController : Controller
    {
        /// <summary>
        /// Service provider settings controller ctor
        /// </summary>
        private readonly ITenantrSettingManager _tenantSettingManager;

        public TenantSettingsController(ITenantrSettingManager tenantSettingManager)

        {
            _tenantSettingManager = tenantSettingManager;

        }

        /// <summary>
        /// Get tenant settings by provider id(GUID)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{tenantId}")]
        [ProducesResponseType(typeof(TenantSettingModel), StatusCodes.Status200OK  /* 200 */)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized /* 401 */)]
        [SwaggerOperation(Tags = new[] { "Service provider settings API" })]
        public async Task<IActionResult> GetTenantSettingById(Guid tenantId)
        {
            try
            {
                var settings = await _tenantSettingManager.GetTenantSettings(tenantId);
                return Ok(settings);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Post tenant settings for provider
        /// </summary>
        /// <param name="providerId"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        [HttpPost("{tenantId}")]
        [ProducesResponseType(typeof(TenantSettingModel), StatusCodes.Status200OK  /* 200 */)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized /* 401 */)]
        [SwaggerOperation(Tags = new[] { "Service provider settings API" })]
        public async Task<IActionResult> PostTenantSetting(Guid tenantId, [FromBody] TenantSettingModel settings)
        {
            await _tenantSettingManager.InsertTenantSettings(tenantId, settings);
            return NoContent();
        }

        /// <summary>
        /// Update tenant settings by provider id(GUID)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        [HttpPut("{providerId}")]
        [ProducesResponseType(typeof(TenantSettingModel), StatusCodes.Status200OK  /* 200 */)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized /* 401 */)]
        [SwaggerOperation(Tags = new[] { "Service provider settings API" })]
        public async Task<IActionResult> UpdateTenantSettingById(Guid tenantId, TenantSettingModel settings)
        {
            try
            {
                await _tenantSettingManager.UpdateTenantSettings(tenantId, settings);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
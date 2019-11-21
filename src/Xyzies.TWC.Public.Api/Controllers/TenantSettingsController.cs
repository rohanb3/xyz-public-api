using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    [Route("tenant-settings")]
    public class TenantSettingsController : Controller
    {
        /// <summary>
        /// Tenant settings controller ctor
        /// </summary>
        private readonly ITenantSettingManager _tenantSettingManager;

        public TenantSettingsController(ITenantSettingManager tenantSettingManager)

        {
            _tenantSettingManager = tenantSettingManager;

        }

        /// <summary>
        /// Get tenant settings by id(GUID)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{tenantId}")]
        [ProducesResponseType(typeof(TenantSettingModel), StatusCodes.Status200OK  /* 200 */)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized /* 401 */)]
        [SwaggerOperation(Tags = new[] { "Tenant settings API" })]
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
        /// Get tenant settings by company id(int)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("by-company/{companyId}")]
        [ProducesResponseType(typeof(TenantSettingModel), StatusCodes.Status200OK  /* 200 */)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized /* 401 */)]
        [SwaggerOperation(Tags = new[] { "Tenant settings API" })]
        public async Task<IActionResult> GetTenantSettingByCompanyId(int companyId)
        {
            try
            {
                var settings = await _tenantSettingManager.GetTenantSettingsByCompanyId(companyId);
                return Ok(settings);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Post tenant settings for tenant
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        [HttpPost("{tenantId}")]
        [ProducesResponseType(typeof(TenantSettingModel), StatusCodes.Status200OK  /* 200 */)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized /* 401 */)]
        [SwaggerOperation(Tags = new[] { "Tenant settings API" })]
        public async Task<IActionResult> PostTenantSetting(Guid tenantId, [FromBody] TenantSettingModel settings)
        {
            await _tenantSettingManager.InsertTenantSettings(tenantId, settings);
            return NoContent();
        }

        /// <summary>
        /// Update tenant settings by tenant id(GUID)
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        [HttpPut("{tenantId}")]
        [ProducesResponseType(typeof(TenantSettingModel), StatusCodes.Status200OK  /* 200 */)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized /* 401 */)]
        [SwaggerOperation(Tags = new[] { "Tenant settings API" })]
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
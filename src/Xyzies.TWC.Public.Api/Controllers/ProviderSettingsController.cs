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
    /// Service provider settings controller
    /// </summary>
    [ApiController]
    [Route("provider-settings")]
    public class ProviderSettingsController : Controller
    {
        /// <summary>
        /// Service provider settings controller ctor
        /// </summary>
        private readonly IProviderSettingManager _providerSettingManager;

        public ProviderSettingsController(IProviderSettingManager providerSettingManager)

        {
            _providerSettingManager = providerSettingManager;

        }

        /// <summary>
        /// Get provider settings by provider id(GUID)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{providerId}")]
        [ProducesResponseType(typeof(ProviderSettingModel), StatusCodes.Status200OK  /* 200 */)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized /* 401 */)]
        [SwaggerOperation(Tags = new[] { "Service provider settings API" })]
        public async Task<IActionResult> GetProviderSettingById(Guid providerId)
        {
            try
            {
                var settings = await _providerSettingManager.GetProviderSettings(providerId);
                return Ok(settings);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Post provider settings for provider
        /// </summary>
        /// <param name="providerId"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        [HttpPost("{providerId}")]
        [ProducesResponseType(typeof(ProviderSettingModel), StatusCodes.Status200OK  /* 200 */)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized /* 401 */)]
        [SwaggerOperation(Tags = new[] { "Service provider settings API" })]
        public async Task<IActionResult> PostProviderSetting(Guid providerId, [FromBody] ProviderSettingModel settings)
        {
            await _providerSettingManager.InsertProviderSettings(providerId, settings);
            return NoContent();
        }

        /// <summary>
        /// Update provider settings by provider id(GUID)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        [HttpPut("{providerId}")]
        [ProducesResponseType(typeof(ProviderSettingModel), StatusCodes.Status200OK  /* 200 */)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized /* 401 */)]
        [SwaggerOperation(Tags = new[] { "Service provider settings API" })]
        public async Task<IActionResult> UpdateProviderSettingById(Guid providerId, ProviderSettingModel settings)
        {
            try
            {
                await _providerSettingManager.UpdateProviderSettings(providerId, settings);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
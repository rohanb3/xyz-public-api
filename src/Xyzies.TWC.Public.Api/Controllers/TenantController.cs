using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using Xyzies.TWC.Public.Api.Managers.Interfaces;
using Xyzies.TWC.Public.Api.Models;

namespace Xyzies.TWC.Public.Api.Controllers
{
    /// <summary>
    /// Service provider controller
    /// </summary>
    [ApiController]
    [Route("provider")]
    [Authorize]
    public class TenantController : ControllerBase
    {
        private readonly ILogger<TenantController> _logger = null;
        private readonly ITenantManager _tenantService = null;

        /// <summary>
        /// Service provider constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="TenantService"></param>
        public TenantController(ILogger<TenantController> logger,
            ITenantManager TenantService)
        {
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
            _tenantService = TenantService ??
                throw new ArgumentNullException(nameof(TenantService));
        }

        /// <summary>
        /// Create service provider
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created  /* 201 */)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized /* 401 */)]
        [SwaggerOperation(Tags = new[] { "Service provider API" })]
        public async Task<IActionResult> Post([FromBody] TenantRequest request)
        {
            try
            {
                var deviceId = await _tenantService.Create(request);
                return Created(HttpContext.Request.GetEncodedUrl(), deviceId);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DuplicateNameException ex)
            {
                return Conflict(ex.Message);
            }
        }

        /// <summary>
        /// Update service provider by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent  /* 204 */)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized /* 401 */)]
        [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound /* 404 */)]
        [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status409Conflict /* 409 */)]
        [SwaggerOperation(Tags = new[] { "Service provider API" })]
        public async Task<IActionResult> Put(Guid id, [FromBody] TenantRequest request)
        {
            try
            {
                await _tenantService.Update(id, request);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DuplicateNameException ex)
            {
                return Conflict(ex.Message);
            }
        }

        /// <summary>
        /// Update service provider by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id}/by-company")]
        [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent  /* 204 */)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized /* 401 */)]
        [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound /* 404 */)]
        [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status409Conflict /* 409 */)]
        [SwaggerOperation(Tags = new[] { "Service provider API" })]
        public async Task<IActionResult> Put(int id, [FromBody] TenantRequest request)
        {
            try
            {
                await _tenantService.UpdateByCompanyId(id, request);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DuplicateNameException ex)
            {
                return Conflict(ex.Message);
            }
        }

        /// <summary>
        /// Get service provider list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<TenantModel>), StatusCodes.Status200OK  /* 200 */)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized /* 401 */)]
        [SwaggerOperation(Tags = new[] { "Service provider API" })]
        public async Task<IActionResult> Get()
        {
            var TenantList = await _tenantService.Get();
            return Ok(TenantList);
        }

        /// <summary>
        /// Get service provider extended model by id(GUID)
        /// </summary>
        /// <returns></returns>
        [HttpGet("extended/{id}")]
        [ProducesResponseType(typeof(TenantModel), StatusCodes.Status200OK  /* 200 */)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized /* 401 */)]
        [SwaggerOperation(Tags = new[] { "Service provider API" })]
        public async Task<IActionResult> GetExtended(Guid id)
        {
            try
            {
                var Tenant = await _tenantService.GetExtended(id);
                return Ok(Tenant);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Get service provider single model by id(GUID)
        /// </summary>
        /// <returns></returns>
        [HttpGet("single/{id}")]
        [ProducesResponseType(typeof(TenantModel), StatusCodes.Status200OK  /* 200 */)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized /* 401 */)]
        [SwaggerOperation(Tags = new[] { "Service provider API" })]
        public async Task<IActionResult> GetSingle(Guid id)
        {
            try
            {
                var Tenant = await _tenantService.GetSingle(id);
                return Ok(Tenant);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Get service provider by company id(int)
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}/by-company")]
        [ProducesResponseType(typeof(TenantSingleModel), StatusCodes.Status200OK  /* 200 */)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized /* 401 */)]
        [SwaggerOperation(Tags = new[] { "Service provider API" })]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var Tenant = await _tenantService.Get(id);
                return Ok(Tenant);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Get single model of service provider by company id(int)
        /// </summary>
        /// <returns></returns>
        [HttpGet("single/{id}/by-company")]
        [ProducesResponseType(typeof(TenantSingleModel), StatusCodes.Status200OK  /* 200 */)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized /* 401 */)]
        [SwaggerOperation(Tags = new[] { "Service provider API" })]
        public async Task<IActionResult> GetSingle(int id)
        {
            try
            {
                var Tenant = await _tenantService.GetSingle(id);
                return Ok(Tenant);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
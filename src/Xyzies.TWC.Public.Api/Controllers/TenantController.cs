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
using Xyzies.TWC.Public.Api.Models.Filters;

namespace Xyzies.TWC.Public.Api.Controllers
{
    /// <summary>
    /// Tenant controller
    /// </summary>
    [ApiController]
    [Route("tenant")]
    [Authorize]
    public class TenantController : ControllerBase
    {
        private readonly ILogger<TenantController> _logger = null;
        private readonly ITenantManager _tenantService = null;

        /// <summary>
        /// Tenant controller constructor
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
        /// Create Tenant
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created  /* 201 */)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized /* 401 */)]
        [SwaggerOperation(Tags = new[] { "Tenant API" })]
        public async Task<IActionResult> Post([FromBody] TenantRequest request)
        {
            try
            {
                var tenantId = await _tenantService.Create(request);
                return Created(HttpContext.Request.GetEncodedUrl(), tenantId);
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
        /// Create company to tenant relation
        /// </summary>
        /// <returns></returns>
        [HttpPost("{companyId}/in/{tenantId}")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created  /* 201 */)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized /* 401 */)]
        [SwaggerOperation(Tags = new[] { "Tenant API" })]
        public async Task<IActionResult> PostRelation(int companyId, Guid tenantId)
        {
            try
            {
                await _tenantService.CreateRelation(companyId, tenantId);
                return NoContent();
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
        /// Update Tenant by Id
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
        [SwaggerOperation(Tags = new[] { "Tenant API" })]
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
        /// Update Tenant by Id
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
        [SwaggerOperation(Tags = new[] { "Tenant API" })]
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
        /// Get Tenants list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<TenantModel>), StatusCodes.Status200OK  /* 200 */)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized /* 401 */)]
        [SwaggerOperation(Tags = new[] { "Tenant API" })]
        public async Task<IActionResult> Get([FromQuery]TenantFilterModel filterModel)
        {
            var tenantList = await _tenantService.Get(filterModel);
            return Ok(tenantList);
        }

        /// <summary>
        /// Get Tenant extended model by id(GUID)
        /// </summary>
        /// <returns></returns>
        [HttpGet("extended/{id}")]
        [ProducesResponseType(typeof(TenantModel), StatusCodes.Status200OK  /* 200 */)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized /* 401 */)]
        [SwaggerOperation(Tags = new[] { "Tenant API" })]
        public async Task<IActionResult> GetExtended(Guid id)
        {
            try
            {
                var tenant = await _tenantService.GetExtended(id);
                return Ok(tenant);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Get Tenant single model by id(GUID)
        /// </summary>
        /// <returns></returns>
        [HttpGet("single/{id}")]
        [ProducesResponseType(typeof(TenantModel), StatusCodes.Status200OK  /* 200 */)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized /* 401 */)]
        [SwaggerOperation(Tags = new[] { "Tenant API" })]
        public async Task<IActionResult> GetSingle(Guid id)
        {
            try
            {
                var tenant = await _tenantService.GetSingle(id);
                return Ok(tenant);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Get Tenant single model by List ids
        /// </summary>
        /// <returns></returns>
        [HttpGet(Consts.PrefixForBaseUrl.TenantSimple)]
        [ProducesResponseType(typeof(IEnumerable<TenantWithCompaniesSimpleModel>), StatusCodes.Status200OK  /* 200 */)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized /* 401 */)]
        [SwaggerOperation(Tags = new[] { "Tenant API" })]
        public async Task<IActionResult> GetSimple([FromQuery]TenantFilterModel filterModel)
        {
            var tenant = await _tenantService.GetSimple(filterModel);
            return Ok(tenant);
        }

        /// <summary>
        /// Get Tenant by company id(int)
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}/by-company")]
        [ProducesResponseType(typeof(TenantSingleModel), StatusCodes.Status200OK  /* 200 */)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized /* 401 */)]
        [SwaggerOperation(Tags = new[] { "Tenant API" })]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var tenant = await _tenantService.Get(id);
                return Ok(tenant);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Get single model of Tenant by company id(int)
        /// </summary>
        /// <returns></returns>
        [HttpGet("single/{id}/by-company")]
        [ProducesResponseType(typeof(TenantSingleModel), StatusCodes.Status200OK  /* 200 */)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized /* 401 */)]
        [SwaggerOperation(Tags = new[] { "Tenant API" })]
        public async Task<IActionResult> GetSingle(int id)
        {
            try
            {
                var tenant = await _tenantService.GetSingle(id);
                return Ok(tenant);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Get single model of Tenant by company id(int)
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("trusted/{token}/single/{id}/by-company")]
        [ProducesResponseType(typeof(TenantSingleModel), StatusCodes.Status200OK  /* 200 */)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized /* 401 */)]
        [SwaggerOperation(Tags = new[] { "Tenant API" })]
        public async Task<IActionResult> GetSingle(int id, string token)
        {
            try
            {
                if (token != Consts.StaticToken)
                {
                    return new ContentResult { StatusCode = 403 };
                }
                var tenant = await _tenantService.GetSingle(id);
                return Ok(tenant);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
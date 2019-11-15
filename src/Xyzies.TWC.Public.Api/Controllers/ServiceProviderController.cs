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
    /// Service provider controller
    /// </summary>
    [ApiController]
    [Route("provider")]
    [Authorize]
    public class ServiceProviderController : ControllerBase
    {
        private readonly ILogger<ServiceProviderController> _logger = null;
        private readonly IServiceProviderManager _serviceProviderService = null;

        /// <summary>
        /// Service provider constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="serviceProviderService"></param>
        public ServiceProviderController(ILogger<ServiceProviderController> logger,
            IServiceProviderManager serviceProviderService)
        {
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
            _serviceProviderService = serviceProviderService ??
                throw new ArgumentNullException(nameof(serviceProviderService));
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
        public async Task<IActionResult> Post([FromBody] ServiceProviderRequest request)
        {
            try
            {
                var deviceId = await _serviceProviderService.Create(request);
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
        public async Task<IActionResult> Put(Guid id, [FromBody] ServiceProviderRequest request)
        {
            try
            {
                await _serviceProviderService.Update(id, request);
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
        public async Task<IActionResult> Put(int id, [FromBody] ServiceProviderRequest request)
        {
            try
            {
                await _serviceProviderService.UpdateByCompanyId(id, request);
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
        [ProducesResponseType(typeof(List<ServiceProviderModel>), StatusCodes.Status200OK  /* 200 */)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized /* 401 */)]
        [SwaggerOperation(Tags = new[] { "Service provider API" })]
        public async Task<IActionResult> Get([FromQuery]TenantFilterModel filterModel)
        {
            var serviceProviderList = await _serviceProviderService.Get(filterModel);
            return Ok(serviceProviderList);
        }

        /// <summary>
        /// Get service provider extended model by id(GUID)
        /// </summary>
        /// <returns></returns>
        [HttpGet("extended/{id}")]
        [ProducesResponseType(typeof(ServiceProviderModel), StatusCodes.Status200OK  /* 200 */)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized /* 401 */)]
        [SwaggerOperation(Tags = new[] { "Service provider API" })]
        public async Task<IActionResult> GetExtended(Guid id)
        {
            try
            {
                var serviceProvider = await _serviceProviderService.GetExtended(id);
                return Ok(serviceProvider);
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
        [ProducesResponseType(typeof(ServiceProviderModel), StatusCodes.Status200OK  /* 200 */)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized /* 401 */)]
        [SwaggerOperation(Tags = new[] { "Service provider API" })]
        public async Task<IActionResult> GetSingle(Guid id)
        {
            try
            {
                var serviceProvider = await _serviceProviderService.GetSingle(id);
                return Ok(serviceProvider);
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
        [ProducesResponseType(typeof(ServiceProviderSingleModel), StatusCodes.Status200OK  /* 200 */)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized /* 401 */)]
        [SwaggerOperation(Tags = new[] { "Service provider API" })]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var serviceProvider = await _serviceProviderService.Get(id);
                return Ok(serviceProvider);
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
        [ProducesResponseType(typeof(ServiceProviderSingleModel), StatusCodes.Status200OK  /* 200 */)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized /* 401 */)]
        [SwaggerOperation(Tags = new[] { "Service provider API" })]
        public async Task<IActionResult> GetSingle(int id)
        {
            try
            {
                var serviceProvider = await _serviceProviderService.GetSingle(id);
                return Ok(serviceProvider);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
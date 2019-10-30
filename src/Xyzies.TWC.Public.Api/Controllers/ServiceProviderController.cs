﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
    public class ServiceProviderController :ControllerBase
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
        /// Update service provider
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
        /// Get service provider list
        /// </summary>
        /// <param name="lazyLoadFilters"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<ServiceProviderModel>), StatusCodes.Status200OK  /* 200 */)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized /* 401 */)]
        [SwaggerOperation(Tags = new[] { "Service provider API" })]
        public async Task<IActionResult> Get()
        {
            var serviceProviderList = await _serviceProviderService.Get();
            return Ok(serviceProviderList);
        }
    }
}
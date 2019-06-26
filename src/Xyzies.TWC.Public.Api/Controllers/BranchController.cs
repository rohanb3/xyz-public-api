using System;
using System.Net;
using System.Linq;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Xyzies.TWC.Public.Api.Models;
using Xyzies.TWC.Public.Api.Managers;
using Xyzies.TWC.Public.Data.Entities;
using Xyzies.TWC.Public.Data.Repositories.Interfaces;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;

namespace Xyzies.TWC.Public.Api.Controllers
{
    /// <summary>
    /// Resource of branch
    /// </summary>
    [Authorize]
    [ApiController]
    public class BranchController : Controller
    {
        private readonly ILogger<BranchController> _logger = null;
        private readonly IBranchRepository _branchRepository = null;
        private readonly IBranchManager _branchManager = null;

        /// <summary>
        /// Ctor with dependencies
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="branchRepository"></param>
        /// <param name="branchManager"></param>
        public BranchController(ILogger<BranchController> logger,
            IBranchRepository branchRepository,
            IBranchManager branchManager)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _branchRepository = branchRepository ?? throw new ArgumentNullException(nameof(branchRepository));
            _branchManager = branchManager ?? throw new ArgumentNullException(nameof(branchManager));
        }

        /// <summary>
        /// Returns a list of branches of companies
        /// </summary>
        /// <returns></returns>
        [HttpGet("branch", Name = "GetListBranches")]
        [ProducesResponseType(typeof(PagingResult<BranchModel>), (int)HttpStatusCode.OK /* 200 */)]
        [ProducesResponseType(typeof(ModelStateDictionary), (int)HttpStatusCode.BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized /* 401 */)]
        [SwaggerOperation(Tags = new[] { "Branch API" })]
        public async Task<IActionResult> Get(
            [FromQuery] BranchFilter filterModel,
            [FromQuery] Sortable sortable,
            [FromQuery] Paginable paginable)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = new PagingResult<BranchModel>();
            if (filterModel.BranchIds.Any())
            {
                return Ok(await _branchManager.GetBranchesById(filterModel.BranchIds));
            }

            result = await _branchManager.GetBranches(filterModel, sortable, paginable);

            return Ok(result);
        }

        /// <summary>
        /// Returns a list of branches of companies for trusted microservice by token
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("branch/{token}/trusted")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PagingResult<BranchModel>), (int)HttpStatusCode.OK /* 200 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Forbidden /* 403 */)]
        [SwaggerOperation(Tags = new[] { "Branch API" })]
        public async Task<IActionResult> Get([FromRoute] string token)
        {
            if (token != Consts.StaticToken)
            {
                return new ContentResult { StatusCode = 403 };
            }

            var result = await _branchManager.GetBranches();
            return Ok(result);
        }

        /// <summary>
        /// Returns a branch selected by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("branch/{id}", Name = "GetBranchDetails")]
        [ProducesResponseType(typeof(BranchModel), (int)HttpStatusCode.OK /* 200 */)]
        [ProducesResponseType(typeof(ModelStateDictionary), (int)HttpStatusCode.BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized /* 401 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound /* 404 */)]
        [SwaggerOperation(Tags = new[] { "Branch API" })]
        public async Task<IActionResult> Get([FromRoute]Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var branchDetail = await _branchManager.GetBranchById(id);
                if (branchDetail == null)
                {
                    return NotFound();
                }

                return Ok(branchDetail);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Returns all found branches related to the specific company
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="filterModel"></param>
        /// <param name="sortable"></param>
        /// <param name="paginable"></param>
        /// <returns></returns>
        /// <response code="200">List of branches related to the specific company</response>
        /// <response code="400">Some input params were wrong</response>
        /// <response code="404">Company not found</response>
        [HttpGet("company/{companyId}/branch")]
        [ProducesResponseType(typeof(IEnumerable<BranchModel>), (int)HttpStatusCode.OK /* 200 */)]
        [ProducesResponseType(typeof(ModelStateDictionary), (int)HttpStatusCode.BadRequest /* 400 */)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound /* 404 */)] // Company not found
        [SwaggerOperation(Tags = new[] { "Company API" })]
        public async Task<IActionResult> GetBranchesOfCompany(
            [FromRoute] int companyId,
            [FromQuery] BranchFilter filterModel,
            [FromQuery] Sortable sortable,
            [FromQuery] Paginable paginable)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(await _branchManager.GetBranchesByCompany(companyId, filterModel, sortable, paginable));
        }

        /// <summary>
        /// POST api/branches
        /// </summary>
        /// <param name="branchModel"></param>
        /// <returns></returns>
        [HttpPost("branch", Name = "CreateBranch")]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK /* 200 */)]
        [ProducesResponseType(typeof(ModelStateDictionary), (int)HttpStatusCode.BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized /* 401 */)]
        [SwaggerOperation(Tags = new[] { "Branch API" })]
        public async Task<IActionResult> Post([FromBody] CreateBranchModel branchModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var branchEntity = branchModel.Adapt<Branch>();
                Guid branchId = await _branchRepository.AddAsync(branchEntity);

                return Ok(branchId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// api/branch/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="branchModel"></param>
        [HttpPut("branch/{id}", Name = "UpdateBranch")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK /* 200 */)]
        [ProducesResponseType(typeof(ModelStateDictionary), (int)HttpStatusCode.BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized /* 401 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound /* 404 */)]
        [SwaggerOperation(Tags = new[] { "Branch API" })]
        public IActionResult Put([FromRoute]Guid id, [FromBody] CreateBranchModel branchModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var branchEntity = branchModel.Adapt<Branch>();
                branchEntity.Id = id;

                bool result = _branchRepository.Update(branchEntity);
                if (!result)
                {
                    return NotFound();
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Change status of branch
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isEnabled"></param>
        /// <returns></returns>
        [HttpPatch("branch/{id}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK /* 200 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized /* 401 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound /* 404 */)]
        [SwaggerOperation(Tags = new[] { "Branch API" })]
        public async Task<IActionResult> Patch([FromRoute] Guid id, [FromQuery] bool isEnabled)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool entityState = await _branchRepository.SetActivationState(id, isEnabled);
            if (!entityState)
            {
                return NotFound();
            }

            return Ok();
        }

        /// <summary>
        /// Get any branch by id or by name
        /// </summary>
        /// <param name="requestModel"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet("branch/{token}/trusted/internal", Name = "GetAnyBranchAsync")]
        [ProducesResponseType(typeof(BranchMin), (int)HttpStatusCode.OK /* 200 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized /* 401 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound /* 404 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Forbidden /* 403 */)]
        [SwaggerOperation(Tags = new[] { "Branch API" })]
        public async Task<IActionResult> GetAnyBranchAsync([FromQuery]BranchMinRequestModel requestModel, [FromRoute]string token)
        {
            if (token != Consts.StaticToken)
            {
                return new ContentResult { StatusCode = 403 };
            }

            try
            {
                var branchMinModel = await _branchManager.GetAnyBranchAsync(requestModel);
                return Ok(branchMinModel);
            }
            catch(KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _branchRepository.Dispose();
                _branchManager.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}

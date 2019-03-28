using System;
using Mapster;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Xyzies.TWC.Public.Api.Controllers.Http.Extentions;
using Xyzies.TWC.Public.Api.Managers.Interfaces;
using Xyzies.TWC.Public.Api.Models;
using Xyzies.TWC.Public.Data.Entities;
using Xyzies.TWC.Public.Data.Repositories.Interfaces;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.Annotations;

namespace Xyzies.TWC.Public.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/public-api")] // TODO: Find a better solution
    [ApiController]
    public class BranchController : Controller
    {
        private readonly ILogger<BranchController> _logger = null;
        private readonly IBranchRepository _branchRepository = null;
        private readonly IBranchManager _branchManager = null;

        /// <summary>
        /// 
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
        /// GET api/branches
        /// </summary>
        /// <returns></returns>
        [HttpGet("branch", Name = "GetListBranches")]
        [ProducesResponseType(typeof(PagingResult<BranchModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BadRequestResult), (int)HttpStatusCode.BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized /* 401 */)]
        [ProducesResponseType(typeof(NotFoundResult), (int)HttpStatusCode.NotFound /* 404 */)]
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
            if (filterModel.BranchIds.Count > 0)
            {
                return Ok(await _branchManager.GetBranchesById(filterModel.BranchIds));
            }
            //else if (filterModel.UserIds.Count > 0)
            //{
            //    return Ok(await _branchManager.GetBranchesByUser(filterModel.UserIds));
            //}
            else
            {
               result = await _branchManager.GetBranches(filterModel, sortable, paginable);
            }
            if (!result.Data.Any())
            {
                return NotFound();
            }

            return Ok(result);
        }

        /// <summary>
        /// GET api/branch/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("branch/{id}", Name = "GetBranchDetails")]
        [ProducesResponseType(typeof(BranchModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized /* 401 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound /* 404 */)]
        [SwaggerOperation(Tags = new[] { "Branch API" })]
        public async Task<IActionResult> Get([FromRoute]Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            BranchModel branchDetail = null;
            try
            {
                branchDetail = await _branchManager.GetBranchById(id);
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }

            if (branchDetail == null)
            {
                return NotFound();
            }

            return Ok(branchDetail);
        }

        /// <summary>
        /// Get all branches related to specify company
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="filterModel"></param>
        /// <param name="sortable"></param>
        /// <param name="paginable"></param>
        /// <returns></returns>
        [HttpGet("company/{companyId}/branch")]
        [ProducesResponseType(typeof(IEnumerable<BranchModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest /* 404 */)]
        [ProducesResponseType(typeof(NotFoundResult), (int)HttpStatusCode.NotFound /* 400 */)]
        [SwaggerOperation(Tags = new[] { "Company API" })]
        public async Task<IActionResult> GetBranchOfCompany(
            [FromRoute] int companyId,
            [FromQuery] BranchFilter filterModel,
            [FromQuery] Sortable sortable,
            [FromQuery] Paginable paginable)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _branchManager.GetBranchesByCompany(companyId, filterModel, sortable, paginable);

            if (result.Total.Equals(0))
            {
                return NotFound();
            }

            return Ok(result);

        }

        /// <summary>
        /// POST api/branches
        /// </summary>
        /// <param name="branchModel"></param>
        /// <returns></returns>
        [HttpPost("branch", Name = "CreateBranch")]
        [ProducesResponseType(typeof(CreatedResult), (int)HttpStatusCode.Created /* 201 */)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized /* 401 */)]
        [SwaggerOperation(Tags = new[] { "Branch API" })]
        public async Task<IActionResult> Post([FromBody] UploadBranchModel branchModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var branchEntity = branchModel.Adapt<Branch>();

            Guid branchId;
            try
            {
                branchId = await _branchRepository.AddAsync(branchEntity);
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(branchId);
        }

        /// <summary>
        /// api/branch/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="branchModel"></param>
        [HttpPut("branch/{id}", Name = "UpdateBranch")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK /* 200 */)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized /* 401 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound /* 404 */)]
        [SwaggerOperation(Tags = new[] { "Branch API" })]
        public IActionResult Put([FromRoute]Guid id, [FromBody] UploadBranchModel branchModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool result = false;
            var branchEntity = branchModel.Adapt<Branch>();
            branchEntity.Id = id;

            try
            {
                result = _branchRepository.Update(branchEntity);
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }

            if (result.Equals(false))
            {
                return NotFound($"Update failed. Branch with id={id} not found");
            }

            return Ok(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isEnabled"></param>
        /// <returns></returns>
        [HttpPatch("branch/{id}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
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

            // TODO: Refactoring
            var entityState = await _branchRepository.SetActivationState(id, isEnabled);

            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("branch/{id}")]
        [SwaggerOperation(Tags = new[] { "Branch API" })]
        public void Delete(int id)
        {
            throw new NotImplementedException();
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

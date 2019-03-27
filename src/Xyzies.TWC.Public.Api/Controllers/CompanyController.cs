using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using Xyzies.TWC.Public.Api.Controllers.Http.Extentions;
using Xyzies.TWC.Public.Api.Managers.Interfaces;
using Xyzies.TWC.Public.Api.Models;
using Xyzies.TWC.Public.Data.Entities;
using Xyzies.TWC.Public.Data.Repositories.Interfaces;

namespace Xyzies.TWC.Public.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/public-api/company")]
    [ApiController]
    public class CompanyController : Controller
    {
        private readonly ILogger<CompanyController> _logger = null;
        private readonly ICompanyRepository _companyRepository = null;
        private readonly ICompanyManager _companyManager = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="companyRepository"></param>
        /// <param name="companyManager"></param>
        public CompanyController(ILogger<CompanyController> logger,
            ICompanyRepository companyRepository,
            ICompanyManager companyManager)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
            _companyManager = companyManager ?? throw new ArgumentNullException(nameof(companyManager));
        }

        /// <summary>
        /// GET api/company
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetListCompanies")]
        [ProducesResponseType(typeof(IEnumerable<CompanyModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BadRequestResult), (int)HttpStatusCode.BadRequest /* 404 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized /* 401 */)]
        [ProducesResponseType(typeof(NotFoundResult), (int)HttpStatusCode.NotFound /* 400 */)]
        [SwaggerOperation(Tags = new[] { "Company API" })]
        public async Task<IActionResult> Get(
            [FromQuery] CompanyFilter filterModel,
            [FromQuery] Sortable sortable,
            [FromQuery] Paginable paginable)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            PagingResult<CompanyModel> result = new PagingResult<CompanyModel>();
            if (filterModel.CompanyIds != null)
            {
                return Ok(await _companyManager.GetCompanyNameById(filterModel.CompanyIds));
            }
            //TODO: Check this case.
             else
            {
                result = await _companyManager.GetCompanies(filterModel, sortable, paginable);
            }

            if (!result.Data.Any())
            {
                return NotFound();
            }

            return Ok(result);

        }

        /// <summary>
        /// GET api/company/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetCompanyDetails")]
        [ProducesResponseType(typeof(IEnumerable<CompanyModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized /* 401 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound /* 404 */)]
        [SwaggerOperation(Tags = new[] { "Company API" })]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            CompanyModel companyDetails = null;
            try
            {
                companyDetails = await _companyManager.GetCompanyById(id);
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }

            if (companyDetails == null)
            {
                return NotFound();
            }

            return Ok(companyDetails);
        }

        /// <summary>
        /// POST api/company
        /// </summary>
        /// <param name="companyModel"></param>
        [HttpPost(Name = "CreateNewCompany")]
        [ProducesResponseType(typeof(CreatedResult), (int)HttpStatusCode.Created /* 201 */)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized /* 401 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound /* 404 */)]
        [SwaggerOperation(Tags = new[] { "Company API" })]
        public async Task<IActionResult> Post([FromBody] UploadCompanyModel companyModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var companyEntity = companyModel.Adapt<Company>();

            int companyId;
            try
            {
                companyId = await _companyRepository.AddAsync(companyEntity);
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(companyId);
        }

        /// <summary>
        /// Update a company
        /// </summary>
        /// <param name="id"></param>
        /// <param name="companyModel"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized /* 401 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound /* 404 */)]
        [SwaggerOperation(Tags = new[] { "Company API" })]
        public IActionResult Put([FromRoute]int id, [FromBody] UploadCompanyModel companyModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool result = false;
            var companyEntity = companyModel.Adapt<Company>();
            companyEntity.Id = id;

            try
            {
                result = _companyRepository.Update(companyEntity);
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }

            if (result.Equals(false))
            {
                return NotFound($"Update failed. Company with id={id} not found");
            }

            return Ok(result);
        }

        /// <summary>
        /// Set enable/disable state for company
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isEnabled"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized /* 401 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent /* 404 */)]
        [SwaggerOperation(Tags = new[] { "Company API" })]
        public async Task<IActionResult> Patch([FromRoute]int id, [FromQuery] bool isEnabled)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool result = await _companyRepository.SetActivationState(id, isEnabled);
            if (result)
            {
                return Ok();
            }

            return NotFound();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        [SwaggerOperation(Tags = new[] { "Company API" })]
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // TODO: Disposing
                _companyRepository.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}

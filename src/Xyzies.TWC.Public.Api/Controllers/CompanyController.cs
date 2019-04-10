using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using Xyzies.TWC.Public.Api.Controllers.Http.Extentions;
using Xyzies.TWC.Public.Api.Managers;
using Xyzies.TWC.Public.Api.Managers.Interfaces;
using Xyzies.TWC.Public.Api.Models;
using Xyzies.TWC.Public.Data.Entities;
using Xyzies.TWC.Public.Data.Entities.Azure;
using Xyzies.TWC.Public.Data.Repositories.Interfaces;

namespace Xyzies.TWC.Public.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("company")]
    [ApiController]
    public class CompanyController : Controller
    {
        private readonly ILogger<CompanyController> _logger = null;
        private readonly ICompanyRepository _companyRepository = null;
        private readonly ICompanyManager _companyManager = null;
        private readonly ICompanyAvatarsManager _companyAvatarsManager = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="companyRepository"></param>
        /// <param name="companyManager"></param>
        /// <param name="companyAvatarsManager"></param>
        public CompanyController(ILogger<CompanyController> logger,
            ICompanyRepository companyRepository,
            ICompanyManager companyManager,
            ICompanyAvatarsManager companyAvatarsManager)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
            _companyManager = companyManager ?? throw new ArgumentNullException(nameof(companyManager));
            _companyAvatarsManager = companyAvatarsManager ?? throw new ArgumentNullException(nameof(companyAvatarsManager));
        }

        /// <summary>
        /// GET api/company
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetListCompanies")]
        [ProducesResponseType(typeof(PagingResult<CompanyModel>), (int)HttpStatusCode.OK) /* 200 */]
        [ProducesResponseType(typeof(ModelStateDictionary), (int)HttpStatusCode.BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized /* 401 */)]
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

            var result = new PagingResult<CompanyModel>();
            if (filterModel.CompanyIds.Any())
            {
                return Ok(await _companyManager.GetCompanyNameById(filterModel.CompanyIds));
            }

            result = await _companyManager.GetCompanies(filterModel, sortable, paginable);

            return Ok(result);
        }

        /// <summary>
        /// GET api/company/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetCompanyDetails")]
        [ProducesResponseType(typeof(CompanyModel), (int)HttpStatusCode.OK /* 200 */)]
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

            try
            {
                CompanyModel companyDetails = await _companyManager.GetCompanyById(id);
                if (companyDetails == null)
                {
                    return NotFound();
                }

                return Ok(companyDetails);
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
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
        public async Task<IActionResult> Post([FromBody] CreateCompanyModel companyModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var companyEntity = companyModel.Adapt<Company>();
                int companyId = await _companyRepository.AddAsync(companyEntity);

                return Ok(companyId);
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update a company
        /// </summary>
        /// <param name="id"></param>
        /// <param name="companyModel"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(IActionResult), (int)HttpStatusCode.OK /* 200 */)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized /* 401 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound /* 404 */)]
        [SwaggerOperation(Tags = new[] { "Company API" })]
        public IActionResult Put([FromRoute]int id, [FromBody] CreateCompanyModel companyModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var companyEntity = companyModel.Adapt<Company>();
                companyEntity.Id = id;

                bool result = _companyRepository.Update(companyEntity);
                if (!result)
                {
                    return NotFound();
                }

                return Ok();
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Set enable/disable state for company
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isEnabled"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK /* 200 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized /* 401 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound /* 404 */)]
        [SwaggerOperation(Tags = new[] { "Company API" })]
        public async Task<IActionResult> Patch([FromRoute]int id, [FromQuery] bool isEnabled)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool result = await _companyRepository.SetActivationState(id, isEnabled);
            if (!result)
            {
                return NotFound();
            }

            return Ok();
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

        /// <summary>
        /// Updates company avatar
        /// </summary>
        /// <param name="companyId">company id to update avatar</param>
        /// <param name="avatar">Avatar file</param>
        /// <returns></returns>
        [HttpPut("{companyId}/avatar")]
        [SwaggerOperation(Tags = new[] { "Company API" })]
        public async Task<IActionResult> UpdateCompanyAvatar(string companyId, [FromForm] [Required] AvatarModel avatar)
        {
            var result = await _companyAvatarsManager.UploadCompanyAvatarAsync(new CompanyAvatar { File = avatar.File, Id = companyId });

            return result ? Ok() : throw new ApplicationException("result is false");
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _companyManager.Dispose();
                _companyRepository.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}

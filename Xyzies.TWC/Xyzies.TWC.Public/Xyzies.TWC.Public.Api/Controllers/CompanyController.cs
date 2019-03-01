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
    [Route("api/company")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepository = null;
        private readonly ILogger<CompanyController> _logger = null;
        private readonly ICompanyManager _companyManager = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="companyRepository"></param>
        /// <param name="companyManager"></param>
        public CompanyController(ILogger<CompanyController> logger,
            ICompanyRepository companyRepository, ICompanyManager companyManager)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
            _companyManager = companyManager;
        }

        /// <summary>
        /// GET api/company
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetListCompanies")]
        [ProducesResponseType(typeof(IEnumerable<CompanyModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized /* 401 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound /* 404 */)]
        public async Task<IActionResult> Get(
            [FromQuery] Filter filterModel,
            [FromQuery] Sortable sortable,
            [FromQuery] Paginable paginable)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _companyManager.GetCompanies(filterModel, sortable, paginable);
            if (!result.Data.Any())
            {
                return NoContent();
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
        public async Task<IActionResult> Get(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Company companyDetails = null;
            try
            {
                companyDetails = await _companyRepository.GetAsync(id);
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }

            if (companyDetails == null)
            {
                return NotFound();
            }
            var companyDetailModel = companyDetails.Adapt<CompanyModel>();
            return Ok(companyDetails);
        }

        /// <summary>
        /// // POST api/company
        /// </summary>
        /// <param name="companyModel"></param>
        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<CompanyModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized /* 401 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound /* 404 */)]
        public IActionResult Post([FromBody] UploadCompanyModel companyModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var companyEntity = companyModel.Adapt<Company>();

            int companyId;
            try
            {
                companyId = _companyRepository.Add(companyEntity);
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(companyId);
        }

        /// <summary>
        /// // PUT api/company/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="companyModel"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized /* 401 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound /* 404 */)]
        public IActionResult Put(int id, [FromBody] CompanyModel companyModel)
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

        // DELETE api/company/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

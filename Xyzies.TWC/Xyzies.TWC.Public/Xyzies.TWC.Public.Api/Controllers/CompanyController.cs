using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="companyRepository"></param>
        public CompanyController(ILogger<CompanyController> logger,
            ICompanyRepository companyRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
        }

        /// <summary>
        /// GET api/company
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpHead]
        [ProducesResponseType(typeof(IEnumerable<CompanyModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized /* 401 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound /* 404 */)]
        public async Task<IActionResult> Get()
        {
            var companies = new List<Company>();
            try
            {
                companies = (await _companyRepository.GetAsync())?.ToList();
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }

            if (companies.Count.Equals(0))
            {
                return NotFound();
            }

            return Ok(companies);
        }

        /// <summary>
        /// GET api/company/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
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

            return Ok(companyDetails);
        }

        // POST api/company
        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<CompanyModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized /* 401 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound /* 404 */)]
        public void Post([FromBody] string value)
        {

        }

        // PUT api/company/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(IEnumerable<CompanyModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized /* 401 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound /* 404 */)]
        public void Put(int id, [FromBody] string value)
        {

        }

        // DELETE api/company/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

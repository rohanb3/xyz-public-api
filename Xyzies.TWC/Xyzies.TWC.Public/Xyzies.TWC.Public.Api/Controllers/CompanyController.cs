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

        /// <summary>
        /// // POST api/company
        /// </summary>
        /// <param name="companyModel"></param>
        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<CompanyModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized /* 401 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound /* 404 */)]
        public  IActionResult Post([FromBody] CompanyModel companyModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var company = new Company()
            {
                CompanyName = companyModel.CompanyName,
                LegalName = companyModel.LegalName,
                Email = companyModel.Email,
                Phone = companyModel.Phone,
                Address = companyModel.Address,
                City = companyModel.City,
                State = companyModel.State,
                ZipCode = companyModel.ZipCode,
                 StoreID = companyModel.StoreID,
                 CreatedDate = companyModel.CreatedDate,
                 ModifiedDate = companyModel.ModifiedDate,
                 CreatedBy = companyModel.CreatedBy,
                 ModifiedBy = companyModel.ModifiedBy,
                 Agentid = companyModel.Agentid,
                 Status = companyModel.Status,
                 StoreLocationCount = companyModel.StoreLocationCount,
                PrimaryContactName = companyModel.PrimaryContactName,
                PrimaryContactTitle = companyModel.PrimaryContactTitle,
                Fax = companyModel.Fax,
                FedId = companyModel.FedId,
                 TypeOfCompany = companyModel.TypeOfCompany,
                StateEstablished = companyModel.StateEstablished,
                 CompanyType = companyModel.CompanyType,
                CallerId = companyModel.CallerId,
                 IsAgreement = companyModel.IsAgreement,
                ActivityStatus = companyModel.ActivityStatus,
                 CompanyKey = companyModel.CompanyKey,
                FirstName = companyModel.FirstName,
                LastName = companyModel.LastName,
                CellNumber = companyModel.CellNumber,
                BankNumber = companyModel.BankNumber,
                BankName = companyModel.BankName,
                BankAccountNumber = companyModel.BankAccountNumber,
                XyziesId = companyModel.XyziesId,
                 ApprovedDate = companyModel.ApprovedDate,
                 BankInfoGiven = companyModel.BankInfoGiven,
                 AccountManager = companyModel.AccountManager,
                CrmCompanyId = companyModel.CrmCompanyId,
                 IsCallCenter = companyModel.IsCallCenter,
                 ParentCompanyId = companyModel.ParentCompanyId,
                 TeamKey = companyModel.TeamKey,
                 RetailerGroupKey = companyModel.RetailerGroupKey,
                SocialMediaAccount = companyModel.SocialMediaAccount,
                RetailerGoogleAccount = companyModel.RetailerGoogleAccount,
                 PaymentMode = companyModel.PaymentMode,
                 CustomerDemographicId = companyModel.CustomerDemographicId,
                 LocationTypeId = companyModel.LocationTypeId,
                 IsOwnerPassBackground = companyModel.IsOwnerPassBackground,
                 IsWebsite = companyModel.IsWebsite,
                 IsSellsLifelineWireless = companyModel.IsSellsLifelineWireless,
                 NumberofStores = companyModel.NumberofStores,
                BusinessDescription = companyModel.BusinessDescription,
                WebsiteList = companyModel.WebsiteList,
                 IsSpectrum = companyModel.IsSpectrum,
                 BusinessSource = companyModel.BusinessSource,
                GeoLat = companyModel.GeoLat,
                GeoLon = companyModel.GeoLon,
                 IsMarketPlace = companyModel.IsMarketPlace,
                MarketPlaceName = companyModel.MarketPlaceName,
                PhysicalName = companyModel.PhysicalName,
                MarketStrategy = companyModel.MarketStrategy,
                 NoSyncInfusion = companyModel.NoSyncInfusion,
                StorePhoneNumber = companyModel.StorePhoneNumber,
                 ReferralUserId = companyModel.ReferralUserId,
                 CompanyStatusKey = companyModel.CompanyStatusKey,
                 CompanyStatusChangedOn = companyModel.CompanyStatusChangedOn,
                 CompanyStatusChangedBy = companyModel.CompanyStatusChangedBy
            };

            int companyId;
            try
            {
                companyId = _companyRepository.Add(company);
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(companyId);
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

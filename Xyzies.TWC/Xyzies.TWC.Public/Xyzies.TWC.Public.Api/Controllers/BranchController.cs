using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xyzies.TWC.Public.Api.Models;
using Xyzies.TWC.Public.Data.Entities;
using Xyzies.TWC.Public.Data.Enums;
using Xyzies.TWC.Public.Data.Repositories.Interfaces;

namespace Xyzies.TWC.Public.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/branch")]
    [ApiController]
    public class BranchController : ControllerBase
    {
        private readonly IBranchRepository _branchRepository = null;
        private readonly ILogger<BranchController> _logger = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="branchRepository"></param>
        public BranchController(ILogger<BranchController> logger,
            IBranchRepository branchRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _branchRepository = branchRepository ?? throw new ArgumentNullException(nameof(branchRepository));
        }

        /// <summary>
        /// GET api/branches
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BranchModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized /* 401 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound /* 404 */)]
        [ProducesResponseType(typeof(IEnumerable<BranchModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var branches = new List<Branch>();
            try
            {
                branches = (await _branchRepository.GetAsync())?.ToList();
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }

            if (branches.Count.Equals(0))
            {
                return NotFound();
            }
            var results = new List<BranchModel>();

            foreach (var branch in branches)
            {
                results.Add(new BranchModel
                {
                    BranchName = branch.BranchName,
                    Email = branch.Email,
                    Phone = branch.Phone,
                    Fax = branch.Fax,
                    Address = branch.Address,
                    City = branch.City,
                    ZipCode = branch.ZipCode,
                    GeoLat = branch.GeoLat,
                    GeoLon = branch.GeoLon,
                    Status = Enum.GetName(typeof(Status), branch.Status),
                    State = branch.State,
                    CreatedDate = branch.CreatedDate,
                    ModifiedDate = branch.ModifiedDate,
                    CreatedBy = branch.CreatedBy,
                    ModifiedBy = branch.ModifiedBy,
                });
            }

            return Ok(branches);
        }

        /// <summary>
        /// GET api/branch/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BranchModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized /* 401 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound /* 404 */)]
        public async Task<IActionResult> Get(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Branch branchDetails = null;
            try
            {
                branchDetails = await _branchRepository.GetAsync(id);
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }

            if (branchDetails == null)
            {
                return NotFound();
            }

            var branchModel = new BranchModel
            {
                BranchName = branchDetails.BranchName,
                Email = branchDetails.Email,
                Phone = branchDetails.Phone,
                Fax = branchDetails.Fax,
                Address = branchDetails.Address,
                City = branchDetails.City,
                ZipCode = branchDetails.ZipCode,
                GeoLat = branchDetails.GeoLat,
                GeoLon = branchDetails.GeoLon,
                Status = Enum.GetName(typeof(Status), branchDetails.Status),
                State = branchDetails.State,
                CreatedDate = branchDetails.CreatedDate,
                ModifiedDate = branchDetails.ModifiedDate,
                CreatedBy = branchDetails.CreatedBy,
                ModifiedBy = branchDetails.ModifiedBy,

                BranchContacts = branchDetails.BranchContacts
            };

            return Ok(branchDetails);
        }

        /// <summary>
        /// POST api/branch
        /// </summary>
        /// <param name="branch"></param>
        [HttpPost]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Created /* 201 */)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized /* 401 */)]
        public async Task<IActionResult> Post([FromBody] BranchModel branchModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var branch = new Branch
            {
                BranchName = branchModel.BranchName,
                Email = branchModel.Email,
                Phone = branchModel.Phone,
                Fax = branchModel.Fax,
                Address = branchModel.Address,
                City = branchModel.City,
                ZipCode = branchModel.ZipCode,
                GeoLat = branchModel.GeoLat,
                GeoLon = branchModel.GeoLon,
                Status = (int)Enum.Parse(typeof(Status), branchModel.Status),
                State = branchModel.State,
                CreatedDate = branchModel.CreatedDate,
                ModifiedDate = branchModel.ModifiedDate,
                CreatedBy = branchModel.CreatedBy,
                ModifiedBy = branchModel.ModifiedBy,
            };
            int branchId;
            try
            {
                branchId = _branchRepository.Add(branch);
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(branchId);
        }

        /// <summary>
        /// PUT api/branch/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [HttpPut("{id}", Name = "UpdateBranch")]
        //[ProducesResponseType(typeof(ApiResponse<bool>), (int)HttpStatusCode.OK /* 200 */)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest /* 400 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized /* 401 */)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound /* 404 */)]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/branch/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xyzies.TWC.Public.Api.Models;
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
        public async Task<IActionResult> Get()
        {
            var branches = (await _branchRepository.GetAsync())?.ToList();
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
                    Status = branch.Status,
                    State = branch.State
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
        [ProducesResponseType(typeof(IEnumerable<BranchModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(int id)
        {
            var branch = (await _branchRepository.GetAsync(id));
            var branchModel = new BranchModel
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
                Status = branch.Status,
                State = branch.State
            };

            //branchModel.BranchContact = (await _branchContactRepository.GetAsync(branchModel.BranchId));


            return Ok(branch);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

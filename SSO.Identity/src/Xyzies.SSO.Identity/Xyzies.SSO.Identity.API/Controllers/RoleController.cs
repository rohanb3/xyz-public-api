using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Xyzies.SSO.Identity.API.Models;
using Xyzies.SSO.Identity.Data.Repository;

namespace Xyzies.SSO.Identity.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/role")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository = null;
        private readonly ILogger<RoleController> _logger = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="roleRepository"></param>
        public RoleController(ILogger<RoleController> logger,
            IRoleRepository roleRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpHead]
        [ProducesResponseType(typeof(IEnumerable<RoleModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var roles = (await _roleRepository.GetAsync()).ToList();
            var results = new List<RoleModel>();

            // TODO: Add mapper
            foreach (var role in roles)
            {
                var roleModel = new RoleModel
                {
                    RoleKey = role.Id,
                    RoleId = role.RoleId.Value,
                    RoleName = role.RoleName,
                    IsCustomRole = role.IsCustom
                };

                foreach (var policy in role.Policies)
                {
                    var policyModel = new PolicyModel
                    {
                        PolicyId = policy.Id,
                        PolicyName = policy.Name
                    };

                    foreach (var permission in policy.Permissions)
                    {
                        policyModel.Scopes.Add(new ScopeModel
                        {
                            ScopeId = permission.Id,
                            ScopeName = permission.Scope
                        });
                    }

                    roleModel.Policies.Add(policyModel);
                }

                results.Add(roleModel);
            }

            return Ok(results);
        }
    }
}
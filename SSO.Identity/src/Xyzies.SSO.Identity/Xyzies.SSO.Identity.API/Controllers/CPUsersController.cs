using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Xyzies.SSO.Identity.Services.Service;

namespace Xyzies.SSO.Identity.API.Controllers
{
    [Route("api/cpusers")]
    [ApiController]
    public class CpUsersController : ControllerBase
    {
        private readonly ICpUsersService _users;

        public CpUsersController(ICpUsersService users)
        {
            _users = users;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _users.GetAllCpUsers();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var users = await _users.GetUserById(id);
            return Ok(users);
        }
    }
}
using System.Threading.Tasks;
using graphApiService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace graphApiService.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IGraphClientService _graphClient;
        public ValuesController(IGraphClientService graphClient)
        {
            _graphClient = graphClient;
        }
        // GET api/values
        [AllowAnonymous]
        [HttpGet]
        public ActionResult<string> Get()
        {
           string result = _graphClient.GetAllUsers();
            return result;
        }
        // GET api/values/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<string>> Get(int id)
        {
            await _graphClient.CreateUserAsync();
            return "done";
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

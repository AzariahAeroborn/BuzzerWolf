using BuzzerWolf.BBAPI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BuzzerWolf.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private IBBAPIClient _client;
        public TeamController(IBBAPIClient client)
        {
            _client = client;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            await _client.GetTeam();
            return Ok();
        }
    }
}

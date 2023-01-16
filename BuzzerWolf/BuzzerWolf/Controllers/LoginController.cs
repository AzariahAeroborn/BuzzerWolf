using BuzzerWolf.BBAPI;
using BuzzerWolf.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BuzzerWolf.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class LoginController : ControllerBase
    {
        private IBBAPIClient _client;
        public LoginController(IBBAPIClient client) 
        { 
            _client = client;
        }

        [HttpGet]
        public ActionResult Get()
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.AccessKey))
            {
                return BadRequest(new { Details = "You must supply a User Name and Access Key to log in" });
            }

            return await _client.Login(request.UserName, request.AccessKey);
        }
    }
}

using BuzzerWolf.BBAPI;
using BuzzerWolf.BBAPI.Exceptions;
using BuzzerWolf.Models;
using Microsoft.AspNetCore.Mvc;

namespace BuzzerWolf.Controllers
{
    public class LoginController : Controller
    {
        private readonly IBBAPIClient _client;
        private readonly ILogger<LoginController> _logger;

        public LoginController(IBBAPIClient client, ILogger<LoginController> logger) 
        { 
            _client = client;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel request)
        {
            if (string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.AccessKey))
            {
                return BadRequest(new { Details = "You must supply a User Name and Access Key to log in" });
            }

            try
            {
                HttpContext.Response.Cookies.Append(".ASPXAUTH", await _client.Login(request.UserName, request.AccessKey));
                return RedirectToAction("Index", "Team");
            }
            catch (UnauthorizedException)
            {
                return Unauthorized("Invalid username or BBAPI Access Key provided.  Be sure to provide the BuzzerBeater Access Key set under Preferences > Create/Change Access Key and not the password for logging into BuzzerBeater.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

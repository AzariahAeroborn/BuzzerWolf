using BuzzerWolf.BBAPI;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BuzzerWolf.Server.Controllers
{
    [ApiController]
    [Route("login")]
    public class LoginController : Controller
    {
        private readonly IBBAPIClient _bbapi;
        private readonly ILogger<LoginController> _logger;
        public LoginController(IBBAPIClient bbapi, ILogger<LoginController> logger)
        {
            _bbapi = bbapi;
            _logger = logger;
        }

        [HttpPost()]
        public async Task<IActionResult> Index(string userName, string accessKey, bool secondTeam)
        {
            if (await _bbapi.Login(userName, accessKey, secondTeam))
            {
                var teamInfo = await _bbapi.GetTeamInfo();
                var claims = new List<Claim>
                {
                    new Claim("userName", userName),
                    new Claim("accessKey", accessKey),
                    new Claim("secondTeam", secondTeam.ToString()),
                    new Claim("teamId", teamInfo.Id.ToString()),
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    IssuedUtc = DateTimeOffset.UtcNow,
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}

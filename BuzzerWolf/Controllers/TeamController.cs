using BuzzerWolf.BBAPI;
using Microsoft.AspNetCore.Mvc;

namespace BuzzerWolf.Controllers
{
    public class TeamController : Controller
    {
        private IBBAPIClient _client;
        private readonly ILogger<TeamController> _logger;

        public TeamController(IBBAPIClient client, ILogger<TeamController> logger)
        {
            _client = client;
            _logger = logger;

        }

        [HttpGet]
        public async Task<IActionResult> Index(int? id)
        {
            return View(await _client.GetTeam(id));
        }
    }
}

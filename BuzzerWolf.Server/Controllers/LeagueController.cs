using BuzzerWolf.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BuzzerWolf.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/leagues")]
    public class LeagueController(IBBDataService dataService, ILogger<AutoPromotionController> logger) : Controller
    {
        [HttpGet()]
        public async Task<List<League>> Index()
        {
            return await dataService.GetLeagueList();
        }
    }
}

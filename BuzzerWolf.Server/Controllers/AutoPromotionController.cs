using BuzzerWolf.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BuzzerWolf.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/autopromotion")]
    public class AutoPromotionController(IBBDataService dataService, ILogger<AutoPromotionController> logger) : Controller
    {
        [HttpGet()]
        public async Task<List<PromotionStanding>> Index(int country, int division, int? season = null)
        {
            return await dataService.GetPromotionStandings(country, division, season);
        }
    }
}

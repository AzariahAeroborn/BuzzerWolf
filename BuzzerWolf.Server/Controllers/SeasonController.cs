using BuzzerWolf.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BuzzerWolf.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/season")]
    public class SeasonController(IBBDataService dataService, ILogger<AutoPromotionController> logger) : Controller
    {
        [HttpGet()]
        public async Task<List<Season>> Index()
        {
            return await dataService.GetSeasonList();
        }

        [HttpGet()]
        [Route("current")]
        public async Task<Season> GetCurrentSeason()
        {
            return await dataService.GetCurrentSeason();
        }
    }
}

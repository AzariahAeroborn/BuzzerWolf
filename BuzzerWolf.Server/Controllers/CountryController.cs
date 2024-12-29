using BuzzerWolf.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BuzzerWolf.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/country")]
    public class CountryController(IBBDataService dataService, ILogger<AutoPromotionController> logger) : Controller
    {
        [HttpGet()]
        public async Task<List<Country>> Index()
        {
            return await dataService.GetCountryList();
        }
    }
}

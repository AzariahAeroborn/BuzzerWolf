using BuzzerWolf.Server.Models;

namespace BuzzerWolf.Server
{
    public interface IBBDataService
    {
        Task<Season> GetCurrentSeason();
        Task<List<Season>> GetSeasonList();
        Task<List<Country>> GetCountryList();
        Task<List<PromotionStanding>> GetPromotionStandings(int country, int division, int? season = null);
    }
}

using BuzzerWolf.Server.Models;

namespace BuzzerWolf.Server
{
    public interface IBBDataService
    {
        Task<Season> GetCurrentSeason(bool syncRequested = false);
        Task<List<Season>> GetSeasonList(bool syncRequested = false);
        Task<List<Country>> GetCountryList(bool syncRequested = false);
        Task<List<League>> GetLeagueList(bool syncRequested = false);
        Task<List<PromotionStanding>> GetPromotionStandings(int country, int division, int? season = null, bool syncRequested = false);
    }
}

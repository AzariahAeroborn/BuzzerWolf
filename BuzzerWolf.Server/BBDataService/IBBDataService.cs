using BuzzerWolf.Server.Models;

namespace BuzzerWolf.Server
{
    public interface IBBDataService
    {
        Task<int> GetCurrentSeason();
        Task<List<Season>> GetSeasonList();
        Task<List<PromotionStanding>> GetPromotionStandings(int country, int division, int? season = null);
    }
}

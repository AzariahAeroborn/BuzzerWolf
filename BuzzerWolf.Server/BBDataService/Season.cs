using BuzzerWolf.Server.Models;
using BuzzerWolf.Server.Models.Extensions;

namespace BuzzerWolf.Server
{
    public partial class BBDataService : IBBDataService
    {
        public async Task<int> GetCurrentSeason()
        {
            var seasonList = await bbapi.GetSeasons();
            return seasonList.Seasons.OrderBy(s => s.Id).Last().Id;
        }

        public async Task<List<Season>> GetSeasonList()
        {
            var seasonList = await bbapi.GetSeasons();
            return seasonList.Seasons.FromBBAPI().ToList();
        }
    }
}

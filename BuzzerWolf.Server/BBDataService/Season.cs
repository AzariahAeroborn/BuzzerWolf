using BuzzerWolf.Server.Models;
using BuzzerWolf.Server.Models.Extensions;
using Microsoft.EntityFrameworkCore;

namespace BuzzerWolf.Server
{
    public partial class BBDataService : IBBDataService
    {
        private readonly int seasonLength = 14 * 7;
        private static readonly SemaphoreSlim syncSeason = new(1);

        private async Task SynchronizeSeasons()
        {
            syncSeason.Wait();
            try
            {
                var seasonSyncRecord = await GetTableSyncRecord(SyncTable.Season);
                if (seasonSyncRecord.NextAutoSync <= DateTimeOffset.UtcNow)
                {
                    var seasonList = await bbapi.GetSeasons();
                    context.Seasons.UpsertRange(seasonList.Seasons.FromBBAPI()).Run();
                    await UpdateTableSyncRecord(seasonSyncRecord, seasonList.Seasons.Last().Start.AddDays(seasonLength));
                }
            }
            finally { syncSeason.Release(); }
        }

        public async Task<Season> GetCurrentSeason()
        {
            await SynchronizeSeasons();
            return context.Seasons.OrderBy(s => s.Id).Last();
        }

        public async Task<List<Season>> GetSeasonList()
        {
            await SynchronizeSeasons();
            return await context.Seasons.ToListAsync();
        }
    }
}

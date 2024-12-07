using BuzzerWolf.Server.Models;
using BuzzerWolf.Server.Models.Extensions;
using Microsoft.EntityFrameworkCore;

namespace BuzzerWolf.Server
{
    public partial class BBDataService : IBBDataService
    {
        private readonly int seasonLength = 14 * 7;

        private async Task SynchronizeSeasons(bool syncRequested)
        {
            var syncEntity = new SynchronizedEntity(SyncTable.Season);
            if (!await ShouldSync(syncEntity, syncRequested))
                return;

            var syncLock = SyncLockDictionary.GetOrAdd(syncEntity, new SemaphoreSlim(1));
            await syncLock.WaitAsync();
            try
            {
                if (await ShouldSync(syncEntity, syncRequested))
                {
                    var seasonList = await bbapi.GetSeasons();
                    var context = serviceProvider.GetRequiredService<BuzzerWolfContext>();
                    await context.Seasons.UpsertRange(seasonList.Seasons.FromBBAPI()).RunAsync();
                    await UpdateSyncRecord(syncEntity, seasonList.Seasons.Last().Start.AddDays(seasonLength));
                }
            }
            finally { syncLock.Release(); }
        }

        public async Task<Season> GetCurrentSeason(bool syncRequested = false)
        {
            await SynchronizeSeasons(syncRequested);
            var context = serviceProvider.GetRequiredService<BuzzerWolfContext>();
            return context.Seasons.OrderBy(s => s.Id).Last();
        }

        public async Task<List<Season>> GetSeasonList(bool syncRequested = false)
        {
            await SynchronizeSeasons(syncRequested);
            var context = serviceProvider.GetRequiredService<BuzzerWolfContext>();
            return await context.Seasons.ToListAsync();
        }
    }
}

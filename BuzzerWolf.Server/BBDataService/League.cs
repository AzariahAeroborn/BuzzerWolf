using BuzzerWolf.Server.Models.Extensions;
using BuzzerWolf.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace BuzzerWolf.Server
{
    public partial class BBDataService : IBBDataService
    {
        private async Task SynchronizeLeagues(bool syncRequested)
        {
            var syncEntity = new SynchronizedEntity(SyncTable.League);
            if (!await ShouldSync(syncEntity, syncRequested))
                return;

            var syncLock = SyncLockDictionary.GetOrAdd(syncEntity, new SemaphoreSlim(1));
            await syncLock.WaitAsync();
            try
            {
                if (await ShouldSync(syncEntity, syncRequested))
                {
                    var leagueSyncTasks = new List<Task<List<BBAPI.Model.League>>>();
                    var context = serviceProvider.GetRequiredService<BuzzerWolfContext>();
                    foreach (var country in context.Countries)
                    {
                        for (int level = 1; level <= (country.Divisions ?? 1); level++)
                        {
                            leagueSyncTasks.Add(bbapi.GetLeagues(country.Id, level));
                        }
                    }

                    var leagueLists = await Task.WhenAll(leagueSyncTasks);
                    await context.Leagues.UpsertRange(leagueLists.SelectMany(ll => ll).FromBBAPI()).RunAsync();
                    var currentSeason = await this.GetCurrentSeason();
                    var nextSyncTime = currentSeason?.Start.AddDays(seasonLength) ?? DateTimeOffset.UtcNow.AddDays(7);
                    await UpdateSyncRecord(syncEntity, nextSyncTime);
                }
            }
            finally { syncLock.Release(); }
        }

        public async Task<List<League>> GetLeagueList(bool syncRequested = false)
        {
            await SynchronizeLeagues(syncRequested);
            var context = serviceProvider.GetRequiredService<BuzzerWolfContext>();
            return await context.Leagues.ToListAsync();
        }

        public async Task<List<League>> GetLeagues(int countryId, int level, bool syncRequested = false)
        {
            await SynchronizeLeagues(syncRequested);
            var context = serviceProvider.GetRequiredService<BuzzerWolfContext>();
            return await context.Leagues.Include(l => l.Country).Where(l => l.CountryId == countryId && l.DivisionLevel == level).ToListAsync();
        }
    }
}

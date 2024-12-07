using BuzzerWolf.Server.Models;
using BuzzerWolf.Server.Models.Extensions;
using Microsoft.EntityFrameworkCore;

namespace BuzzerWolf.Server
{
    public partial class BBDataService : IBBDataService
    {
        private async Task SynchronizeCountry(bool syncRequested)
        {
            var syncEntity = new SynchronizedEntity(SyncTable.Country);
            if (!await ShouldSync(syncEntity, syncRequested))
                return;

            var syncLock = SyncLockDictionary.GetOrAdd(syncEntity, new SemaphoreSlim(1));
            await syncLock.WaitAsync();
            try
            {
                if (await ShouldSync(syncEntity, syncRequested))
                {
                    var countryList = await bbapi.GetCountries();
                    var context = serviceProvider.GetRequiredService<BuzzerWolfContext>();
                    await context.Countries.UpsertRange(countryList.FromBBAPI()).RunAsync();
                    var currentSeason = await this.GetCurrentSeason();
                    var nextSyncTime = currentSeason?.Start.AddDays(seasonLength) ?? DateTimeOffset.UtcNow.AddDays(7);
                    await UpdateSyncRecord(syncEntity, nextSyncTime);
                }
            }
            finally { syncLock.Release(); }
        }

        public async Task<List<Country>> GetCountryList(bool syncRequested = false)
        {
            await SynchronizeCountry(syncRequested);
            var context = serviceProvider.GetRequiredService<BuzzerWolfContext>();
            return await context.Countries.ToListAsync();
        }
    }
}

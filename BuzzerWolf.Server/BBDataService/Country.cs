using BuzzerWolf.Server.Models;
using BuzzerWolf.Server.Models.Extensions;
using Microsoft.EntityFrameworkCore;

namespace BuzzerWolf.Server
{
    public partial class BBDataService : IBBDataService
    {
        private static readonly SemaphoreSlim syncCountry = new(1);
        private async Task SynchronizeCountry()
        {
            syncCountry.Wait();
            try
            {
                var countrySyncRecord = await GetTableSyncRecord(SyncTable.Country);
                if (countrySyncRecord.NextAutoSync <= DateTimeOffset.UtcNow)
                {
                    var countryList = await bbapi.GetCountries();
                    context.Countries.UpsertRange(countryList.FromBBAPI()).Run();
                    var currentSeason = await this.GetCurrentSeason();
                    await UpdateTableSyncRecord(countrySyncRecord, currentSeason?.Start.AddDays(seasonLength) ?? DateTimeOffset.UtcNow.AddDays(7));
                }
            }
            finally { syncCountry.Release(); }
        }

        public async Task<List<Country>> GetCountryList()
        {
            await SynchronizeCountry();
            return await context.Countries.ToListAsync();
        }
    }
}

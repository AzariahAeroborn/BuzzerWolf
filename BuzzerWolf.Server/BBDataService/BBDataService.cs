using BuzzerWolf.BBAPI;
using BuzzerWolf.Server.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace BuzzerWolf.Server
{
    public partial class BBDataService(IServiceProvider serviceProvider, IBBAPIClient bbapi) : IBBDataService
    {
        private static readonly ConcurrentDictionary<SynchronizedEntity, SemaphoreSlim> SyncLockDictionary = new();

        private const int MINIMUM_MINUTES_BETWEEN_REQUESTED_SYNC = 15;
        private async Task<bool> ShouldSync(SynchronizedEntity syncEntity, bool syncRequested)
        {
            var context = serviceProvider.GetRequiredService<BuzzerWolfContext>();
            var syncRecord = await context.Sync.FirstOrDefaultAsync(s => s.Table == syncEntity.Table && s.TeamId == syncEntity.TeamId && s.EntityId == syncEntity.EntityId && s.Season == syncEntity.Season);

            if (syncRecord == null)
                return true;

            if (DateTimeOffset.UtcNow > syncRecord.NextAutoSync)
                return true;

            if (syncRequested && DateTimeOffset.UtcNow > syncRecord.LastSync + TimeSpan.FromMinutes(MINIMUM_MINUTES_BETWEEN_REQUESTED_SYNC))
                return true;

            return false;
        }

        private async Task UpdateSyncRecord(SynchronizedEntity syncEntity, DateTimeOffset nextAutoSync)
        {
            var context = serviceProvider.GetRequiredService<BuzzerWolfContext>();
            var syncRecord = await context.Sync.FirstOrDefaultAsync(s => s.Table == syncEntity.Table && s.TeamId == syncEntity.TeamId && s.EntityId == syncEntity.EntityId && s.Season == syncEntity.Season);
            if (syncRecord == null)
            {
                syncRecord = new Sync { Table = syncEntity.Table, TeamId = syncEntity.TeamId, EntityId = syncEntity.EntityId, Season = syncEntity.Season, LastSync = DateTimeOffset.MinValue, NextAutoSync = DateTimeOffset.MinValue };
                await context.Sync.AddAsync(syncRecord);
            }

            syncRecord.LastSync = DateTimeOffset.UtcNow;
            syncRecord.NextAutoSync = nextAutoSync;
            await context.SaveChangesAsync();
        }
    }
}

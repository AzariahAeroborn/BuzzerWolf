using BuzzerWolf.BBAPI;
using BuzzerWolf.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace BuzzerWolf.Server
{
    public partial class BBDataService(BuzzerWolfContext context, IBBAPIClient bbapi) : IBBDataService
    {
        private async Task<Sync> GetTableSyncRecord(SyncTable table, int? teamId = null, int? entityId = null, int? seasonId = null)
        {
            var syncRecord = await context.Sync.FirstOrDefaultAsync(s => s.TeamId == teamId && s.Table == table && s.EntityId == entityId && s.Season == seasonId);
            if (syncRecord == null)
            {
                syncRecord = new Sync { TeamId = teamId, Table = table, EntityId = entityId, Season = seasonId, LastSync = DateTimeOffset.MinValue, NextAutoSync = DateTimeOffset.MinValue };
                await context.Sync.AddAsync(syncRecord);
            }
            return syncRecord;
        }

        private async Task UpdateTableSyncRecord(Sync sync, DateTimeOffset nextAutoSync)
        {
            sync.LastSync = DateTimeOffset.UtcNow;
            sync.NextAutoSync = nextAutoSync;
            await context.SaveChangesAsync();
        }
    }
}

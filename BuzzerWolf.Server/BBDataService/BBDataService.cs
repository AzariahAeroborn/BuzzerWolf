using BuzzerWolf.BBAPI;
using BuzzerWolf.Server.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace BuzzerWolf.Server
{
    public partial class BBDataService(BuzzerWolfContext context, IBBAPIClient bbapi) : IBBDataService
    {
        private async Task<Sync> GetTableSyncRecord(SyncTable table, int? teamId = null, object? parameters = null)
        {
            var syncParams = parameters != null ? JsonSerializer.Serialize(parameters): null;
            var syncRecord = await context.Sync.FirstOrDefaultAsync(s => s.TeamId == teamId && s.Table == table && s.Params == syncParams);
            if (syncRecord == null) 
            { 
                syncRecord = new Sync { TeamId = teamId, Table = table, Params = syncParams, LastSync = DateTimeOffset.MinValue, NextAutoSync = DateTimeOffset.MinValue };
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

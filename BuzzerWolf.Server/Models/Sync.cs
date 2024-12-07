namespace BuzzerWolf.Server.Models
{
    public class Sync
    {
        public int Id { get; set; }
        public int? TeamId { get; set; }
        public SyncTable Table { get; set; }
        public int? EntityId { get; set; }
        public int? Season { get; set; }
        public DateTimeOffset LastSync { get; set; }
        public DateTimeOffset NextAutoSync { get; set; }
    }

    public enum SyncTable
    {
        Season,
        Country,
        League,
        LeagueStandings
    }

    public class SynchronizedEntity
    {
        public SynchronizedEntity(SyncTable table, int? forTeam = null, int? entityId = null, int? forSeason = null)
        {
            Table = table;
            TeamId = forTeam;
            EntityId = entityId;
            Season = forSeason;
        }
        public SyncTable Table { get; }
        public int? TeamId { get; }
        public int? EntityId { get; }
        public int? Season { get; }
    }
}

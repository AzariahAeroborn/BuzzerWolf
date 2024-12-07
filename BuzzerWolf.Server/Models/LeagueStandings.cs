using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace BuzzerWolf.Server.Models
{
    public enum Conference
    {
        Big8,
        Great8,
    }

    [PrimaryKey(nameof(LeagueId), nameof(Season), nameof(TeamId))]
    public class LeagueStandings
    {
        public int LeagueId { get; set; }
        public int Season { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public Conference Conference { get; set; }
        public int ConferenceRank { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int PointsFor { get; set; }
        public int PointsAgainst { get; set; }
        public bool IsBot { get; set; }

        [JsonIgnore]
        public League League { get; set; }
    }
}

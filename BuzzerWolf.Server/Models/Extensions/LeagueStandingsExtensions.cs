namespace BuzzerWolf.Server.Models.Extensions
{
    public static class LeagueStandingsExtensions
    {
        public static IEnumerable<LeagueStandings> FromBBAPI(this BBAPI.Model.Standings standings)
        {
            return standings.Great8.Select(ts => ts.FromBBAPI(standings, Conference.Great8, ts.ConferenceRank))
                .Concat(standings.Big8.Select(ts => ts.FromBBAPI(standings, Conference.Big8, ts.ConferenceRank)));
        }

        private static LeagueStandings FromBBAPI(this BBAPI.Model.TeamStanding teamStanding, BBAPI.Model.Standings standings, Conference conference, int conferenceRank)
        {
            return new LeagueStandings
            {
                LeagueId = standings.League.Id,
                Season = standings.Season,
                Conference = conference,
                ConferenceRank = conferenceRank,
                TeamId = teamStanding.TeamId,
                TeamName = teamStanding.TeamName,
                Wins = teamStanding.Wins,
                Losses = teamStanding.Losses,
                PointsFor = teamStanding.PointsFor,
                PointsAgainst = teamStanding.PointsAgainst,
                IsBot = teamStanding.IsBot,
            };
        }

        public static IOrderedEnumerable<LeagueStandings> OrderedConferenceStandings(this IEnumerable<LeagueStandings> standings, Conference conference)
        {
            return standings.Where(s => s.Conference == conference)
                            .OrderByDescending(s => s.Wins)
                            .ThenByDescending(s => s.PointsFor - s.PointsAgainst);
        }
    }
}

using BuzzerWolf.BBAPI.Model;
using BuzzerWolf.Server.Models;

namespace BuzzerWolf.Server
{
    public partial class BBDataService : IBBDataService
    {
        public async Task<List<PromotionStanding>> GetPromotionStandings(int country, int division, int? season = null)
        {
            season ??= (await GetCurrentSeason()).Id;
            var auto = 0;
            var total = 0;
            var bot = 0;

            var leaguesList = await bbapi.GetLeagues(country, division);
            var standings = new List<TeamStanding>();
            foreach (var league in leaguesList)
            {
                var leagueStandings = await bbapi.GetStandings(league.Id, season);
                if (leagueStandings.IsFinal)
                {
                    var winner = leagueStandings.Big8.Where(t => t.IsWinner).Union(leagueStandings.Great8.Where(t => t.IsWinner)).First();
                    if (winner.IsBot) { bot++; }
                }
                standings.AddRange(leagueStandings.Big8);
                standings.AddRange(leagueStandings.Great8);
            }

            var nextDivisionHigher = (int)division - 1;
            var checkDivision = nextDivisionHigher;
            do
            {
                var promotingLeaguesList = Task.Run(() => bbapi.GetLeagues(country, checkDivision)).Result;
                foreach (var league in promotingLeaguesList)
                {
                    int leagueBots = 0;
                    var leagueStandings = Task.Run(() => bbapi.GetStandings(league.Id)).Result;
                    if (checkDivision == nextDivisionHigher)
                    {
                        if (leagueStandings.IsFinal)
                        {

                        }
                        else
                        {
                            leagueBots = leagueStandings.Big8.Count(t => t.ConferenceRank < 8 && t.IsBot) + leagueStandings.Great8.Count(t => t.ConferenceRank < 8 && t.IsBot);
                        }
                    }
                    else
                    {
                        leagueBots = leagueStandings.Big8.Count(t => t.IsBot) + leagueStandings.Great8.Count(t => t.IsBot);
                    }

                    if (checkDivision == nextDivisionHigher)
                    {
                        auto += 1;
                        total += 5;
                    }

                    total += leagueBots;
                    bot += leagueBots;
                }

                checkDivision--;
            } while (checkDivision >= 1);

            var champ = standings.Count(s => s.IsWinner);
            return standings.Where(s => s.IsWinner || s.ConferenceRank <= 2)
                    .Select((s, idx) => new PromotionStanding
                    {
                        Country = country,
                        Division = division,
                        Season = (int)season,
                        TeamId = s.TeamId,
                        TeamName = s.TeamName,
                        Wins = s.Wins,
                        Losses = s.Losses,
                        PointDifference = s.PointDifference,
                        ConferenceRank = s.ConferenceRank,
                        LeagueName = s.LeagueName,
                        ConferenceName = s.ConferenceName,
                        PromotionRank = idx + 1,
                        IsChampionPromotion = s.IsWinner,
                        IsAutoPromotion = !s.IsWinner && (idx + 1) <= (champ + auto),
                        IsBotPromotion = !s.IsWinner && (idx + 1) > (champ + auto) && (idx + 1) <= (champ + auto + bot),
                        IsTotalPromotion = !s.IsWinner && (idx + 1) > (champ + auto + bot) && (idx + 1) <= total,
                    }).ToList();
        }
    }
}

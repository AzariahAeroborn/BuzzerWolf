using BuzzerWolf.BBAPI.Model;
using BuzzerWolf.Server.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace BuzzerWolf.Server
{
    public partial class BBDataService : IBBDataService
    {
        private static SemaphoreSlim syncPromotionStandings = new(1);

        private async Task SynchronizePromotionStandings(int country, int division, int season)
        {
            syncPromotionStandings.Wait();
            try
            {
                var promotionStandingsParams = JsonSerializer.Serialize(new { country, division, season });
                var promotionStandingsSyncRecord = await GetTableSyncRecord(SyncTable.PromotionStanding, null, promotionStandingsParams);

                if (promotionStandingsSyncRecord.NextAutoSync < DateTimeOffset.UtcNow)
                {
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

                    context.PromotionStandings.Where(ps => ps.Country == country && ps.Division == division && ps.Season == season).ExecuteDelete();

                    var champ = standings.Count(s => s.IsWinner);
                    await context.PromotionStandings.AddRangeAsync(
                        standings.Where(s => s.IsWinner || s.ConferenceRank <= 2)
                            .Select((s, idx) => new PromotionStanding
                            {
                                Country = country,
                                Division = division,
                                Season = season,
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
                            })
                    );
                    // If there are champions, the promotion standings are for a completed season and will not update further
                    var nextSync = champ > 0 ? DateTimeOffset.MaxValue : DateTimeOffset.UtcNow.AddHours(1);
                    await UpdateTableSyncRecord(promotionStandingsSyncRecord, nextSync);
                }
            }
            finally { syncPromotionStandings.Release(); }
        }

        public async Task<List<PromotionStanding>> GetPromotionStandings(int country, int division, int? season = null)
        {
            season ??= await GetCurrentSeason();
            await SynchronizePromotionStandings(country, division, season.Value);

            return context.PromotionStandings
                .Where(ps => ps.Country == country && ps.Division == division && ps.Season == season)
                .OrderByDescending(s => s.IsChampionPromotion)
                .ThenBy(s => s.ConferenceRank)
                .ThenByDescending(s => s.Wins)
                .ThenByDescending(s => s.PointDifference)
                .ToList();
        }
    }
}

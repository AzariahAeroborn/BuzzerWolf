using BuzzerWolf.Server.Models;
using BuzzerWolf.Server.Models.Extensions;
using Microsoft.EntityFrameworkCore;

namespace BuzzerWolf.Server
{
    public partial class BBDataService : IBBDataService
    {
        private async Task SynchronizeLeagueStandings(int leagueId, int season, bool syncRequested)
        {
            var syncEntity = new SynchronizedEntity(SyncTable.LeagueStandings, entityId: leagueId, forSeason: season);
            if (!await ShouldSync(syncEntity, syncRequested))
                return;

            var syncLock = SyncLockDictionary.GetOrAdd(syncEntity, new SemaphoreSlim(1));
            await syncLock.WaitAsync();
            try
            {
                if (await ShouldSync(syncEntity, syncRequested))
                {
                    var context = serviceProvider.GetRequiredService<BuzzerWolfContext>();
                    await context.LeagueStandings.UpsertRange((await bbapi.GetStandings(leagueId, season)).FromBBAPI()).RunAsync();
                    var nextSyncTime = DateTimeOffset.UtcNow + TimeSpan.FromHours(1);
                    await UpdateSyncRecord(syncEntity, nextSyncTime);
                }
            }
            finally { syncLock.Release(); }
        }

        public async Task<List<PromotionStanding>> GetPromotionStandings(int country, int division, int? season = null, bool syncRequested = false)
        {
            season ??= (await GetCurrentSeason()).Id;
            var auto = 0;
            var total = 0;
            var bot = 0;

            var leaguesList = await GetLeagues(country, division);
            var leagueStandingsSyncTasks = new List<Task>();
            foreach (var league in leaguesList)
            {
                leagueStandingsSyncTasks.Add(SynchronizeLeagueStandings(league.Id, (int)season, syncRequested));
            }
            for (int higherDivision = division - 1; higherDivision >= 1; higherDivision--)
            {
                var higherDivisionLeaguesList = await GetLeagues(country, higherDivision);
                foreach (var league in higherDivisionLeaguesList)
                {
                    leagueStandingsSyncTasks.Add(SynchronizeLeagueStandings(league.Id, (int)season, syncRequested));
                    if (higherDivision == division - 1)
                    {
                        total += league.Country.Name == "Utopia" ? 6 : 5;
                    }
                }
            }
            auto = total - leaguesList.Count;

            await Task.WhenAll(leagueStandingsSyncTasks);

            var context = serviceProvider.GetRequiredService<BuzzerWolfContext>();
            var standings = context.LeagueStandings.Include(s => s.League).Where(s => s.League.CountryId == country && s.League.DivisionLevel == division && s.Season == season && s.ConferenceRank <= 2).ToList();
            var bots = context.LeagueStandings.Where(s => s.League.CountryId == country && s.League.DivisionLevel < division && s.Season == season && s.IsBot && s.ConferenceRank < 8).Count();
            total += bots;

            return standings
                    .OrderBy(s => s.ConferenceRank)
                    .ThenByDescending(s => s.Wins)
                    .ThenByDescending(s => s.PointsFor - s.PointsAgainst)
                    .Select((s, idx) => new PromotionStanding
                    {
                        Country = country,
                        Division = division,
                        Season = (int)season,
                        TeamId = s.TeamId,
                        TeamName = s.TeamName,
                        Wins = s.Wins,
                        Losses = s.Losses,
                        PointDifference = s.PointsFor - s.PointsAgainst,
                        ConferenceRank = s.ConferenceRank,
                        LeagueName = s.League.Name,
                        ConferenceName = s.Conference.ToString(),
                        PromotionRank = idx + 1,
                        IsChampionPromotion = false,
                        IsAutoPromotion = (idx + 1) <= (auto),
                        IsBotPromotion = (idx + 1) > (auto) && (idx + 1) <= (auto + bot),
                        IsTotalPromotion = (idx + 1) > (auto + bot) && (idx + 1) <= total,
                    }).ToList();
        }
    }
}

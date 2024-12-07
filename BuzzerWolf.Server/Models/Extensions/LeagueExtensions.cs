namespace BuzzerWolf.Server.Models.Extensions
{
    public static class LeagueExtensions
    {
        public static IEnumerable<League> FromBBAPI(this IEnumerable<BBAPI.Model.League> leagues)
        {
            return leagues.Select(FromBBAPI);
        }

        public static League FromBBAPI(this BBAPI.Model.League league)
        {
            return new League
            {
                Id = league.Id,
                Name = league.Name,
                CountryId = league.CountryId,
                DivisionLevel = league.Level,
            };
        }
    }
}

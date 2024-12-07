using System.Xml.Linq;

namespace BuzzerWolf.BBAPI.Model
{
    public class League
    {
        public League(XElement leagueInfo, int countryId, int level)
        {
            Id = int.Parse(leagueInfo.Attribute("id")!.Value);
            Name = leagueInfo.Value;
            CountryId = countryId;
            Level = level;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }
        public int Level { get; set; }
    }
}

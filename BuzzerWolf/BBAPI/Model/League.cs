using System.Xml.Linq;

namespace BuzzerWolf.BBAPI.Model
{
    public class League
    {
        public League(XElement leagueInfo)
        {
            Id = int.Parse(leagueInfo.Attribute("id")!.Value);
            Level = int.Parse(leagueInfo.Attribute("level")!.Value);
            Name = leagueInfo.Value;
        }

        public int Id { get; init; }
        public int Level { get; init; }
        public string Name { get; init; }
    }
}

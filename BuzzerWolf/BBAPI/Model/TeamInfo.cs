using System.Xml.Linq;

namespace BuzzerWolf.BBAPI.Model
{
    public class TeamInfo
    {
        public TeamInfo(XElement bbapiResponse)
        {
            Id = int.Parse(bbapiResponse.Descendants("team").First().Attribute("id")!.Value);
            TeamName = bbapiResponse.Descendants("teamName").First().Value;
            ShortName = bbapiResponse.Descendants("shortName").First().Value;
            var ownerInfo = bbapiResponse.Descendants("owner").First();
            Owner = ownerInfo.Value;
            IsSupporter = ownerInfo.Attribute("supporter")?.Value == "1";
            CreateDate = DateTime.Parse(bbapiResponse.Descendants("createDate").First().Value);
            LastLoginDate = DateTime.Parse(bbapiResponse.Descendants("lastLoginDate").First().Value);
            League = new League(bbapiResponse.Descendants("league").First());
            Country = new Country(bbapiResponse.Descendants("country").First());
        }

        public int Id { get; init; }
        public string TeamName { get; init; }
        public string ShortName { get; init; }
        public string Owner { get; init; }
        public bool IsSupporter { get; init; }
        public DateTime CreateDate { get; init; }
        public DateTime LastLoginDate { get; init; }
        public League League { get; init; }
        public Country Country { get; init; }
        //public TeamInfo Rival { get; init; }
    }
}

using System.Xml.Linq;

namespace BuzzerWolf.BBAPI.Model
{
    public class Country
    {
        public Country(XElement countryInfo)
        {
            Id = int.Parse(countryInfo.Attribute("id")!.Value);
            Name = countryInfo.Value;
        }

        public int Id { get; init; }
        public string Name { get; init; }
    }
}
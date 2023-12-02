namespace BuzzerWolf.Server.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Divisions { get; set; }
        public int? FirstSeason { get; set; }
    }
}

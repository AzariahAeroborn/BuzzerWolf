using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BuzzerWolf.Server.Models
{
    public class League
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public int CountryId { get; set; }
        public string Name { get; set; }
        public int DivisionLevel { get; set; }

        [JsonIgnore]
        public Country Country { get; set; }
    }
}

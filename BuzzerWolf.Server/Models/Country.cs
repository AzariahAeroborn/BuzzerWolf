﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BuzzerWolf.Server.Models
{
    public class Country
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Divisions { get; set; }
        public int? FirstSeason { get; set; }

        public ICollection<League> Leagues { get; set; }
    }
}

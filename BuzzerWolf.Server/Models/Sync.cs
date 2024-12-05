﻿namespace BuzzerWolf.Server.Models
{
    public class Sync
    {
        public int Id { get; set; }
        public int? TeamId { get; set; }
        public SyncTable Table { get; set; }
        public int? EntityId { get; set; }
        public int? Season { get; set; }
        public DateTimeOffset LastSync { get; set; }
        public DateTimeOffset NextAutoSync { get; set; }
    }

    public enum SyncTable
    {
        Season,
        Country,
        League,
        PromotionStanding
    }
}

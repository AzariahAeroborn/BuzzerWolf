using Microsoft.EntityFrameworkCore;

namespace BuzzerWolf.Server.Models
{
    public class BuzzerWolfContext : DbContext
    {
        public int LoggedInTeam { get; set; } = -1;

        public DbSet<Country> Countries { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<Sync> Sync { get; set; }
        public DbSet<PromotionStanding> PromotionStandings { get; set; }

        private string _dbPath;

        public BuzzerWolfContext()
        {
            _dbPath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "BuzzerWolf", "buzzerwolf.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlite($"Data Source={_dbPath}");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Sync>()
                .Property(e => e.Table)
                .HasConversion<string>();
            modelBuilder.Entity<Sync>()
                .HasQueryFilter(f => f.TeamId == null || f.TeamId == LoggedInTeam);
        }
    }
}

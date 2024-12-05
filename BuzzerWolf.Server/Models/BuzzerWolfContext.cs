using Microsoft.EntityFrameworkCore;

namespace BuzzerWolf.Server.Models
{
    public class BuzzerWolfContext : DbContext
    {
        public int LoggedInTeam { get; set; } = -1;

        public DbSet<Country> Countries { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<League> Leagues { get; set; }
        public DbSet<Sync> Sync { get; set; }

        private readonly IConfiguration _configuration;

        public BuzzerWolfContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer(_configuration.GetConnectionString("BuzzerWolfContext"));

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

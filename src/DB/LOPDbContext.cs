using Common;
using Microsoft.EntityFrameworkCore;

namespace DB
{
    public class LOPDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(Config.GetConnectionString("LeagueOfProgrammers"));
        }

        public DbSet<Tables.File> Files { get; set; }
    }
}

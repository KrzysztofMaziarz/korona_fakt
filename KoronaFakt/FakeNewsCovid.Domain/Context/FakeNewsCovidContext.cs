using FakeNewsCovid.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FakeNewsCovid.Domain.Context
{
    public class FakeNewsCovidContext : DbContext
    {
        public FakeNewsCovidContext(DbContextOptions<FakeNewsCovidContext> options)
            : base(options)
        { }

        public DbSet<TaggedUrl> TaggedUrls { get; set; }

        public DbSet<FakeReason> FakeReasons { get; set; }

        public DbSet<VerifiedDomain> VerifiedDomains { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
                optionsBuilder.UseNpgsql(@"Server=77.55.226.197;Port=5432;Database=korona;User Id=dev;Password=korona@3341_fakt#45da@@34;", x => x.MigrationsHistoryTable("__EFMigrationsHistory"));
        }
    }
}

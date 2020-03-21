using FakeNewsCovid.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FakeNewsCovid.Domain.Context
{
    public class FakeNewsCovidContext : DbContext
    {
        private readonly IConfiguration configuration;

        public FakeNewsCovidContext(DbContextOptions<FakeNewsCovidContext> options, IConfiguration configuration)
            : base(options)
        {
            this.configuration = configuration;
        }

        public DbSet<TaggedUrl> TaggedUrls { get; set; }

        public DbSet<FakeReason> FakeReasons { get; set; }

        public DbSet<VerifiedDomain> VerifiedDomains { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
                optionsBuilder.UseNpgsql(configuration.GetValue<string>("SQLConnection:ConnectionString"), x => x.MigrationsHistoryTable("__EFMigrationsHistory"));
        }
    }
}

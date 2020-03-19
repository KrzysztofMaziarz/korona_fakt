using FakeNewsCovid.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FakeNewsCovid.Domain.Context
{
    public class FakeNewsCovidContext : DbContext
    {
        public FakeNewsCovidContext()
            : base()
        {
        }

        public DbSet<TaggedUrl> TaggedUrls { get; set; }


    }
}

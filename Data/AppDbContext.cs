using Microsoft.EntityFrameworkCore;
using IntravisionTest.Models;

namespace IntravisionTest.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Coin> Coins { get; set; }
        public DbSet<Drink> Drinks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Coin>().HasData(
        new Coin { Id = Guid.Parse("f1a1b1c1-0001-0001-0001-000000000001"), Denomination = 1, Count = 20, IsBlocked = false },
        new Coin { Id = Guid.Parse("f1a1b1c1-0002-0002-0002-000000000002"), Denomination = 2, Count = 20, IsBlocked = false },
        new Coin { Id = Guid.Parse("f1a1b1c1-0005-0005-0005-000000000005"), Denomination = 5, Count = 20, IsBlocked = false },
        new Coin { Id = Guid.Parse("f1a1b1c1-0010-0010-0010-000000000010"), Denomination = 10, Count = 20, IsBlocked = false }
    );
}
    }
}

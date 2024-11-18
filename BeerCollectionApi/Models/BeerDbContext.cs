using Microsoft.EntityFrameworkCore;

namespace BeerCollectionApi.Models
{
    public class BeerDbContext : DbContext
    {
        public BeerDbContext(DbContextOptions<BeerDbContext> options) : base(options)
        {
        }

        public DbSet<Beer> Beers { get; set; }
    }
}

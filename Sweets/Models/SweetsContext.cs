using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Sweets.Models
{
  public class SweetsContext: DbContext
  {
    public virtual DbSet<Treat> Treats { get; set; }
    public DbSet<Flavour> Flavours { get; set; }
    public DbSet<FlavourTreat> FlavourTreat { get; set; }

    public SweetsContext(DbContextOptions options) : base(options) { }
  }
}
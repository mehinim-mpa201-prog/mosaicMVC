using Microsoft.EntityFrameworkCore;
using MosaicMVC.Models;

namespace MosaicMVC.Contexts;

public class AppDbContext:DbContext
{
    public DbSet<Team> Teams { get; set; }
    public DbSet<Position> Positions { get; set; }

    public AppDbContext(DbContextOptions options):base(options)
    {
        
    }
}

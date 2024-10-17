using HC_7572.Domain.Objects;
using Microsoft.EntityFrameworkCore;
using Location = HC_7572.Domain.Objects.Location;

namespace HC_7572.DbContexts;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Location> Locations => Set<Location>();
}
using Microsoft.EntityFrameworkCore;
using Model;
using Sep3DataTier.Repository;

namespace Sep3DataTier.Database;

public class DatabaseContext : DbContext
{
    public DbSet<Location> Locations { get; set; }
    public DbSet<Model.Report> Reports { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql($"Host=localhost;Port=5432;Database=cleanup;Username={DatabaseCredentials.PostgresqlUsername};Password={DatabaseCredentials.PostgresqlPassword}",
            options => options.UseAdminDatabase("cleanup"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Location>().HasKey(location => location.Id);
        modelBuilder.Entity<Model.Report>()
            .HasKey(report => report.Id);

    }   
}
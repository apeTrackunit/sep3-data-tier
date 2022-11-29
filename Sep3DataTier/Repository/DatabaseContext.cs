using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Model;
using Sep3DataTier.Repository;

namespace Sep3DataTier.Database;

public class DatabaseContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
{
    public DbSet<Location> Locations { get; set; }
    public DbSet<Model.Report> Reports { get; set; }
    
    public DbSet<ApplicationUser> Users { get; set; }


    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
        
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("" +
                                 DatabaseCredentials.Host +
                                 DatabaseCredentials.Port +
                                 DatabaseCredentials.Database +
                                 DatabaseCredentials.Username +
                                 DatabaseCredentials.Password,
            options => options.UseAdminDatabase(DatabaseCredentials.AdminDatabase));

        // optionsBuilder.UseNpgsql($"Host=localhost;Port=5432;Database=cleanup;Username={DatabaseCredentials.PostgresqlUsername};Password={DatabaseCredentials.PostgresqlPassword}",
            // options => options.UseAdminDatabase("cleanup"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Location>().HasKey(location => location.Id);
        modelBuilder.Entity<Model.Report>()
            .HasKey(report => report.Id);
        modelBuilder.Entity<ApplicationUser>().HasKey(user => user.Id);
        modelBuilder.Entity<IdentityRole>().HasData(GenerateIdentityRole("User"));
        modelBuilder.Entity<IdentityRole>().HasData(GenerateIdentityRole("Admin"));
        base.OnModelCreating(modelBuilder);
        
    }
    private static IdentityRole GenerateIdentityRole(string name)
    {
        return new IdentityRole(name) {NormalizedName = name.ToUpperInvariant()};
    }
}
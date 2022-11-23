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
        optionsBuilder.UseNpgsql("" +
                                 "Host=mouse.db.elephantsql.com;" +
                                 "Port=5432;" +
                                 "Database=mdvlimng;" +
                                 "Username=mdvlimng;" +
                                 "Password=7UAYlVQ88oSXRzRrKMHHpx9MIwkYCNzJ",
            options => options.UseAdminDatabase("mdvlimng"));

        //optionsBuilder.UseNpgsql($"Host=localhost;Port=5432;Database=cleanup;Username={DatabaseCredentials.PostgresqlUsername};Password={DatabaseCredentials.PostgresqlPassword}",
        //options => options.UseAdminDatabase("cleanup"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Location>().HasKey(location => location.Id);
        modelBuilder.Entity<Model.Report>()
            .HasKey(report => report.Id);

    }  
    //
    // private static string Get()
    // {
    //     var uriString = ConfigurationManager.AppSettings["postgres://mdvlimng:7UAYlVQ88oSXRzRrKMHHpx9MIwkYCNzJ@mouse.db.elephantsql.com/mdvlimng"];
    //     var uri = new Uri(uriString);
    //     var db = uri.AbsolutePath.Trim('/');
    //     var user = uri.UserInfo.Split(':')[0];
    //     var passwd = uri.UserInfo.Split(':')[1];
    //     var port = uri.Port > 0 ? uri.Port : 5432;
    //     var connStr = string.Format("Server={0};Database={1};User Id={2};Password={3};Port={4}",
    //         uri.Host, db, user, passwd, port);
    //     return connStr;
    // }

}
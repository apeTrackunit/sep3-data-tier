﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Model;
using Sep3DataTier.Repository;

namespace Sep3DataTier.Database;

public class DatabaseContext : IdentityDbContext<ApplicationUser>
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
                                 "Host=mouse.db.elephantsql.com;" +
                                 "Port=5432;" +
                                 "Database=mdvlimng;" +
                                 "Username=mdvlimng;" +
                                 "Password=7UAYlVQ88oSXRzRrKMHHpx9MIwkYCNzJ",
            options => options.UseAdminDatabase("mdvlimng"));

        // optionsBuilder.UseNpgsql($"Host=localhost;Port=5432;Database=cleanup;Username={DatabaseCredentials.PostgresqlUsername};Password={DatabaseCredentials.PostgresqlPassword}",
            // options => options.UseAdminDatabase("cleanup"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Location>().HasKey(location => location.Id);
        modelBuilder.Entity<Model.Report>()
            .HasKey(report => report.Id);
        modelBuilder.Entity<ApplicationUser>().HasKey(user => user.Id);
        base.OnModelCreating(modelBuilder);

    }   
}
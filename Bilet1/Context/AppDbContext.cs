using Bilet1.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Bilet1.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Service> Services { get; set; }
    public DbSet<Category> Categories { get; set; }

    
}

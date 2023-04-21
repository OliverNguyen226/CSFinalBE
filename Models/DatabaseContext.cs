using Microsoft.EntityFrameworkCore;

namespace CSFinal.Models;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options) { }

    public DbSet<Painting> Paintings => Set<Painting>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Painting>()
            .Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()");
        modelBuilder.Entity<User>()
            .Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()");
    }
}
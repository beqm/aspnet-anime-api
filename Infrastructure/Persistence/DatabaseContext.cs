using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class DatabaseContext : DbContext
{
    public DbSet<Anime> Animes => Set<Anime>();

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Anime>()
            .HasIndex(a => a.Title)
            .IsUnique();
    }

}
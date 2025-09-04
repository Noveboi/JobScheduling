using JobScheduling.Persistence.Domain;
using Microsoft.EntityFrameworkCore;

namespace JobScheduling.Persistence.Application;

/// <summary>
/// Serves as the application's default <see cref="DbContext"/>. Nothing special here, just to help illustrate a simple web API.
/// </summary>
internal sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; private set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var userBuilder = modelBuilder.Entity<User>();

        userBuilder.HasKey(x => x.Id);
        userBuilder.Property(x => x.Id).ValueGeneratedNever();

        userBuilder.Property(x => x.Name).HasMaxLength(50);
    }
}
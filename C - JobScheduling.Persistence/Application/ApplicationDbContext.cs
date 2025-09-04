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
        userBuilder.Property(x => x.EmailAddress).HasMaxLength(250);

        userBuilder.Navigation(x => x.Jobs).AutoInclude();
        
        var userJobBuilder = modelBuilder.Entity<UserJob>();

        userJobBuilder.HasKey(x => x.Id);
        userJobBuilder.Property(x => x.Id).ValueGeneratedNever();
        userJobBuilder.Property(x => x.JobKey).HasMaxLength(100);
        userJobBuilder.Property(x => x.Description).HasMaxLength(200);

        userJobBuilder
            .HasOne<User>()
            .WithMany(u => u.Jobs)
            .HasForeignKey(x => x.UserId);
    }
}
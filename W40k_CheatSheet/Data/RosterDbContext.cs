using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace W40k_CheatSheet.Data;

public class RosterDbContext(DbContextOptions<RosterDbContext> options)
    : IdentityDbContext<AppUser>(options)
{
    public DbSet<SavedRoster> SavedRosters => Set<SavedRoster>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<SavedRoster>(e =>
        {
            e.HasKey(r => r.Id);
            e.HasIndex(r => r.UserId);
            e.HasOne(r => r.User)
             .WithMany(u => u.Rosters)
             .HasForeignKey(r => r.UserId)
             .OnDelete(DeleteBehavior.Cascade);
        });
    }
}

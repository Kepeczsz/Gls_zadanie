

using Gls_Etykiety.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;

namespace Gls_Etykiety.Domain;

public class LabelDbContext : DbContext
{
    public LabelDbContext(DbContextOptions<LabelDbContext> options) 
        : base(options)
    {
    }

    public DbSet<Label> Labels { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Label>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId);

        modelBuilder.Entity<User>()
            .HasKey(x =>x.Id);
    }
}

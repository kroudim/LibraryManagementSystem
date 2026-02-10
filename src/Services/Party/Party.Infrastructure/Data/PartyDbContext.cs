using Microsoft.EntityFrameworkCore;
using Party.Domain.Entities;

namespace Party.Infrastructure.Data;

public class PartyDbContext : DbContext
{
    public PartyDbContext(DbContextOptions<PartyDbContext> options) : base(options)
    {
    }

    public DbSet<Domain.Entities.Party> Parties { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<PartyRole> PartyRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Domain.Entities.Party>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.HasIndex(e => e.Email).IsUnique();
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.Name).IsUnique();
        });

        modelBuilder.Entity<PartyRole>(entity =>
        {
            entity.HasKey(e => new { e.PartyId, e.RoleId });
            
            entity.HasOne(e => e.Party)
                .WithMany(p => p.PartyRoles)
                .HasForeignKey(e => e.PartyId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(e => e.Role)
                .WithMany(r => r.PartyRoles)
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Seed data
        var authorRoleId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var customerRoleId = Guid.Parse("22222222-2222-2222-2222-222222222222");

        modelBuilder.Entity<Role>().HasData(
            new Role { Id = authorRoleId, Name = "Author" },
            new Role { Id = customerRoleId, Name = "Customer" }
        );

        var party1Id = Guid.Parse("33333333-3333-3333-3333-333333333333");
        var party2Id = Guid.Parse("44444444-4444-4444-4444-444444444444");
        var party3Id = Guid.Parse("55555555-5555-5555-5555-555555555555");

        modelBuilder.Entity<Domain.Entities.Party>().HasData(
            new Domain.Entities.Party
            {
                Id = party1Id,
                FirstName = "John",
                LastName = "Author",
                Email = "john.author@library.com",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Domain.Entities.Party
            {
                Id = party2Id,
                FirstName = "Jane",
                LastName = "Customer",
                Email = "jane.customer@library.com",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Domain.Entities.Party
            {
                Id = party3Id,
                FirstName = "Bob",
                LastName = "Both",
                Email = "bob.both@library.com",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        );

        modelBuilder.Entity<PartyRole>().HasData(
            new PartyRole { PartyId = party1Id, RoleId = authorRoleId, AssignedAt = DateTime.UtcNow },
            new PartyRole { PartyId = party2Id, RoleId = customerRoleId, AssignedAt = DateTime.UtcNow },
            new PartyRole { PartyId = party3Id, RoleId = authorRoleId, AssignedAt = DateTime.UtcNow },
            new PartyRole { PartyId = party3Id, RoleId = customerRoleId, AssignedAt = DateTime.UtcNow }
        );
    }
}

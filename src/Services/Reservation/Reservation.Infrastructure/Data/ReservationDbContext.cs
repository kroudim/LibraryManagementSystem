using Microsoft.EntityFrameworkCore;
using Reservation.Domain.Entities;

namespace Reservation.Infrastructure.Data;

public class ReservationDbContext : DbContext
{
    public ReservationDbContext(DbContextOptions<ReservationDbContext> options) : base(options)
    {
    }

    public DbSet<Domain.Entities.Reservation> Reservations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Domain.Entities.Reservation>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.CustomerPartyId, e.BookId, e.IsActive });
        });

        // Seed data
        var customerPartyId = Guid.Parse("44444444-4444-4444-4444-444444444444");
        var bookId = Guid.Parse("88888888-8888-8888-8888-888888888888");

        modelBuilder.Entity<Domain.Entities.Reservation>().HasData(
            new Domain.Entities.Reservation
            {
                Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                BookId = bookId,
                CustomerPartyId = customerPartyId,
                BorrowedAt = DateTime.UtcNow.AddDays(-5),
                IsActive = true
            }
        );
    }
}

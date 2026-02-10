using Microsoft.EntityFrameworkCore;
using Reservation.Domain.Interfaces;
using Reservation.Infrastructure.Data;

namespace Reservation.Infrastructure.Repositories;

public class ReservationRepository : IReservationRepository
{
    private readonly ReservationDbContext _context;

    public ReservationRepository(ReservationDbContext context)
    {
        _context = context;
    }

    public async Task<Domain.Entities.Reservation?> GetByIdAsync(Guid id)
    {
        return await _context.Reservations.FindAsync(id);
    }

    public async Task<IEnumerable<Domain.Entities.Reservation>> GetAllAsync()
    {
        return await _context.Reservations.ToListAsync();
    }

    public async Task<IEnumerable<Domain.Entities.Reservation>> GetActiveReservationsAsync()
    {
        return await _context.Reservations.Where(r => r.IsActive).ToListAsync();
    }

    public async Task<IEnumerable<Domain.Entities.Reservation>> GetByCustomerIdAsync(Guid customerPartyId)
    {
        return await _context.Reservations.Where(r => r.CustomerPartyId == customerPartyId).ToListAsync();
    }

    public async Task<Domain.Entities.Reservation?> GetActiveByCustomerAndBookAsync(Guid customerPartyId, Guid bookId)
    {
        return await _context.Reservations
            .FirstOrDefaultAsync(r => r.CustomerPartyId == customerPartyId && r.BookId == bookId && r.IsActive);
    }

    public async Task<Domain.Entities.Reservation> AddAsync(Domain.Entities.Reservation reservation)
    {
        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync();
        return reservation;
    }

    public async Task UpdateAsync(Domain.Entities.Reservation reservation)
    {
        _context.Reservations.Update(reservation);
        await _context.SaveChangesAsync();
    }
}

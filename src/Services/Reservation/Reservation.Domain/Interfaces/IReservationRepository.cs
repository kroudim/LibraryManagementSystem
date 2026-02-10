using Reservation.Domain.Entities;

namespace Reservation.Domain.Interfaces;

public interface IReservationRepository
{
    Task<Entities.Reservation?> GetByIdAsync(Guid id);
    Task<IEnumerable<Entities.Reservation>> GetAllAsync();
    Task<IEnumerable<Entities.Reservation>> GetActiveReservationsAsync();
    Task<IEnumerable<Entities.Reservation>> GetByCustomerIdAsync(Guid customerPartyId);
    Task<Entities.Reservation?> GetActiveByCustomerAndBookAsync(Guid customerPartyId, Guid bookId);
    Task<Entities.Reservation> AddAsync(Entities.Reservation reservation);
    Task UpdateAsync(Entities.Reservation reservation);
}

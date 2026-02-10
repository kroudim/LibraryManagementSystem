namespace Reservation.Domain.Entities;

public class Reservation
{
    public Guid Id { get; set; }
    public Guid BookId { get; set; }
    public Guid CustomerPartyId { get; set; }
    public DateTime BorrowedAt { get; set; }
    public DateTime? ReturnedAt { get; set; }
    public bool IsActive { get; set; }
}

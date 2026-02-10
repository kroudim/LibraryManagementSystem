using System;

namespace Reservation.Application.DTOs;

public class ReservationDto
{
    public Guid Id { get; set; }
    public Guid BookId { get; set; }
    public Guid CustomerPartyId { get; set; }
    public DateTime BorrowedAt { get; set; }
    public DateTime? ReturnedAt { get; set; }
    public bool IsActive { get; set; }
}

public class BorrowBookDto
{
    public Guid BookId { get; set; }
    public Guid CustomerPartyId { get; set; }
}

public class ReturnBookDto
{
    public Guid ReservationId { get; set; }
}

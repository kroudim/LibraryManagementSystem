namespace Library.Shared.Events;

public class BookBorrowed : BaseEvent
{
    public Guid ReservationId { get; set; }
    public Guid BookId { get; set; }
    public Guid CustomerPartyId { get; set; }
    public DateTime BorrowedAt { get; set; }
}

public class BookReturned : BaseEvent
{
    public Guid ReservationId { get; set; }
    public Guid BookId { get; set; }
    public Guid CustomerPartyId { get; set; }
    public DateTime ReturnedAt { get; set; }
}

namespace Library.Shared.Events;

public class BookCreated : BaseEvent
{
    public Guid BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public Guid AuthorPartyId { get; set; }
    public Guid CategoryId { get; set; }
    public int TotalCopies { get; set; }
    public int AvailableCopies { get; set; }
}

public class BookUpdated : BaseEvent
{
    public Guid BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public Guid AuthorPartyId { get; set; }
    public Guid CategoryId { get; set; }
    public int TotalCopies { get; set; }
    public int AvailableCopies { get; set; }
}

public class BookDeleted : BaseEvent
{
    public Guid BookId { get; set; }
}

public class CategoryCreated : BaseEvent
{
    public Guid CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class CategoryUpdated : BaseEvent
{
    public Guid CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class CategoryDeleted : BaseEvent
{
    public Guid CategoryId { get; set; }
}

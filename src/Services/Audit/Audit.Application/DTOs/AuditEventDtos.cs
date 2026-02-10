namespace Audit.Application.DTOs;

public class AuditEventDto
{
    public Guid EventId { get; set; }
    public string EntityId { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
    public string ActionType { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string Payload { get; set; } = string.Empty;
}

public class PagedResult<T>
{
    public List<T> Items { get; set; } = new();
    public int Page { get; set; }
    public int PageSize { get; set; }
    public long TotalCount { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}

namespace Library.Shared.Events;

public abstract class BaseEvent
{
    public Guid EventId { get; set; } = Guid.NewGuid();
    public string EntityId { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
    public string ActionType { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string Payload { get; set; } = string.Empty;
}

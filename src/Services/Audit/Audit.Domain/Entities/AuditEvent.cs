using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Audit.Domain.Entities;

public class AuditEvent
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid EventId { get; set; }
    
    public string EntityId { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
    public string ActionType { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string Payload { get; set; } = string.Empty;
}

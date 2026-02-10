using Audit.Application.DTOs;
using Audit.Domain.Entities;
using MongoDB.Driver;

namespace Audit.Application.Services;

public class AuditEventService
{
    private readonly IMongoCollection<AuditEvent> _auditEvents;

    public AuditEventService(IMongoDatabase database)
    {
        _auditEvents = database.GetCollection<AuditEvent>("AuditEvents");
    }

    public async Task AddEventAsync(AuditEvent auditEvent)
    {
        await _auditEvents.InsertOneAsync(auditEvent);
    }

    public async Task<PagedResult<AuditEventDto>> GetEventsAsync(int page, int pageSize)
    {
        var filter = Builders<AuditEvent>.Filter.Empty;
        var totalCount = await _auditEvents.CountDocumentsAsync(filter);
        
        var events = await _auditEvents
            .Find(filter)
            .Sort(Builders<AuditEvent>.Sort.Descending(e => e.Timestamp))
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync();

        return new PagedResult<AuditEventDto>
        {
            Items = events.Select(MapToDto).ToList(),
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<List<AuditEventDto>> GetEventsByPartyIdAsync(Guid partyId)
    {
        var filter = Builders<AuditEvent>.Filter.Eq(e => e.EntityId, partyId.ToString());
        var events = await _auditEvents
            .Find(filter)
            .Sort(Builders<AuditEvent>.Sort.Descending(e => e.Timestamp))
            .ToListAsync();

        return events.Select(MapToDto).ToList();
    }

    public async Task<List<AuditEventDto>> GetEventsByBookIdAsync(Guid bookId)
    {
        var filter = Builders<AuditEvent>.Filter.Eq(e => e.EntityId, bookId.ToString());
        var events = await _auditEvents
            .Find(filter)
            .Sort(Builders<AuditEvent>.Sort.Descending(e => e.Timestamp))
            .ToListAsync();

        return events.Select(MapToDto).ToList();
    }

    public async Task DeleteOldEventsAsync(DateTime before)
    {
        var filter = Builders<AuditEvent>.Filter.Lt(e => e.Timestamp, before);
        await _auditEvents.DeleteManyAsync(filter);
    }

    private static AuditEventDto MapToDto(AuditEvent auditEvent)
    {
        return new AuditEventDto
        {
            EventId = auditEvent.EventId,
            EntityId = auditEvent.EntityId,
            EntityType = auditEvent.EntityType,
            ActionType = auditEvent.ActionType,
            Timestamp = auditEvent.Timestamp,
            Payload = auditEvent.Payload
        };
    }
}

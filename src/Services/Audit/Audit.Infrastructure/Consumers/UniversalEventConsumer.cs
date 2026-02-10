using MassTransit;
using Audit.Application.Services;
using Audit.Domain.Entities;
using Library.Shared.Events;

namespace Audit.Infrastructure.Consumers;

public class PartyCreatedConsumer : IConsumer<PartyCreated>
{
    private readonly AuditEventService _auditService;
    public PartyCreatedConsumer(AuditEventService auditService) => _auditService = auditService;
    public async Task Consume(ConsumeContext<PartyCreated> context)
    {
        await _auditService.AddEventAsync(new AuditEvent
        {
            EventId = context.Message.EventId,
            EntityId = context.Message.EntityId,
            EntityType = context.Message.EntityType,
            ActionType = context.Message.ActionType,
            Timestamp = context.Message.Timestamp,
            Payload = context.Message.Payload
        });
    }
}

public class PartyUpdatedConsumer : IConsumer<PartyUpdated>
{
    private readonly AuditEventService _auditService;
    public PartyUpdatedConsumer(AuditEventService auditService) => _auditService = auditService;
    public async Task Consume(ConsumeContext<PartyUpdated> context)
    {
        await _auditService.AddEventAsync(new AuditEvent
        {
            EventId = context.Message.EventId,
            EntityId = context.Message.EntityId,
            EntityType = context.Message.EntityType,
            ActionType = context.Message.ActionType,
            Timestamp = context.Message.Timestamp,
            Payload = context.Message.Payload
        });
    }
}

public class PartyDeletedConsumer : IConsumer<PartyDeleted>
{
    private readonly AuditEventService _auditService;
    public PartyDeletedConsumer(AuditEventService auditService) => _auditService = auditService;
    public async Task Consume(ConsumeContext<PartyDeleted> context)
    {
        await _auditService.AddEventAsync(new AuditEvent
        {
            EventId = context.Message.EventId,
            EntityId = context.Message.EntityId,
            EntityType = context.Message.EntityType,
            ActionType = context.Message.ActionType,
            Timestamp = context.Message.Timestamp,
            Payload = context.Message.Payload
        });
    }
}

public class RoleAssignedConsumer : IConsumer<RoleAssigned>
{
    private readonly AuditEventService _auditService;
    public RoleAssignedConsumer(AuditEventService auditService) => _auditService = auditService;
    public async Task Consume(ConsumeContext<RoleAssigned> context)
    {
        await _auditService.AddEventAsync(new AuditEvent
        {
            EventId = context.Message.EventId,
            EntityId = context.Message.EntityId,
            EntityType = context.Message.EntityType,
            ActionType = context.Message.ActionType,
            Timestamp = context.Message.Timestamp,
            Payload = context.Message.Payload
        });
    }
}

public class RoleRemovedConsumer : IConsumer<RoleRemoved>
{
    private readonly AuditEventService _auditService;
    public RoleRemovedConsumer(AuditEventService auditService) => _auditService = auditService;
    public async Task Consume(ConsumeContext<RoleRemoved> context)
    {
        await _auditService.AddEventAsync(new AuditEvent
        {
            EventId = context.Message.EventId,
            EntityId = context.Message.EntityId,
            EntityType = context.Message.EntityType,
            ActionType = context.Message.ActionType,
            Timestamp = context.Message.Timestamp,
            Payload = context.Message.Payload
        });
    }
}

public class BookCreatedConsumer : IConsumer<BookCreated>
{
    private readonly AuditEventService _auditService;
    public BookCreatedConsumer(AuditEventService auditService) => _auditService = auditService;
    public async Task Consume(ConsumeContext<BookCreated> context)
    {
        await _auditService.AddEventAsync(new AuditEvent
        {
            EventId = context.Message.EventId,
            EntityId = context.Message.EntityId,
            EntityType = context.Message.EntityType,
            ActionType = context.Message.ActionType,
            Timestamp = context.Message.Timestamp,
            Payload = context.Message.Payload
        });
    }
}

public class BookUpdatedConsumer : IConsumer<BookUpdated>
{
    private readonly AuditEventService _auditService;
    public BookUpdatedConsumer(AuditEventService auditService) => _auditService = auditService;
    public async Task Consume(ConsumeContext<BookUpdated> context)
    {
        await _auditService.AddEventAsync(new AuditEvent
        {
            EventId = context.Message.EventId,
            EntityId = context.Message.EntityId,
            EntityType = context.Message.EntityType,
            ActionType = context.Message.ActionType,
            Timestamp = context.Message.Timestamp,
            Payload = context.Message.Payload
        });
    }
}

public class BookDeletedConsumer : IConsumer<BookDeleted>
{
    private readonly AuditEventService _auditService;
    public BookDeletedConsumer(AuditEventService auditService) => _auditService = auditService;
    public async Task Consume(ConsumeContext<BookDeleted> context)
    {
        await _auditService.AddEventAsync(new AuditEvent
        {
            EventId = context.Message.EventId,
            EntityId = context.Message.EntityId,
            EntityType = context.Message.EntityType,
            ActionType = context.Message.ActionType,
            Timestamp = context.Message.Timestamp,
            Payload = context.Message.Payload
        });
    }
}

public class CategoryCreatedConsumer : IConsumer<CategoryCreated>
{
    private readonly AuditEventService _auditService;
    public CategoryCreatedConsumer(AuditEventService auditService) => _auditService = auditService;
    public async Task Consume(ConsumeContext<CategoryCreated> context)
    {
        await _auditService.AddEventAsync(new AuditEvent
        {
            EventId = context.Message.EventId,
            EntityId = context.Message.EntityId,
            EntityType = context.Message.EntityType,
            ActionType = context.Message.ActionType,
            Timestamp = context.Message.Timestamp,
            Payload = context.Message.Payload
        });
    }
}

public class CategoryUpdatedConsumer : IConsumer<CategoryUpdated>
{
    private readonly AuditEventService _auditService;
    public CategoryUpdatedConsumer(AuditEventService auditService) => _auditService = auditService;
    public async Task Consume(ConsumeContext<CategoryUpdated> context)
    {
        await _auditService.AddEventAsync(new AuditEvent
        {
            EventId = context.Message.EventId,
            EntityId = context.Message.EntityId,
            EntityType = context.Message.EntityType,
            ActionType = context.Message.ActionType,
            Timestamp = context.Message.Timestamp,
            Payload = context.Message.Payload
        });
    }
}

public class CategoryDeletedConsumer : IConsumer<CategoryDeleted>
{
    private readonly AuditEventService _auditService;
    public CategoryDeletedConsumer(AuditEventService auditService) => _auditService = auditService;
    public async Task Consume(ConsumeContext<CategoryDeleted> context)
    {
        await _auditService.AddEventAsync(new AuditEvent
        {
            EventId = context.Message.EventId,
            EntityId = context.Message.EntityId,
            EntityType = context.Message.EntityType,
            ActionType = context.Message.ActionType,
            Timestamp = context.Message.Timestamp,
            Payload = context.Message.Payload
        });
    }
}

public class BookBorrowedConsumer : IConsumer<BookBorrowed>
{
    private readonly AuditEventService _auditService;
    public BookBorrowedConsumer(AuditEventService auditService) => _auditService = auditService;
    public async Task Consume(ConsumeContext<BookBorrowed> context)
    {
        await _auditService.AddEventAsync(new AuditEvent
        {
            EventId = context.Message.EventId,
            EntityId = context.Message.EntityId,
            EntityType = context.Message.EntityType,
            ActionType = context.Message.ActionType,
            Timestamp = context.Message.Timestamp,
            Payload = context.Message.Payload
        });
    }
}

public class BookReturnedConsumer : IConsumer<BookReturned>
{
    private readonly AuditEventService _auditService;
    public BookReturnedConsumer(AuditEventService auditService) => _auditService = auditService;
    public async Task Consume(ConsumeContext<BookReturned> context)
    {
        await _auditService.AddEventAsync(new AuditEvent
        {
            EventId = context.Message.EventId,
            EntityId = context.Message.EntityId,
            EntityType = context.Message.EntityType,
            ActionType = context.Message.ActionType,
            Timestamp = context.Message.Timestamp,
            Payload = context.Message.Payload
        });
    }
}

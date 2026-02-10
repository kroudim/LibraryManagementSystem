using Audit.Application.Services;
using Audit.Domain.Entities;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace Audit.UnitTests;

public class AuditEventServiceTests
{
    [Fact]
    public async Task AddEventAsync_ValidEvent_CallsInsertOne()
    {
        var mockCollection = new Mock<IMongoCollection<AuditEvent>>();
        var mockDatabase = new Mock<IMongoDatabase>();
        mockDatabase.Setup(d => d.GetCollection<AuditEvent>("AuditEvents", null))
            .Returns(mockCollection.Object);

        var service = new AuditEventService(mockDatabase.Object);
        var auditEvent = new AuditEvent
        {
            EventId = Guid.NewGuid(),
            EntityId = Guid.NewGuid().ToString(),
            EntityType = "Party",
            ActionType = "Created",
            Timestamp = DateTime.UtcNow,
            Payload = "{}"
        };

        await service.AddEventAsync(auditEvent);

        mockCollection.Verify(c => c.InsertOneAsync(
            It.IsAny<AuditEvent>(),
            It.IsAny<InsertOneOptions>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}

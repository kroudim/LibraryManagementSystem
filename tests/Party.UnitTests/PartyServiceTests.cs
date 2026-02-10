using Moq;
using Party.Application.DTOs;
using Party.Application.Services;
using Party.Domain.Entities;
using Party.Domain.Interfaces;
using Xunit;

namespace Party.UnitTests;

public class PartyServiceTests
{
    private readonly Mock<IPartyRepository> _partyRepositoryMock;
    private readonly Mock<IRoleRepository> _roleRepositoryMock;
    private readonly Mock<IEventPublisher> _eventPublisherMock;
    private readonly PartyService _partyService;

    public PartyServiceTests()
    {
        _partyRepositoryMock = new Mock<IPartyRepository>();
        _roleRepositoryMock = new Mock<IRoleRepository>();
        _eventPublisherMock = new Mock<IEventPublisher>();
        _partyService = new PartyService(_partyRepositoryMock.Object, _roleRepositoryMock.Object, _eventPublisherMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ValidParty_ReturnsPartyDto()
    {
        var createDto = new CreatePartyDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@test.com"
        };

        _partyRepositoryMock.Setup(r => r.EmailExistsAsync(createDto.Email, null))
            .ReturnsAsync(false);
        _partyRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Domain.Entities.Party>()))
            .ReturnsAsync((Domain.Entities.Party p) => p);

        var result = await _partyService.CreateAsync(createDto);

        Assert.NotNull(result);
        Assert.Equal(createDto.FirstName, result.FirstName);
        Assert.Equal(createDto.Email, result.Email);
        _eventPublisherMock.Verify(p => p.PublishAsync(It.IsAny<object>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_DuplicateEmail_ThrowsInvalidOperationException()
    {
        var createDto = new CreatePartyDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "existing@test.com"
        };

        _partyRepositoryMock.Setup(r => r.EmailExistsAsync(createDto.Email, null))
            .ReturnsAsync(true);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _partyService.CreateAsync(createDto));
    }

    [Fact]
    public async Task AssignRoleAsync_ValidAssignment_Succeeds()
    {
        var partyId = Guid.NewGuid();
        var roleId = Guid.NewGuid();
        var party = new Domain.Entities.Party { Id = partyId, Email = "test@test.com" };
        var role = new Role { Id = roleId, Name = "Customer" };

        _partyRepositoryMock.Setup(r => r.GetByIdAsync(partyId)).ReturnsAsync(party);
        _roleRepositoryMock.Setup(r => r.GetByIdAsync(roleId)).ReturnsAsync(role);
        _roleRepositoryMock.Setup(r => r.IsRoleAssignedAsync(partyId, roleId)).ReturnsAsync(false);

        await _partyService.AssignRoleAsync(partyId, roleId);

        _roleRepositoryMock.Verify(r => r.AssignRoleAsync(It.IsAny<PartyRole>()), Times.Once);
        _eventPublisherMock.Verify(p => p.PublishAsync(It.IsAny<object>()), Times.Once);
    }

    [Fact]
    public async Task AssignRoleAsync_DuplicateRole_ThrowsInvalidOperationException()
    {
        var partyId = Guid.NewGuid();
        var roleId = Guid.NewGuid();
        var party = new Domain.Entities.Party { Id = partyId, Email = "test@test.com" };
        var role = new Role { Id = roleId, Name = "Customer" };

        _partyRepositoryMock.Setup(r => r.GetByIdAsync(partyId)).ReturnsAsync(party);
        _roleRepositoryMock.Setup(r => r.GetByIdAsync(roleId)).ReturnsAsync(role);
        _roleRepositoryMock.Setup(r => r.IsRoleAssignedAsync(partyId, roleId)).ReturnsAsync(true);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _partyService.AssignRoleAsync(partyId, roleId));
    }
}

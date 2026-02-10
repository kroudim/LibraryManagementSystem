using Party.Application.DTOs;
using Party.Domain.Entities;
using Party.Domain.Interfaces;
using System.Text.Json;
using Library.Shared.Events;

namespace Party.Application.Services;

public class PartyService
{
    private readonly IPartyRepository _partyRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IEventPublisher _eventPublisher;

    public PartyService(IPartyRepository partyRepository, IRoleRepository roleRepository, IEventPublisher eventPublisher)
    {
        _partyRepository = partyRepository;
        _roleRepository = roleRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task<PartyDto> GetByIdAsync(Guid id)
    {
        var party = await _partyRepository.GetByIdAsync(id);
        if (party == null)
            throw new KeyNotFoundException($"Party with ID {id} not found");
        
        return MapToDto(party);
    }

    public async Task<IEnumerable<PartyDto>> GetAllAsync()
    {
        var parties = await _partyRepository.GetAllAsync();
        return parties.Select(MapToDto);
    }

    public async Task<PartyDto> CreateAsync(CreatePartyDto dto)
    {
        if (await _partyRepository.EmailExistsAsync(dto.Email))
            throw new InvalidOperationException($"Email {dto.Email} already exists");

        var party = new Domain.Entities.Party
        {
            Id = Guid.NewGuid(),
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        party = await _partyRepository.AddAsync(party);

        await _eventPublisher.PublishAsync(new PartyCreated
        {
            PartyId = party.Id,
            FirstName = party.FirstName,
            LastName = party.LastName,
            Email = party.Email,
            EntityId = party.Id.ToString(),
            EntityType = "Party",
            ActionType = "Created",
            Payload = JsonSerializer.Serialize(party)
        });

        return MapToDto(party);
    }

    public async Task<PartyDto> UpdateAsync(Guid id, UpdatePartyDto dto)
    {
        var party = await _partyRepository.GetByIdAsync(id);
        if (party == null)
            throw new KeyNotFoundException($"Party with ID {id} not found");

        if (await _partyRepository.EmailExistsAsync(dto.Email, id))
            throw new InvalidOperationException($"Email {dto.Email} already exists");

        party.FirstName = dto.FirstName;
        party.LastName = dto.LastName;
        party.Email = dto.Email;
        party.UpdatedAt = DateTime.UtcNow;

        await _partyRepository.UpdateAsync(party);

        await _eventPublisher.PublishAsync(new PartyUpdated
        {
            PartyId = party.Id,
            FirstName = party.FirstName,
            LastName = party.LastName,
            Email = party.Email,
            EntityId = party.Id.ToString(),
            EntityType = "Party",
            ActionType = "Updated",
            Payload = JsonSerializer.Serialize(party)
        });

        return MapToDto(party);
    }

    public async Task DeleteAsync(Guid id)
    {
        var party = await _partyRepository.GetByIdAsync(id);
        if (party == null)
            throw new KeyNotFoundException($"Party with ID {id} not found");

        await _partyRepository.DeleteAsync(id);

        await _eventPublisher.PublishAsync(new PartyDeleted
        {
            PartyId = id,
            EntityId = id.ToString(),
            EntityType = "Party",
            ActionType = "Deleted",
            Payload = JsonSerializer.Serialize(new { PartyId = id })
        });
    }

    public async Task AssignRoleAsync(Guid partyId, Guid roleId)
    {
        var party = await _partyRepository.GetByIdAsync(partyId);
        if (party == null)
            throw new KeyNotFoundException($"Party with ID {partyId} not found");

        var role = await _roleRepository.GetByIdAsync(roleId);
        if (role == null)
            throw new KeyNotFoundException($"Role with ID {roleId} not found");

        if (await _roleRepository.IsRoleAssignedAsync(partyId, roleId))
            throw new InvalidOperationException($"Role {role.Name} is already assigned to party");

        var partyRole = new PartyRole
        {
            PartyId = partyId,
            RoleId = roleId,
            AssignedAt = DateTime.UtcNow
        };

        await _roleRepository.AssignRoleAsync(partyRole);

        await _eventPublisher.PublishAsync(new RoleAssigned
        {
            PartyId = partyId,
            RoleId = roleId,
            RoleName = role.Name,
            EntityId = partyId.ToString(),
            EntityType = "Party",
            ActionType = "RoleAssigned",
            Payload = JsonSerializer.Serialize(new { PartyId = partyId, RoleId = roleId, RoleName = role.Name })
        });
    }

    public async Task RemoveRoleAsync(Guid partyId, Guid roleId)
    {
        var party = await _partyRepository.GetByIdAsync(partyId);
        if (party == null)
            throw new KeyNotFoundException($"Party with ID {partyId} not found");

        var role = await _roleRepository.GetByIdAsync(roleId);
        if (role == null)
            throw new KeyNotFoundException($"Role with ID {roleId} not found");

        if (!await _roleRepository.IsRoleAssignedAsync(partyId, roleId))
            throw new InvalidOperationException($"Role {role.Name} is not assigned to party");

        await _roleRepository.RemoveRoleAsync(partyId, roleId);

        await _eventPublisher.PublishAsync(new RoleRemoved
        {
            PartyId = partyId,
            RoleId = roleId,
            RoleName = role.Name,
            EntityId = partyId.ToString(),
            EntityType = "Party",
            ActionType = "RoleRemoved",
            Payload = JsonSerializer.Serialize(new { PartyId = partyId, RoleId = roleId, RoleName = role.Name })
        });
    }

    public async Task<IEnumerable<RoleDto>> GetRolesAsync()
    {
        var roles = await _roleRepository.GetAllAsync();
        return roles.Select(r => new RoleDto { Id = r.Id, Name = r.Name });
    }

    private static PartyDto MapToDto(Domain.Entities.Party party)
    {
        return new PartyDto
        {
            Id = party.Id,
            FirstName = party.FirstName,
            LastName = party.LastName,
            Email = party.Email,
            CreatedAt = party.CreatedAt,
            UpdatedAt = party.UpdatedAt,
            Roles = party.PartyRoles.Select(pr => new RoleDto
            {
                Id = pr.Role.Id,
                Name = pr.Role.Name
            }).ToList()
        };
    }
}

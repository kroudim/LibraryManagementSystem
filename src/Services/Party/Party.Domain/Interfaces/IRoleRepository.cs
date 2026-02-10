using Party.Domain.Entities;

namespace Party.Domain.Interfaces;

public interface IRoleRepository
{
    Task<Role?> GetByIdAsync(Guid id);
    Task<IEnumerable<Role>> GetAllAsync();
    Task<bool> IsRoleAssignedAsync(Guid partyId, Guid roleId);
    Task AssignRoleAsync(PartyRole partyRole);
    Task RemoveRoleAsync(Guid partyId, Guid roleId);
}

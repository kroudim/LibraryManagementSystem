using Party.Domain.Entities;

namespace Party.Domain.Interfaces;

public interface IPartyRepository
{
    Task<Entities.Party?> GetByIdAsync(Guid id);
    Task<IEnumerable<Entities.Party>> GetAllAsync();
    Task<Entities.Party> AddAsync(Entities.Party party);
    Task UpdateAsync(Entities.Party party);
    Task DeleteAsync(Guid id);
    Task<bool> EmailExistsAsync(string email, Guid? excludePartyId = null);
}

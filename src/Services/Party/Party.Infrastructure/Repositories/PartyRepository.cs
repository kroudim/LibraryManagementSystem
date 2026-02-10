using Microsoft.EntityFrameworkCore;
using Party.Domain.Interfaces;
using Party.Infrastructure.Data;

namespace Party.Infrastructure.Repositories;

public class PartyRepository : IPartyRepository
{
    private readonly PartyDbContext _context;

    public PartyRepository(PartyDbContext context)
    {
        _context = context;
    }

    public async Task<Domain.Entities.Party?> GetByIdAsync(Guid id)
    {
        return await _context.Parties
            .Include(p => p.PartyRoles)
            .ThenInclude(pr => pr.Role)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Domain.Entities.Party>> GetAllAsync()
    {
        return await _context.Parties
            .Include(p => p.PartyRoles)
            .ThenInclude(pr => pr.Role)
            .ToListAsync();
    }

    public async Task<Domain.Entities.Party> AddAsync(Domain.Entities.Party party)
    {
        _context.Parties.Add(party);
        await _context.SaveChangesAsync();
        return party;
    }

    public async Task UpdateAsync(Domain.Entities.Party party)
    {
        _context.Parties.Update(party);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var party = await _context.Parties.FindAsync(id);
        if (party != null)
        {
            _context.Parties.Remove(party);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> EmailExistsAsync(string email, Guid? excludePartyId = null)
    {
        return await _context.Parties
            .AnyAsync(p => p.Email == email && (excludePartyId == null || p.Id != excludePartyId));
    }
}

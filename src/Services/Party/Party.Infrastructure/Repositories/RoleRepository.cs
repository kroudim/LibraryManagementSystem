using Microsoft.EntityFrameworkCore;
using Party.Domain.Entities;
using Party.Domain.Interfaces;
using Party.Infrastructure.Data;

namespace Party.Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly PartyDbContext _context;

    public RoleRepository(PartyDbContext context)
    {
        _context = context;
    }

    public async Task<Role?> GetByIdAsync(Guid id)
    {
        return await _context.Roles.FindAsync(id);
    }

    public async Task<IEnumerable<Role>> GetAllAsync()
    {
        return await _context.Roles.ToListAsync();
    }

    public async Task<bool> IsRoleAssignedAsync(Guid partyId, Guid roleId)
    {
        return await _context.PartyRoles
            .AnyAsync(pr => pr.PartyId == partyId && pr.RoleId == roleId);
    }

    public async Task AssignRoleAsync(PartyRole partyRole)
    {
        _context.PartyRoles.Add(partyRole);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveRoleAsync(Guid partyId, Guid roleId)
    {
        var partyRole = await _context.PartyRoles
            .FirstOrDefaultAsync(pr => pr.PartyId == partyId && pr.RoleId == roleId);
        
        if (partyRole != null)
        {
            _context.PartyRoles.Remove(partyRole);
            await _context.SaveChangesAsync();
        }
    }
}

namespace Party.Domain.Entities;

public class PartyRole
{
    public Guid PartyId { get; set; }
    public Party Party { get; set; } = null!;
    
    public Guid RoleId { get; set; }
    public Role Role { get; set; } = null!;
    
    public DateTime AssignedAt { get; set; }
}

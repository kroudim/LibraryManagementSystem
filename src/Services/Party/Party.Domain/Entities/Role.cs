namespace Party.Domain.Entities;

public class Role
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    public ICollection<PartyRole> PartyRoles { get; set; } = new List<PartyRole>();
}

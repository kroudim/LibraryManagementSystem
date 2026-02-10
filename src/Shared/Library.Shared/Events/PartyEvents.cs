namespace Library.Shared.Events;

public class PartyCreated : BaseEvent
{
    public Guid PartyId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class PartyUpdated : BaseEvent
{
    public Guid PartyId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class PartyDeleted : BaseEvent
{
    public Guid PartyId { get; set; }
}

public class RoleAssigned : BaseEvent
{
    public Guid PartyId { get; set; }
    public Guid RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;
}

public class RoleRemoved : BaseEvent
{
    public Guid PartyId { get; set; }
    public Guid RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;
}

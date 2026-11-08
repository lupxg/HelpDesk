using HelpDesk.Domain.Common;

namespace HelpDesk.Domain.Entities;

public class SupportTeam : BaseEntity<Guid>
{
    public string Name { get; set; }
}

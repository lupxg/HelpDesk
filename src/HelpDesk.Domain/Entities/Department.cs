using HelpDesk.Domain.Common;

namespace HelpDesk.Domain.Entities;

public class Department : BaseEntity<Guid>
{
    public string Name { get; set; } = string.Empty;
}

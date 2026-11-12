using HelpDesk.Domain.Common;

namespace HelpDesk.Domain.Entities;


/// <summary>
/// This entity represent a team to which a team member is assignet to. 
/// </summary>
public class SupportTeam : BaseEntity<Guid>
{
    public string Name { get; private set; } = string.Empty;

    // EF core
    private SupportTeam() { }

    public SupportTeam(string name) : base(Guid.NewGuid()) 
    {
        Name = ValidateName(name);
    }

    public void Rename(string newName)
    {
        Name = ValidateName(newName);
    }

    private static string ValidateName(string name)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(name, nameof(name));

        return name.Trim();
    }
}

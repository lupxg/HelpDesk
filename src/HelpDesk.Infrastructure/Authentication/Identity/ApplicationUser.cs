using Microsoft.AspNetCore.Identity;

namespace HelpDesk.Infrastructure.Authentication.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
    public Guid? DepartmentId { get; set; }
    public bool IsActive { get; set; }
}

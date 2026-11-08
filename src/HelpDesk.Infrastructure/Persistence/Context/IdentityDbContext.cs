using HelpDesk.Infrastructure.Authentication.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Infrastructure.Persistence.Context;

public class IdentityDbContext(DbContextOptions options) : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        #region Custom Identity table names
        
            builder.Entity<ApplicationUser>(entity => { entity.ToTable(name: "Users"); });

            builder.Entity<IdentityRole>(entity => { entity.ToTable(name: "Roles"); });

            builder.Entity<IdentityUserRole<string>>(entity => { entity.ToTable(name: "UserRoles"); });

            builder.Entity<IdentityUserClaim<string>>(entity => { entity.ToTable(name: "UserClaims"); });

            builder.Entity<IdentityUserLogin<string>>(entity => { entity.ToTable(name: "UserLogins"); });

            builder.Entity<IdentityRoleClaim<string>>(entity => { entity.ToTable(name: "RoleClaims"); });

            builder.Entity<IdentityUserToken<string>>(entity => { entity.ToTable(name: "UserTokens"); });

        #endregion
    }
}

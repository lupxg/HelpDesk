using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using HelpDesk.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(
            options => options.UseNpgsql(
                connectionString: configuration.GetConnectionString("HelpDeskConnection") ?? throw new ArgumentNullException("Empty connection string")));

        services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>(tags: ["PostgreSQL", "ApiDbContext"]);

        return services;
    }
}

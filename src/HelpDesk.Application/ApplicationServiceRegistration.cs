using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;


namespace HelpDesk.Application;
public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }
}

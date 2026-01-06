using ExpressedRealms.Admin.Repository.ActivityLogs;
using Microsoft.Extensions.DependencyInjection;

namespace ExpressedRealms.Admin.Repository;

public static class AdminRepositoryInjections
{
    public static IServiceCollection AddAdminRepositoryInjections(this IServiceCollection services)
    {
        services.AddScoped<IUsersRepository, UsersRepository>();
        services.AddScoped<IActivityLogRepository, ActivityLogRepository>();
        services.AddScoped<IRolesRepository, RolesRepository>();

        return services;
    }
}

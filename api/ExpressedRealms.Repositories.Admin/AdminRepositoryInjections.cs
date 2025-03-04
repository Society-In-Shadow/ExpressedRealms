using ExpressedRealms.Repositories.Admin.ActivityLogs;
using Microsoft.Extensions.DependencyInjection;

namespace ExpressedRealms.Repositories.Admin;

public static class AdminRepositoryInjections
{
    public static IServiceCollection AddAdminRepositoryInjections(this IServiceCollection services)
    {
        services.AddScoped<IUsersRepository, UsersRepository>();
        services.AddScoped<IActivityLogRepository, ActivityLogRepository>();

        return services;
    }
}

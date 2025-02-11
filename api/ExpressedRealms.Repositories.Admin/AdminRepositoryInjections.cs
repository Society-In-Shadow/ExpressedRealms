﻿using Microsoft.Extensions.DependencyInjection;

namespace ExpressedRealms.Repositories.Admin;

public static class AdminRepositoryInjections
{
    public static IServiceCollection AddAdminRepositoryInjections(
        this IServiceCollection services
    )
    {
        services.AddScoped<IUsersRepository, UsersRepository>();
        

        return services;
    }
}
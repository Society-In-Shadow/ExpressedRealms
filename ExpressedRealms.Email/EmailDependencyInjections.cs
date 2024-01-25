using ExpressedRealms.Email;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SendGrid.Extensions.DependencyInjection;

public static class EmailDependencyInjections
{
    public static IServiceCollection AddEmailDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<ISendGridEmail, SendGridEmail>();
        services.AddTransient<IEmailSender, IdentityEmailSender>();
        services.AddSendGrid((options) =>
        {
            options.ApiKey = configuration["SENDGRID_API_KEY"];
            options.Host = configuration["SENDGRID_HOST"];
        });
        return services;
    }
}
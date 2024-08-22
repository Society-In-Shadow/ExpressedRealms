using ExpressedRealms.Email;
using ExpressedRealms.Email.IdentityEmails;
using ExpressedRealms.Email.IdentityEmails.ConfirmAccountEmail;
using ExpressedRealms.Email.IdentityEmails.ForgotPasswordEmail;
using ExpressedRealms.Email.SendGridTestEmail;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class EmailDependencyInjections
{
    public static IServiceCollection AddEmailDependencies(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddScoped<IEmailClientAdapter, EmailClientAdapter>();
        
        services.AddTransient<IEmailSender, IdentityEmailSender>();
        services.InjectIndividualEmails();
        return services;
    }

    private static void InjectIndividualEmails(this IServiceCollection services)
    {
        services.AddTransient<ISendGridEmail, SendGridEmail>();
        services.AddTransient<IForgetPasswordEmail, ForgetPasswordEmail>();
        services.AddTransient<IConfirmAccountEmail, ConfirmAccountEmail>();
    }
}

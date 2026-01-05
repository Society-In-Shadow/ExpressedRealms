using ExpressedRealms.Email.EmailClientAdapter;
using ExpressedRealms.Email.IdentityEmails;
using ExpressedRealms.Email.IdentityEmails.ConfirmAccountEmail;
using ExpressedRealms.Email.IdentityEmails.ForgotPasswordEmail;
using ExpressedRealms.Email.TestEmail;
using ExpressedRealms.Shared.AzureKeyVault;
using ExpressedRealms.Shared.AzureKeyVault.Secrets;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ExpressedRealms.Email;

public static class EmailDependencyInjections
{
    public static async Task<IServiceCollection> AddEmailDependencies(
        this IServiceCollection services,
        EarlyKeyVaultManager keyVaultManager
    )
    {
        if (!await keyVaultManager.IsSecretSet(EmailSettings.SMTPServer))
        {
            services.AddTransient<IEmailClientAdapter, EmailClientAdapter.EmailClientAdapter>();
        }
        else
        {
            services.AddTransient<IEmailClientAdapter, LocalAdapter>();
        }

        services.AddTransient<IEmailSender, IdentityEmailSender>();
        services.InjectIndividualEmails();
        return services;
    }

    private static void InjectIndividualEmails(this IServiceCollection services)
    {
        services.AddTransient<ITestEmail, TestEmail.TestEmail>();
        services.AddTransient<IForgetPasswordEmail, ForgetPasswordEmail>();
        services.AddTransient<IConfirmAccountEmail, ConfirmAccountEmail>();
    }
}

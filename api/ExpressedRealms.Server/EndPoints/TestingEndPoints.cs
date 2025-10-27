using ExpressedRealms.Authentication;
using ExpressedRealms.Email.TestEmail;
using ExpressedRealms.Events.API.Discord;
using ExpressedRealms.FeatureFlags;
using ExpressedRealms.FeatureFlags.FeatureClient;
using ExpressedRealms.Server.Shared;

namespace ExpressedRealms.Server.EndPoints;

internal static class TestingEndPoints
{
    internal static void AddTestingEndPoints(this WebApplication app)
    {
        app.MapGet(
                "/sendTestEmail",
                async (HttpContext httpContext, ITestEmail email) =>
                {
                    var user = httpContext.User;

                    // Retrieve the email from claims
                    var emailClaim = user
                        .Claims.FirstOrDefault(c => c.Type.Contains("email"))
                        ?.Value;

                    if (string.IsNullOrEmpty(emailClaim))
                    {
                        // If the email claim is missing or empty, return a bad request
                        return Results.BadRequest("Email claim is missing.");
                    }

                    await email.SendTestEmail(emailClaim);
                    return Results.Ok();
                }
            )
            .RequireAuthorization()
            .RequirePolicyAuthorization(Policies.UserManagementPolicy);

        app.MapGet(
                "/getFeatureFlag",
                async (IFeatureToggleClient client) =>
                {
                    return await client.HasFeatureFlag(ReleaseFlags.TestReleaseFlag);
                }
            )
            .RequireAuthorization();

        app.MapGet(
                "/sendDiscordTestMessage",
                async (IDiscordService discordService) =>
                {
                    await discordService.SendMessageToChannelAsync(
                        DiscordChannel.DevTestingChannel,
                        "Test"
                    );
                }
            )
            .RequireAuthorization();
    }
}

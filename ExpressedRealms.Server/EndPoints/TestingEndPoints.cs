using ExpressedRealms.Email.SendGridTestEmail;

namespace ExpressedRealms.Server.EndPoints;

internal static class TestingEndPoints
{
    internal static void AddTestingEndPoints(this WebApplication app)
    {
        app.MapGet(
            "/sendTestEmail",
            async (ISendGridEmail email) =>
            {
                await email.SendTestEmail();
                return Results.Ok();
            }
        );
    }
}

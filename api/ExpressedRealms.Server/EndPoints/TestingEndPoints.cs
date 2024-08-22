using ExpressedRealms.Email.TestEmail;

namespace ExpressedRealms.Server.EndPoints;

internal static class TestingEndPoints
{
    internal static void AddTestingEndPoints(this WebApplication app)
    {
        app.MapGet(
            "/sendTestEmail",
            async (ITestEmail email) =>
            {
                await email.SendTestEmail();
                return Results.Ok();
            }
        );
    }
}

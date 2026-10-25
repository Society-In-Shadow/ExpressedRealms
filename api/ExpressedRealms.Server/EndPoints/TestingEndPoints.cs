using ExpressedRealms.Authentication.PermissionCollection;
using ExpressedRealms.Authentication.PermissionCollection.Configuration;
using ExpressedRealms.DB;
using ExpressedRealms.Email.TestEmail;
using ExpressedRealms.Events.API.Discord;
using ExpressedRealms.Events.API.UseCases.Events.TriggerEventReminder;
using ExpressedRealms.FeatureFlags;
using ExpressedRealms.FeatureFlags.FeatureClient;
using ExpressedRealms.UseCases.Shared;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace ExpressedRealms.Server.EndPoints;

internal static class TestingEndPoints
{
    internal static void AddTestingEndPoints(this WebApplication app)
    {
        var endpoints = app.MapGroup("dev")
            .RequireAuthorization()
            .RequirePermission(Permissions.DevDebug.View);

        endpoints
            .MapPost(
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
            .RequirePermission(Permissions.DevDebug.SendTestEmail);

        endpoints
            .MapGet(
                "/getFeatureFlag",
                async (IFeatureToggleClient client) =>
                {
                    return await client.HasFeatureFlag(ReleaseFlags.TestReleaseFlag);
                }
            )
            .RequirePermission(Permissions.DevDebug.GetFeatureFlag);

        endpoints
            .MapPost(
                "/sendDiscordTestMessage",
                async (IDiscordService discordService) =>
                {
                    await discordService.SendMessageToChannelAsync(
                        DiscordChannel.DevTestingChannel,
                        "Test"
                    );
                }
            )
            .RequirePermission(Permissions.DevDebug.SendDiscordMessage);

        endpoints
            .MapPost(
                "/testRedis",
                async (IConnectionMultiplexer redisService) =>
                {
                    var db = redisService.GetDatabase();
                    await db.StringSetAsync("testKey", "testValue");
                    var value = await db.StringGetAsync("testKey");
                    return Results.Ok(value.ToString());
                }
            )
            .RequirePermission(Permissions.DevDebug.TestRedis);

        endpoints
            .MapPost("/updateLookup", () => Results.Ok())
            .RequirePermission(Permissions.DevDebug.RunSpecialScripts);

        endpoints
            .MapPost(
                "/rerunDailyMessage",
                async (IEventReminderHandlerUseCase eventReminderHandler) =>
                {
                    await eventReminderHandler.ExecuteAsync();
                    return Results.Ok();
                }
            )
            .RequirePermission(Permissions.DevDebug.RerunDailyUpdates);
        
        endpoints
            .MapPost(
                "/runPlanTextMigration",
                async (ExpressedRealmsDbContext dbContext) =>
                {
                    var rows = await dbContext.ExpressionSections
                        .ToListAsync();

                    foreach (var row in rows)
                    {
                        row.ProcessedPlainText = row.Content.ToPlainText();
                    }

                    await dbContext.SaveChangesAsync();
                }
            );
        
        app.MapGet("/search", async (
            string q,
            ExpressedRealmsDbContext db,
            CancellationToken cancellationToken) =>
        {
            var results = await db.ExpressionSections
                .Where(x => x.SearchVector.Matches(
                    EF.Functions.WebSearchToTsQuery("english", q)))
                .OrderByDescending(x => x.Name.ToLower() == q.ToLower())
                .ThenByDescending(x =>
                    x.SearchVector.Rank(
                        EF.Functions.WebSearchToTsQuery("english", q)))
                .Select(x => new
                {
                    EntryId = x.Id,
                    Section = x.Expression.CmsType.Name,
                    ExpressionName = x.Expression.Name,
                    SectionName = x.Name,
                    SectionContent = x.Content
                })
                .Take(6)
                .ToListAsync(cancellationToken);
            return Results.Ok(results);
        });

    }
}

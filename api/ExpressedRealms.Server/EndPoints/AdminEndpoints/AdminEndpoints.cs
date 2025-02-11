using ExpressedRealms.Authentication;
using ExpressedRealms.Repositories.Admin;
using ExpressedRealms.Server.Extensions;
using Microsoft.AspNetCore.Authorization;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace ExpressedRealms.Server.EndPoints.AdminEndpoints;

public static class AdminEndpoints
{
    internal static void AddAdminEndPoints(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("admin")
            .AddFluentValidationAutoValidation()
            .WithTags("admin")
            .RequirePolicyAuthorization(Policies.UserManagementPolicy)
            .WithOpenApi();

        endpointGroup
            .MapGet(
                "users",
                [Authorize] async (IUsersRepository repository) =>
                    TypedResults.Ok(await repository.GetUsersAsync())
            )
            .RequireAuthorization();
    }
}
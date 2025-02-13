using ExpressedRealms.Authentication;
using ExpressedRealms.Repositories.Admin;
using ExpressedRealms.Server.EndPoints.AdminEndpoints.Dtos;
using ExpressedRealms.Server.EndPoints.AdminEndpoints.Response;
using ExpressedRealms.Server.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
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
                [Authorize]
                async Task<Ok<UserListResponse>> (IUsersRepository repository) =>
                {
                    var users = await repository.GetUsersAsync();

                    return TypedResults.Ok(
                        new UserListResponse()
                        {
                            Users = users
                                .Select(x => new UserListItem()
                                {
                                    Id = x.Id,
                                    Email = x.Email,
                                    Username = x.Username,
                                })
                                .ToList(),
                        }
                    );
                }
            )
            .RequireAuthorization();
    }
}

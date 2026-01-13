using ExpressedRealms.Admin.UseCases.Users.GetUsersForRole;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Admin.API.RolesEndpoints.GetUsersForRole;

public static class GetUsersForRoleEndpoint
{
    public static async Task<Results<Ok<RoleListResponse>, ValidationProblem, NotFound>> Execute(
        int id,
        [FromServices] IGetUsersForRoleUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(new GetUsersForRoleModel() { RoleId = id });

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        if (results.HasNotFound(out var notFound))
            return notFound;
        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok(
            new RoleListResponse()
            {
                Roles = results
                    .Value.Select(x => new UserMapping()
                    {
                        Name = x.Name,
                        ExpireDate = x.ExpireDate,
                        UserId = x.UserId,
                    })
                    .OrderBy(x => x.Name)
                    .ToList(),
            }
        );
    }
}

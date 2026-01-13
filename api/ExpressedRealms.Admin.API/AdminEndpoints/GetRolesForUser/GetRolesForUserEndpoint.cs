using ExpressedRealms.Admin.UseCases.Roles.GetRolesForUser;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Admin.API.AdminEndpoints.GetRolesForUser;

public static class GetRolesForUserEndpoint
{
    public static async Task<Results<Ok<RoleListResponse>, ValidationProblem, NotFound>> Execute(
        string userId,
        [FromServices] IGetRolesForUserUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(new GetRolesForUserModel() { UserId = userId });

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        if (results.HasNotFound(out var notFound))
            return notFound;
        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok(
            new RoleListResponse()
            {
                Roles = results
                    .Value.Select(x => new RoleMapping()
                    {
                        Name = x.Name,
                        ExpireDate = x.ExpireDate,
                        RoleId = x.RoleId,
                        Description = x.Description,
                    })
                    .OrderBy(x => x.Name)
                    .ToList(),
            }
        );
    }
}

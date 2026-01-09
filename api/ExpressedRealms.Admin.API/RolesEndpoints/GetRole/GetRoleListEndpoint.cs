using ExpressedRealms.Admin.UseCases.Roles.GetRole;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Admin.API.RolesEndpoints.GetRole;

public static class GetRoleEndpoint
{
    public static async Task<Results<Ok<RoleResponse>, ValidationProblem, NotFound>> Execute(
        int id,
        IGetRoleUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(new GetRoleModel() { Id = id });

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        if (results.HasNotFound(out var notFound))
            return notFound;
        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok(
            new RoleResponse()
            {
                Id = results.Value.Id,
                Name = results.Value.Name,
                Description = results.Value.Description,
                PermissionIds = results.Value.PermissionIds,
            }
        );
    }
}

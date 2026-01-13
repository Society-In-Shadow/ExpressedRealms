using ExpressedRealms.Admin.UseCases.Roles.DeleteUserRole;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Admin.API.RolesEndpoints.DeleteUserRoleMapping;

public static class DeleteUserRoleMappingEndpoint
{
    public static async Task<Results<Ok, ValidationProblem, NotFound>> Execute(
        int id,
        string userId,
        IDeleteUserRoleUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(new() { RoleId = id, UserId = userId });

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        if (results.HasNotFound(out var notFound))
            return notFound;
        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok();
    }
}

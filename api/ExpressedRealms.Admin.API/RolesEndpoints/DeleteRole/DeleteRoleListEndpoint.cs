using ExpressedRealms.Admin.UseCases.Roles.DeleteRole;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Admin.API.RolesEndpoints.DeleteRole;

public static class DeleteRoleEndpoint
{
    public static async Task<Results<Ok, ValidationProblem, NotFound>> Execute(
        int id,
        IDeleteRoleUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(new DeleteRoleModel() { Id = id });

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        if (results.HasNotFound(out var notFound))
            return notFound;
        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok();
    }
}

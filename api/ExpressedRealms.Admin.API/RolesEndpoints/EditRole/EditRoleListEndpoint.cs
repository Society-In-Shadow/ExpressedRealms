using ExpressedRealms.Admin.UseCases.Roles.EditRole;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Admin.API.RolesEndpoints.EditRole;

public static class EditRoleEndpoint
{
    public static async Task<Results<Ok, ValidationProblem, NotFound>> Execute(
        int id,
        RoleRequest request,
        IEditRoleUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(
            new EditRoleModel()
            {
                Id = id,
                Name = request.Name,
                Description = request.Description,
                PermissionIds = request.PermissionIds,
            }
        );

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        if (results.HasNotFound(out var notFound))
            return notFound;
        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok();
    }
}

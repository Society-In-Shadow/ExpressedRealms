using ExpressedRealms.Admin.UseCases.Roles.AddRole;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Admin.API.RolesEndpoints.AddRole;

public static class AddRoleEndpoint
{
    public static async Task<Results<Ok, ValidationProblem, NotFound>> Execute(
        RoleRequest request,
        IAddRoleUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(new AddRoleModel()
        {
            Name = request.Name, 
            Description = request.Description, 
            PermissionIds = request.PermissionIds
        });

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        if (results.HasNotFound(out var notFound))
            return notFound;
        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok();
    }
}

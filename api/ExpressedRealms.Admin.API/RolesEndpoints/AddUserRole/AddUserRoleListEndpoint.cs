using ExpressedRealms.Admin.UseCases.Roles.AddUserToRole;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Admin.API.RolesEndpoints.AddUserRole;

public static class AddUserRoleEndpoint
{
    public static async Task<Results<Ok, ValidationProblem, NotFound>> Execute(
        int roleId,
        UserRoleRequest request,
        IAddUserToRoleUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(
            new AddUserToRoleModel()
            {
                UserId = request.UserId,
                RoleId = roleId,
                ExpireDate = request.ExpireDate,
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

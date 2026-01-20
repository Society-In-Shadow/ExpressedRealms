using ExpressedRealms.Admin.UseCases.Roles.AddUserToRole;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Admin.API.AdminEndpoints.AddRoleUser;

public static class AddRoleUserEndpoint
{
    public static async Task<Results<Ok, ValidationProblem, NotFound>> Execute(
        string userId,
        RoleUserRequest request,
        IAddUserToRoleUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(
            new AddUserToRoleModel()
            {
                UserId = userId,
                RoleId = request.RoleId,
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

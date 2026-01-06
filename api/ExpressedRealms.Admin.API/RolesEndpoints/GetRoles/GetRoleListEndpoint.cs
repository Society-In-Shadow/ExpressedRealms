using ExpressedRealms.Admin.UseCases.Roles.Get;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Admin.API.RolesEndpoints.GetRoles;

public static class GetRoleListEndpoint
{
    public static async Task<Ok<RoleResponse>> Execute(IGetRolesUseCase useCase)
    {
        var roles = await useCase.ExecuteAsync();

        return TypedResults.Ok(
            new RoleResponse()
            {
                Roles = roles
                    .Value.Roles.Select(x => new Role()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                    })
                    .ToList(),
            }
        );
    }
}

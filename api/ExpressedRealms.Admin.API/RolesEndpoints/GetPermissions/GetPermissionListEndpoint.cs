using ExpressedRealms.Admin.UseCases.Roles.GetPermissions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Admin.API.RolesEndpoints.GetPermissions;

public static class GetPermissionsEndpoint
{
    public static async Task<Ok<PermissionsBaseResponse>> Execute(IGetPermissionsUseCase useCase)
    {
        var roles = await useCase.ExecuteAsync();

        return TypedResults.Ok(
            new PermissionsBaseResponse()
            {
                Resources = roles
                    .Value.Resources.Select(x => new ResourceGroup()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        Permissions = x
                            .Permissions.Select(y => new Permission()
                            {
                                Name = y.Name,
                                Description = y.Description,
                                Id = y.Id,
                            })
                            .ToList(),
                    })
                    .ToList(),
            }
        );
    }
}

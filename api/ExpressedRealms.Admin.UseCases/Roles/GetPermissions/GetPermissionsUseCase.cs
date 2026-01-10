using ExpressedRealms.Admin.Repository;
using FluentResults;

namespace ExpressedRealms.Admin.UseCases.Roles.GetPermissions;

internal sealed class GetPermissionsUseCase(IRolesRepository rolesRepository)
    : IGetPermissionsUseCase
{
    public async Task<Result<PermissionsBaseReturnModel>> ExecuteAsync()
    {
        var roles = await rolesRepository.GetPermissionResourcesForList();

        return Result.Ok(
            new PermissionsBaseReturnModel()
            {
                Resources = roles
                    .Select(x => new ResourceGroup()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        Permissions = x
                            .Permissions.Select(y => new Permission()
                            {
                                Id = y.Id,
                                Name = y.Name,
                                Description = y.Description,
                            })
                            .ToList(),
                    })
                    .ToList(),
            }
        );
    }
}

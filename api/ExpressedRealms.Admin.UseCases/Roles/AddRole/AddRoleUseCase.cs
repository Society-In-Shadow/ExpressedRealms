using ExpressedRealms.Admin.Repository;
using ExpressedRealms.DB.Models.Authorization.RolePermissionMappingSetup;
using ExpressedRealms.DB.Models.Authorization.RoleSetup;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Admin.UseCases.Roles.AddRole;

internal sealed class AddRoleUseCase(
    IRolesRepository rolesRepository,
    AddRoleModelValidator validator,
    CancellationToken cancellationToken
) : IAddRoleUseCase
{
    public async Task<Result> ExecuteAsync(AddRoleModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        await rolesRepository.AddAsync(
            new Role()
            {
                Name = model.Name,
                Description = model.Description,
                RolePermissionMappings = model
                    .PermissionIds.Select(x => new RolePermissionMapping() { PermissionId = x })
                    .ToList(),
            }
        );

        return Result.Ok();
    }
}

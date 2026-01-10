using ExpressedRealms.Admin.Repository;
using ExpressedRealms.DB.Helpers;
using ExpressedRealms.DB.Models.Authorization.RolePermissionMappingSetup;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Admin.UseCases.Roles.EditRole;

internal sealed class EditRoleUseCase(
    IRolesRepository rolesRepository,
    EditRoleModelValidator validator,
    CancellationToken cancellationToken
) : IEditRoleUseCase
{
    public async Task<Result> ExecuteAsync(EditRoleModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var role = await rolesRepository.GetRoleForEditAsync(model.Id);

        role.Name = model.Name;
        role.Description = model.Description;

        CollectionHelpers.Sync(
            role.RolePermissionMappings,
            model.PermissionIds,
            rp => rp.PermissionId,
            permissionId => new RolePermissionMapping { PermissionId = permissionId }
        );

        await rolesRepository.EditAsync(role);

        return Result.Ok();
    }
}

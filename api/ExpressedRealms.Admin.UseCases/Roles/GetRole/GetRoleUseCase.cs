using ExpressedRealms.Admin.Repository;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Admin.UseCases.Roles.GetRole;

internal sealed class GetRoleUseCase(
    IRolesRepository rolesRepository,
    GetRoleModelValidator validator, 
    CancellationToken cancellationToken) : IGetRoleUseCase
{
    public async Task<Result<RoleBaseReturnModel>> ExecuteAsync(GetRoleModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );
        
        if (result.IsFailed)
            return Result.Fail(result.Errors);
        
        var role = await rolesRepository.GetRoleForEditView(model.Id);

        return Result.Ok(
            new RoleBaseReturnModel()
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
                PermissionIds = role.PermissionIds
            }
        );
    }
}

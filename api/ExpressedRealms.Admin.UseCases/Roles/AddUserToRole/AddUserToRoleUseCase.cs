using ExpressedRealms.Admin.Repository;
using ExpressedRealms.DB.Models.Authorization.UserRoleMappingSetup;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Admin.UseCases.Roles.AddUserToRole;

internal sealed class AddUserToRoleUseCase(
    IRolesRepository rolesRepository,
    AddUserToRoleModelValidator validator,
    CancellationToken cancellationToken
) : IAddUserToRoleUseCase
{
    public async Task<Result> ExecuteAsync(AddUserToRoleModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var existingMapping = await rolesRepository.RoleUserMappingExistsAsync(
            model.RoleId,
            model.UserId
        );
        if (existingMapping)
            return Result.Ok();

        await rolesRepository.AddUserRoleMappingAsync(
            new UserRoleMapping()
            {
                RoleId = model.RoleId,
                UserId = model.UserId,
                ExpireDate = model.ExpireDate,
            }
        );

        return Result.Ok();
    }
}

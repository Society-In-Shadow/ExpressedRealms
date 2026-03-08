using ExpressedRealms.Admin.Repository;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Admin.UseCases.Roles.DeleteUserRole;

internal sealed class DeleteUserRoleUseCase(
    IRolesRepository rolesRepository,
    DeleteUserRoleModelValidator validator,
    CancellationToken cancellationToken
) : IDeleteUserRoleUseCase
{
    public async Task<Result> ExecuteAsync(DeleteUserRoleModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var mapping = await rolesRepository.GetUserRoleMappingAsync(model.RoleId, model.UserId);
        mapping.SoftDelete();
        await rolesRepository.EditAsync(mapping);

        return Result.Ok();
    }
}

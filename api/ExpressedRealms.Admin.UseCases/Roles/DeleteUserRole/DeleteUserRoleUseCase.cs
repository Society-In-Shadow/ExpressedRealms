using ExpressedRealms.Admin.Repository;
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

        await rolesRepository.DeleteRoleUserMappingAsync(model.RoleId, model.UserId);

        return Result.Ok();
    }
}

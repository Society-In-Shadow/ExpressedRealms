using ExpressedRealms.Admin.Repository;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Admin.UseCases.Roles.DeleteRole;

internal sealed class DeleteRoleUseCase(
    IRolesRepository rolesRepository,
    DeleteRoleModelValidator validator,
    CancellationToken cancellationToken
) : IDeleteRoleUseCase
{
    public async Task<Result> ExecuteAsync(DeleteRoleModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var role = await rolesRepository.GetRoleForEditAsync(model.Id);

        role.SoftDelete();

        await rolesRepository.EditAsync(role);

        return Result.Ok();
    }
}

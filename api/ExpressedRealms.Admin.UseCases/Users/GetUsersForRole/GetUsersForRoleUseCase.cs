using ExpressedRealms.Admin.Repository;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Admin.UseCases.Users.GetUsersForRole;

internal sealed class GetUsersForRoleUseCase(
    IRolesRepository rolesRepository,
    GetUsersForRoleModelValidator validator,
    CancellationToken cancellationToken
) : IGetUsersForRoleUseCase
{
    public async Task<Result<List<RoleForUserMappingReturnModel>>> ExecuteAsync(
        GetUsersForRoleModel model
    )
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var roles = await rolesRepository.GetUsersForRoleAsync(model.RoleId);

        return roles
            .Select(x => new RoleForUserMappingReturnModel
            {
                UserId = x.UserId,
                Name = x.Name,
                ExpireDate = x.ExpireDate,
            })
            .ToList();
    }
}

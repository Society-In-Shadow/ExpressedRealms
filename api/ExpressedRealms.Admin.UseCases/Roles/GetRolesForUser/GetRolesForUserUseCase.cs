using ExpressedRealms.Admin.Repository;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Admin.UseCases.Roles.GetRolesForUser;

internal sealed class GetRolesForUserUseCase(
    IRolesRepository rolesRepository,
    GetRolesForUserModelValidator validator,
    CancellationToken cancellationToken
) : IGetRolesForUserUseCase
{
    public async Task<Result<List<RoleForUserMappingReturnModel>>> ExecuteAsync(
        GetRolesForUserModel model
    )
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var roles = await rolesRepository.GetRolesForUserAsync(model.UserId);

        return roles
            .Select(x => new RoleForUserMappingReturnModel
            {
                RoleId = x.RoleId,
                Name = x.Name,
                ExpireDate = x.ExpireDate,
                Description = x.Description,
            })
            .ToList();
    }
}

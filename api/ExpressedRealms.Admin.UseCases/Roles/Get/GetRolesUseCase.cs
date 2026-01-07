using ExpressedRealms.Admin.Repository;
using FluentResults;

namespace ExpressedRealms.Admin.UseCases.Roles.Get;

internal sealed class GetRolesUseCase(IRolesRepository rolesRepository) : IGetRolesUseCase
{
    public async Task<Result<RolesBaseReturnModel>> ExecuteAsync()
    {
        var roles = await rolesRepository.GetRolesForListAsync();

        return Result.Ok(
            new RolesBaseReturnModel()
            {
                Roles = roles
                    .Select(x => new RoleModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                    })
                    .ToList(),
            }
        );
    }
}

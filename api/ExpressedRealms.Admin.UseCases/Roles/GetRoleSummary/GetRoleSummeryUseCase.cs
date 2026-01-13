using ExpressedRealms.Admin.Repository;
using ExpressedRealms.DB.Shared;
using FluentResults;

namespace ExpressedRealms.Admin.UseCases.Roles.GetRoleSummary;

internal sealed class GetRoleSummaryUseCase(
    IRolesRepository rolesRepository
) : IGetRoleSummaryUseCase
{
    public async Task<Result<List<GenericListDto<int>>>> ExecuteAsync()
    {
        return await rolesRepository.GetRoleSummaryForListAsync();
    }
}

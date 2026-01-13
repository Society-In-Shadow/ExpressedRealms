using ExpressedRealms.Admin.Repository;
using ExpressedRealms.DB.Shared;
using FluentResults;

namespace ExpressedRealms.Admin.UseCases.Users.GetUserSummary;

internal sealed class GetUserSummaryUseCase(IUsersRepository repository) : IGetUserSummaryUseCase
{
    public async Task<Result<List<GenericListDto<string>>>> ExecuteAsync()
    {
        return await repository.GetUserSummaryAsync();
    }
}

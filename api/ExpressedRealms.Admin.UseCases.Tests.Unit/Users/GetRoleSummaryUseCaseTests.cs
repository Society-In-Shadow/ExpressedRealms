using ExpressedRealms.Admin.Repository;
using ExpressedRealms.Admin.UseCases.Users.GetUserSummary;
using ExpressedRealms.DB.Shared;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Admin.UseCases.Tests.Unit.Users;

public class GetUserSummaryUseCaseTests
{
    private readonly GetUserSummaryUseCase _useCase;
    private readonly List<GenericListDto<string>> _dbModel;

    public GetUserSummaryUseCaseTests()
    {
        _dbModel = new List<GenericListDto<string>>();
        var repository = A.Fake<IUsersRepository>();

        A.CallTo(() => repository.GetUserSummaryAsync()).Returns(_dbModel);

        _useCase = new GetUserSummaryUseCase(repository);
    }

    [Fact]
    public async Task UseCase_CreatesUser_WithExpectedValues()
    {
        var result = await _useCase.ExecuteAsync();

        Assert.Same(_dbModel, result.Value);
    }
}

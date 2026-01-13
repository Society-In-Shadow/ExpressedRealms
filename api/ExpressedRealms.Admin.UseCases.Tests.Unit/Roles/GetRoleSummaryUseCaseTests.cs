using ExpressedRealms.Admin.Repository;
using ExpressedRealms.Admin.UseCases.Roles.GetRoleSummary;
using ExpressedRealms.DB.Shared;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Admin.UseCases.Tests.Unit.Roles;

public class GetRoleSummaryUseCaseTests
{
    private readonly GetRoleSummaryUseCase _useCase;
    private readonly List<GenericListDto<int>> _dbModel;

    public GetRoleSummaryUseCaseTests()
    {
        _dbModel = new List<GenericListDto<int>>();
        var repository = A.Fake<IRolesRepository>();

        A.CallTo(() => repository.GetRoleSummaryForListAsync()).Returns(_dbModel);

        _useCase = new GetRoleSummaryUseCase(repository);
    }

    [Fact]
    public async Task UseCase_CreatesRole_WithExpectedValues()
    {
        var result = await _useCase.ExecuteAsync();

        Assert.Same(_dbModel, result.Value);
    }
}

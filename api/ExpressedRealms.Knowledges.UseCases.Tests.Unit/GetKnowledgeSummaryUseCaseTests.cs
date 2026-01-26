using ExpressedRealms.DB.Shared;
using ExpressedRealms.Knowledges.Repository.Knowledges;
using ExpressedRealms.Knowledges.UseCases.Knowledges.GetKnowledgeSummary;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Knowledges.UseCases.Tests.Unit;

public class GetKnowledgeSummaryUseCaseTests
{
    private readonly GetKnowledgeSummaryUseCase _useCase;
    private readonly List<GenericListDto<int>> _dbModel;

    public GetKnowledgeSummaryUseCaseTests()
    {
        _dbModel = new List<GenericListDto<int>>();
        var repository = A.Fake<IKnowledgeRepository>();

        A.CallTo(() => repository.GetKnowledgeSummaryAsync()).Returns(_dbModel);

        _useCase = new GetKnowledgeSummaryUseCase(repository);
    }

    [Fact]
    public async Task UseCase_CreatesUser_WithExpectedValues()
    {
        var result = await _useCase.ExecuteAsync();

        Assert.Same(_dbModel, result.Value);
    }
}

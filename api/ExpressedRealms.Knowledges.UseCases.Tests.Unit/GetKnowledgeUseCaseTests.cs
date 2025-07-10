using ExpressedRealms.DB.Models.Knowledges;
using ExpressedRealms.DB.Models.Knowledges.KnowledgeModels;
using ExpressedRealms.Knowledges.Repository.Knowledges;
using ExpressedRealms.Knowledges.UseCases.Knowledges.GetKnowledges;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Knowledges.UseCases.Tests.Unit;

public class GetKnowledgeUseCaseTests
{
    private readonly GetKnowledgesUseCase _useCase;
    private readonly IKnowledgeRepository _repository;

    public GetKnowledgeUseCaseTests()
    {
        _repository = A.Fake<IKnowledgeRepository>();

        _useCase = new GetKnowledgesUseCase(_repository);
    }

    [Fact]
    public async Task UseCase_Grabs_TheKnowledges()
    {
        await _useCase.ExecuteAsync();

        A.CallTo(() => _repository.GetKnowledges()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_Returns_FlattenedListOf_TheKnowledges_AndKnowledgeTypes()
    {
        var listTest = new List<Knowledge>()
        {
            new Knowledge()
            {
                Id = 1,
                Name = "Test Knowledge 1",
                Description = "Test Description 1",
                KnowledgeTypeId = 1,
                KnowledgeType = new KnowledgeType()
                {
                    Id = 1,
                    Name = "Test Knowledge Type 1",
                    Description = "Test Knowledge Type Description 1",
                },
            },
            new Knowledge()
            {
                Id = 2,
                Name = "Test Knowledge 2",
                Description = "Test Description 2",
                KnowledgeTypeId = 2,
                KnowledgeType = new KnowledgeType()
                {
                    Id = 2,
                    Name = "Test Knowledge Type 2",
                    Description = "Test Knowledge Type Description 2",
                },
            },
        };

        var expectedReturn = new KnowledgeReturnModel()
        {
            KnowledgeTypes = new List<KnowledgeModel>()
            {
                new()
                {
                    Id = 1,
                    Name = "Test Knowledge 1",
                    Description = "Test Description 1",
                    TypeId = 1,
                    TypeName = "Test Knowledge Type 1",
                    TypeDescription = "Test Knowledge Type Description 1",
                },
                new()
                {
                    Id = 2,
                    Name = "Test Knowledge 2",
                    Description = "Test Description 2",
                    TypeId = 2,
                    TypeName = "Test Knowledge Type 2",
                    TypeDescription = "Test Knowledge Type Description 2",
                },
            },
        };

        A.CallTo(() => _repository.GetKnowledges()).Returns(listTest);

        var results = await _useCase.ExecuteAsync();

        Assert.Equivalent(expectedReturn.KnowledgeTypes, results.Value.KnowledgeTypes);
    }
}

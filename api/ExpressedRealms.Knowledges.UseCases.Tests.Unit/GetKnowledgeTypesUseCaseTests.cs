using ExpressedRealms.DB.Models.Knowledges;
using ExpressedRealms.Knowledges.Repository.Knowledges;
using ExpressedRealms.Knowledges.UseCases.KnowledgeTypes.GetKnowledgeTypes;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Knowledges.UseCases.Tests.Unit;

public class GetKnowledgeTypesUseCaseTests
{
    private readonly GetKnowledgeTypesUseCase _useCase;
    private readonly IKnowledgeRepository _repository;

    private List<KnowledgeType> KnowledgeTypesDbModel { get; set; }

    public GetKnowledgeTypesUseCaseTests()
    {
        KnowledgeTypesDbModel = new List<KnowledgeType>()
        {
            new KnowledgeType()
            {
                Id = 1,
                Name = "Test Knowledge Type 1",
                Description = "Test Knowledge Type Description 1",
            },
            new KnowledgeType()
            {
                Id = 2,
                Name = "Test Knowledge Type 2",
                Description = "Test Knowledge Type Description 2",
            },
        };

        _repository = A.Fake<IKnowledgeRepository>();

        A.CallTo(() => _repository.GetKnowledgeTypesAsync()).Returns(KnowledgeTypesDbModel);

        _useCase = new GetKnowledgeTypesUseCase(_repository);
    }

    [Fact]
    public async Task UseCase_Grabs_TheKnowledgeTypes()
    {
        await _useCase.ExecuteAsync();

        A.CallTo(() => _repository.GetKnowledgeTypesAsync()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_Returns_AvailableKnowledgeTypes()
    {
        var results = await _useCase.ExecuteAsync();

        var knowledgeTypes = KnowledgeTypesDbModel
            .Select(x => new KnowledgeTypeModel()
            {
                Name = x.Name,
                Description = x.Description,
                Id = x.Id,
            })
            .ToList();

        Assert.Equivalent(knowledgeTypes, results.Value.KnowledgeTypes);
    }
}

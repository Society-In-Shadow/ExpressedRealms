using ExpressedRealms.DB.Models.Knowledges.KnowledgeEducationLevels;
using ExpressedRealms.Knowledges.Repository;
using ExpressedRealms.Knowledges.UseCases.KnowledgeLevels;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Knowledges.UseCases.Tests.Unit.KnowledgeTypeTests;

public class GetKnowledgeLevelsUseCaseTests
{
    private readonly GetKnowledgeLevelsUseCase _useCase;
    private readonly IKnowledgeLevelRepository _repository;

    private List<KnowledgeEducationLevel> KnowledgeTypesDbModel { get; set; }

    public GetKnowledgeLevelsUseCaseTests()
    {
        KnowledgeTypesDbModel = new List<KnowledgeEducationLevel>()
        {
            new KnowledgeEducationLevel()
            {
                Id = 1,
                Name = "Test Knowledge Type 1",
                TotalGeneralXpCost = 1,
                TotalUnknownXpCost = 2,
                UnknownXpCost = 3,
                GeneralXpCost = 4,
                Level = 1,
                StoneModifier = 6,
                SpecializationCount = 3,
            },
            new KnowledgeEducationLevel()
            {
                Id = 2,
                Name = "Test Knowledge Type 10",
                TotalGeneralXpCost = 3,
                TotalUnknownXpCost = 4,
                UnknownXpCost = 5,
                GeneralXpCost = 6,
                Level = 8,
                StoneModifier = 9,
                SpecializationCount = 10,
            },
        };

        _repository = A.Fake<IKnowledgeLevelRepository>();

        A.CallTo(() => _repository.GetKnowledgeLevels()).Returns(KnowledgeTypesDbModel);

        _useCase = new GetKnowledgeLevelsUseCase(_repository);
    }

    [Fact]
    public async Task UseCase_Grabs_TheKnowledgeLevels()
    {
        await _useCase.ExecuteAsync();

        A.CallTo(() => _repository.GetKnowledgeLevels()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_Returns_AvailableKnowledgeTypes()
    {
        var results = await _useCase.ExecuteAsync();

        var knowledgeTypes = KnowledgeTypesDbModel
            .Select(x => new KnowledgeLevelModel()
            {
                Id = x.Id,
                Name = x.Name,
                Level = x.Level,
                SpecializationCount = x.SpecializationCount,
                StoneModifier = x.StoneModifier,
                GeneralXpCost = x.GeneralXpCost,
                TotalGeneralXpCost = x.TotalGeneralXpCost,
                UnknownXpCost = x.UnknownXpCost,
                TotalUnknownXpCost = x.TotalUnknownXpCost,
            })
            .ToList();

        Assert.Equivalent(knowledgeTypes, results.Value.KnowledgeLevels);
    }
}

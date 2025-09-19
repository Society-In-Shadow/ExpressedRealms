using ExpressedRealms.Characters.Repository;
using ExpressedRealms.DB.Models.Knowledges.KnowledgeEducationLevels;
using ExpressedRealms.Knowledges.Repository;
using ExpressedRealms.Knowledges.UseCases.KnowledgeLevels;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Knowledges.UseCases.Tests.Unit.KnowledgeTypeTests;

public class GetKnowledgeLevelsUseCaseTests
{
    private readonly GetKnowledgeLevelsUseCase _useCase;
    private readonly IKnowledgeLevelRepository _repository;
    private readonly ICharacterRepository _characterRepository;
    private readonly GetKnowledgeLevelsModel _model;

    private List<KnowledgeEducationLevel> KnowledgeTypesDbModel { get; set; }

    public GetKnowledgeLevelsUseCaseTests()
    {
        _model = new GetKnowledgeLevelsModel() { CharacterId = 1 };

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
        _characterRepository = A.Fake<ICharacterRepository>();

        A.CallTo(() => _repository.GetKnowledgeLevels()).Returns(KnowledgeTypesDbModel);
        A.CallTo(() => _characterRepository.CharacterExistsAsync(_model.CharacterId)).Returns(true);

        var validator = new GetKnowledgeLevelsModelValidator(_characterRepository);

        _useCase = new GetKnowledgeLevelsUseCase(_repository, validator, CancellationToken.None);
    }

    [Fact]
    public async Task ValidationFor_CharacterId_WillFail_WhenItsEmpty()
    {
        _model.CharacterId = 0;
        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(GetKnowledgeLevelsModel.CharacterId),
            "Character Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_CharacterId_WillFail_WhenItDoesNotExist()
    {
        A.CallTo(() => _characterRepository.CharacterExistsAsync(_model.CharacterId))
            .Returns(false);
        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(GetKnowledgeLevelsModel.CharacterId),
            "The Character does not exist."
        );
    }

    [Fact]
    public async Task UseCase_Grabs_TheKnowledgeLevels()
    {
        await _useCase.ExecuteAsync(_model);

        A.CallTo(() => _repository.GetKnowledgeLevels()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_Returns_AvailableKnowledgeTypes()
    {
        var results = await _useCase.ExecuteAsync(_model);

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

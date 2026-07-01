using ExpressedRealms.DB.Models.Factions.FactionLevelModels;
using ExpressedRealms.DB.Models.Factions.FactionRankModels;
using ExpressedRealms.Expressions.Repository.FactionLevels;
using ExpressedRealms.Expressions.UseCases.FactionLevelUseCases.CreateFactionLevel;
using ExpressedRealms.Knowledges.Repository.Knowledges;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Expressions.UseCases.Tests.Unit.FactionLevels;

public class CreateFactionLevelUseCaseTests
{
    private readonly CreateFactionLevelUseCase _useCase;
    private readonly IFactionLevelRepository _factionLevelRepository;
    private readonly IKnowledgeRepository _knowledgeRepository;

    private readonly CreateFactionLevelModel _model;

    public CreateFactionLevelUseCaseTests()
    {
        _model = new CreateFactionLevelModel()
        {
            FactionId = 1,
            KnowledgeId = 2,
            Specialization = "Specialization"
        };

        _factionLevelRepository = A.Fake<IFactionLevelRepository>();
        _knowledgeRepository = A.Fake<IKnowledgeRepository>();

        A.CallTo(() => _knowledgeRepository.IsExistingKnowledge(_model.KnowledgeId)).Returns(true);

        var validator = new CreateFactionLevelModelValidator();

        _useCase = new CreateFactionLevelUseCase(
            _factionLevelRepository,
            _knowledgeRepository,
            validator,
            CancellationToken.None
        );
    }

    [Fact]
    public async Task ValidationFor_FactionId_WillFail_WhenItIsEmpty()
    {
        _model.FactionId = 0;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateFactionLevelModel.FactionId),
            "Faction Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_KnowledgeId_WillFail_WhenItIsEmpty()
    {
        _model.KnowledgeId = 0;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateFactionLevelModel.KnowledgeId),
            "Knowledge Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_KnowledgeId_WillFail_WhenKnowledgeDoesNotExist()
    {
        A.CallTo(() => _knowledgeRepository.IsExistingKnowledge(_model.KnowledgeId)).Returns(false);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveNotFoundError(
            nameof(CreateFactionLevelModel.KnowledgeId),
            "Knowledge not found"
        );
    }

    [Fact]
    public async Task ValidationFor_Specialization_WillFail_WhenSpecialization_IsEmpty()
    {
        _model.Specialization = string.Empty;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateFactionLevelModel.Specialization),
            "Specialization is required."
        );
    }

    [Fact]
    public async Task ValidationFor_Specialization_WillFail_WhenSpecialization_IsOver250Characters()
    {
        _model.Specialization = new string('x', 251);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateFactionLevelModel.Specialization),
            "Specialization cannot exceed 250 characters."
        );
    }

    [Fact]
    public async Task UseCase_WillCheckThatKnowledgeExists()
    {
        await _useCase.ExecuteAsync(_model);

        A.CallTo(() => _knowledgeRepository.IsExistingKnowledge(_model.KnowledgeId))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_WillCreateTheFactionLevels()
    {
        await _useCase.ExecuteAsync(_model);

        A.CallTo(() =>
                _factionLevelRepository.CreateFactionLevelsAsync(
                    A<List<FactionLevel>>.That.Matches(k =>
                        k.Count == 4
                        && k.Any(x =>
                            x.FactionId == _model.FactionId
                            && x.FactionRankId == FactionRankEnum.Basic.Value
                            && x.KnowledgeId == null
                            && x.KnowledgeLevelId == null
                            && x.Specialization == null
                        )
                        && k.Any(x =>
                            x.FactionId == _model.FactionId
                            && x.FactionRankId == FactionRankEnum.Intermediate.Value
                            && x.KnowledgeId == _model.KnowledgeId
                            && x.KnowledgeLevelId == 3
                            && x.Specialization == _model.Specialization
                        )
                        && k.Any(x =>
                            x.FactionId == _model.FactionId
                            && x.FactionRankId == FactionRankEnum.Advance.Value
                            && x.KnowledgeId == _model.KnowledgeId
                            && x.KnowledgeLevelId == 5
                            && x.Specialization == _model.Specialization
                        )
                        && k.Any(x =>
                            x.FactionId == _model.FactionId
                            && x.FactionRankId == FactionRankEnum.Supreme.Value
                            && x.KnowledgeId == _model.KnowledgeId
                            && x.KnowledgeLevelId == 7
                            && x.Specialization == _model.Specialization
                        )
                    )
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_WillReturn_Success_IfSuccessful()
    {
        var result = await _useCase.ExecuteAsync(_model);

        Assert.True(result.IsSuccess);
    }
}

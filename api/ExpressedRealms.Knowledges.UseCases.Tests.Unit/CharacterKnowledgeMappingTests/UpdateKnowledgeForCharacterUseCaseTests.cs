using ExpressedRealms.Characters.Repository.Xp;
using ExpressedRealms.Characters.Repository.Xp.Dtos;
using ExpressedRealms.DB.Models.Characters.XpTables;
using ExpressedRealms.DB.Models.Knowledges.CharacterKnowledgeMappings;
using ExpressedRealms.DB.Models.Knowledges.KnowledgeEducationLevels;
using ExpressedRealms.DB.Models.Knowledges.KnowledgeModels;
using ExpressedRealms.Knowledges.Repository;
using ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings;
using ExpressedRealms.Knowledges.Repository.Knowledges;
using ExpressedRealms.Knowledges.UseCases.CharacterKnowledgeMappings.Create;
using ExpressedRealms.Knowledges.UseCases.CharacterKnowledgeMappings.Edit;
using ExpressedRealms.Shared;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using ExpressedRealms.UseCases.Shared.CommonFailureTypes;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Knowledges.UseCases.Tests.Unit.CharacterKnowledgeMappingTests;

public class UpdateKnowledgeForCharacterUseCaseTests
{
    private readonly UpdateKnowledgeForCharacterUseCase _useCase;
    private readonly IKnowledgeRepository _knowledgeRepository;
    private readonly ICharacterKnowledgeRepository _mappingRepository;
    private readonly IKnowledgeLevelRepository _levelRepository;
    private readonly UpdateKnowledgeForCharacterModel _model;
    private readonly IXpRepository _xpRepository;
    private readonly CharacterKnowledgeMapping _dbModel;

    public UpdateKnowledgeForCharacterUseCaseTests()
    {
        _model = new UpdateKnowledgeForCharacterModel()
        {
            MappingId = 1,
            KnowledgeLevelId = 2,
            Notes = "123",
        };

        _dbModel = new CharacterKnowledgeMapping()
        {
            KnowledgeLevelId = 3,
            CharacterId = 2,
            KnowledgeId = 4,
            Notes = "123",
        };

        _knowledgeRepository = A.Fake<IKnowledgeRepository>();
        _mappingRepository = A.Fake<ICharacterKnowledgeRepository>();
        _levelRepository = A.Fake<IKnowledgeLevelRepository>();
        _xpRepository = A.Fake<IXpRepository>();

        A.CallTo(() => _mappingRepository.GetCharacterKnowledgeMappingForEditing(_model.MappingId))
            .Returns(_dbModel);

        A.CallTo(() => _levelRepository.KnowledgeLevelExists(_model.KnowledgeLevelId))
            .Returns(true);
        A.CallTo(() => _mappingRepository.MappingAlreadyExists(_model.MappingId)).Returns(true);
        A.CallTo(() =>
                _xpRepository.GetAvailableXpForSection(
                    _dbModel.CharacterId,
                    XpSectionTypes.Knowledge
                )
            )
            .Returns(
                new SectionXpDto()
                {
                    AvailableXp = StartingExperience.StartingKnowledges,
                    SpentXp = 0,
                }
            );
        A.CallTo(() => _knowledgeRepository.GetKnowledgeAsync(_dbModel.KnowledgeId))
            .Returns(new Knowledge() { KnowledgeTypeId = 3 });
        A.CallTo(() => _levelRepository.GetKnowledgeLevel(_model.KnowledgeLevelId))
            .Returns(
                new KnowledgeEducationLevel() { TotalGeneralXpCost = 4, TotalUnknownXpCost = 2 }
            );

        var validator = new UpdateKnowledgeForCharacterModelValidator(
            _knowledgeRepository,
            _mappingRepository,
            _levelRepository
        );

        _useCase = new UpdateKnowledgeForCharacterUseCase(
            _mappingRepository,
            _levelRepository,
            _knowledgeRepository,
            _xpRepository,
            validator,
            CancellationToken.None
        );
    }

    [Fact]
    public async Task ValidationFor_MappingId_WillFail_WhenItsEmpty()
    {
        _model.MappingId = 0;
        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(UpdateKnowledgeForCharacterModel.MappingId),
            "Mapping Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_MappingId_WillFail_WhenItDoesNotExist()
    {
        A.CallTo(() => _mappingRepository.MappingAlreadyExists(_model.MappingId)).Returns(false);
        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(UpdateKnowledgeForCharacterModel.MappingId),
            "The Knowledge Mapping does not exist."
        );
    }

    [Fact]
    public async Task ValidationFor_KnowledgeLevelId_WillFail_WhenItsEmpty()
    {
        _model.KnowledgeLevelId = 0;
        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(AddKnowledgeToCharacterModel.KnowledgeLevelId),
            "Knowledge Level Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_KnowledgeLevelId_WillFail_WhenItDoesNotExist()
    {
        A.CallTo(() => _levelRepository.KnowledgeLevelExists(_model.KnowledgeLevelId))
            .Returns(false);
        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(AddKnowledgeToCharacterModel.KnowledgeLevelId),
            "The Knowledge Level does not exist."
        );
    }

    [Fact]
    public async Task ValidationFor_Notes_WillFail_WhenMaxLengthIsGreaterThan5000()
    {
        _model.Notes = new string('x', 5001);
        var result = await _useCase.ExecuteAsync(_model);
        result.MustHaveValidationError(
            nameof(AddKnowledgeToCharacterModel.Notes),
            "Notes must be less than 5000 characters."
        );
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task ValidationFor_Notes_AreOptional(string? notes)
    {
        _model.Notes = notes;
        var result = await _useCase.ExecuteAsync(_model);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task UseCase_GetsKnowledgeLevel()
    {
        await _useCase.ExecuteAsync(_model);

        A.CallTo(() => _levelRepository.GetKnowledgeLevel(_model.KnowledgeLevelId))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_GetsTheKnowledgeMapping()
    {
        await _useCase.ExecuteAsync(_model);

        A.CallTo(() => _mappingRepository.GetCharacterKnowledgeMappingForEditing(_model.MappingId))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_GetsKnowledge()
    {
        await _useCase.ExecuteAsync(_model);

        A.CallTo(() => _knowledgeRepository.GetKnowledgeAsync(_dbModel.KnowledgeId))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_WillUpdateNotesField()
    {
        _model.Notes = "456";
        _model.KnowledgeLevelId = 2;
        _dbModel.KnowledgeLevelId = 2;

        var result = await _useCase.ExecuteAsync(_model);
        Assert.True(result.IsSuccess);

        A.CallTo(() =>
                _mappingRepository.UpdateCharacterKnowledgeMapping(
                    A<CharacterKnowledgeMapping>.That.Matches(x => x.Notes == _model.Notes)
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Theory]
    [InlineData(" test", "test")]
    [InlineData(" test ", "test")]
    [InlineData("test ", "test")]
    [InlineData(" ", null)]
    [InlineData(null, null)]
    public async Task UseCase_WillTrimNotesField(string? notes, string? savedValue)
    {
        _model.Notes = notes;
        _model.KnowledgeLevelId = 2;
        _dbModel.KnowledgeLevelId = 2;

        var result = await _useCase.ExecuteAsync(_model);
        Assert.True(result.IsSuccess);

        A.CallTo(() =>
                _mappingRepository.UpdateCharacterKnowledgeMapping(
                    A<CharacterKnowledgeMapping>.That.Matches(x => x.Notes == savedValue)
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Theory]
    [InlineData(2, 2)]
    [InlineData(3, 4)]
    public async Task UseCase_GrabsCorrectXp_DependingOnKnowledgeType(
        int knowledgeTypeId,
        int xpAmount
    )
    {
        A.CallTo(() => _knowledgeRepository.GetKnowledgeAsync(_dbModel.KnowledgeId))
            .Returns(new Knowledge() { KnowledgeTypeId = knowledgeTypeId });
        A.CallTo(() => _levelRepository.GetKnowledgeLevel(_model.KnowledgeLevelId))
            .Returns(
                new KnowledgeEducationLevel() { TotalGeneralXpCost = 2, TotalUnknownXpCost = 4 }
            );

        A.CallTo(() =>
                _xpRepository.GetAvailableXpForSection(
                    _dbModel.CharacterId,
                    XpSectionTypes.Knowledge
                )
            )
            .Returns(
                new SectionXpDto()
                {
                    AvailableXp = StartingExperience.StartingKnowledges,
                    SpentXp = StartingExperience.StartingKnowledges,
                }
            );

        var result = await _useCase.ExecuteAsync(_model);

        Assert.Equal(xpAmount, ((NotEnoughXPFailure)result.Errors[0]).AmountTryingToSpend);
    }

    [Theory]
    [InlineData(4)]
    [InlineData(2)]
    public async Task UseCase_CalculatesAvailableXP_Correctly(int xpAmount)
    {
        A.CallTo(() => _knowledgeRepository.GetKnowledgeAsync(_dbModel.KnowledgeId))
            .Returns(new Knowledge() { KnowledgeTypeId = 2 });
        A.CallTo(() => _levelRepository.GetKnowledgeLevel(_model.KnowledgeLevelId))
            .Returns(
                new KnowledgeEducationLevel()
                {
                    TotalGeneralXpCost = StartingExperience.StartingKnowledges,
                }
            );
        A.CallTo(() =>
                _xpRepository.GetAvailableXpForSection(
                    _dbModel.CharacterId,
                    XpSectionTypes.Knowledge
                )
            )
            .Returns(
                new SectionXpDto()
                {
                    AvailableXp = StartingExperience.StartingKnowledges,
                    SpentXp = xpAmount,
                }
            );

        var result = await _useCase.ExecuteAsync(_model);

        Assert.Equal(
            StartingExperience.StartingKnowledges - xpAmount,
            ((NotEnoughXPFailure)result.Errors[0]).AvailableXP
        );
    }

    [Fact]
    public async Task UseCase_WillReturnNotEnoughXp_WhenOutOfXp()
    {
        A.CallTo(() =>
                _xpRepository.GetAvailableXpForSection(
                    _dbModel.CharacterId,
                    XpSectionTypes.Knowledge
                )
            )
            .Returns(
                new SectionXpDto()
                {
                    AvailableXp = StartingExperience.StartingKnowledges,
                    SpentXp = StartingExperience.StartingKnowledges,
                }
            );

        var result = await _useCase.ExecuteAsync(_model);
        Assert.True(result.HasError<NotEnoughXPFailure>());
        Assert.Equal(0, ((NotEnoughXPFailure)result.Errors[0]).AvailableXP);
        Assert.Equal(2, ((NotEnoughXPFailure)result.Errors[0]).AmountTryingToSpend);
    }

    [Fact]
    public async Task UseCase_WillUpdateKnowledgeLevelId()
    {
        _model.KnowledgeLevelId = 2;
        _dbModel.KnowledgeLevelId = 3;

        var result = await _useCase.ExecuteAsync(_model);
        Assert.True(result.IsSuccess);

        A.CallTo(() =>
                _mappingRepository.UpdateCharacterKnowledgeMapping(
                    A<CharacterKnowledgeMapping>.That.Matches(x =>
                        x.KnowledgeLevelId == _model.KnowledgeLevelId
                    )
                )
            )
            .MustHaveHappenedOnceExactly();
    }
}

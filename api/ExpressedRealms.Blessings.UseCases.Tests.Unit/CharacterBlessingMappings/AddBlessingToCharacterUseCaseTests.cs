using ExpressedRealms.Blessings.Repository.Blessings;
using ExpressedRealms.Blessings.Repository.CharacterBlessings;
using ExpressedRealms.Blessings.UseCases.CharacterBlessingMappings.Create;
using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.Repository.DTOs;
using ExpressedRealms.Characters.Repository.Xp;
using ExpressedRealms.DB.Characters.xpTables;
using ExpressedRealms.DB.Models.Blessings.BlessingLevelSetup;
using ExpressedRealms.DB.Models.Blessings.BlessingSetup;
using ExpressedRealms.DB.Models.Blessings.CharacterBlessingMappings;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using ExpressedRealms.UseCases.Shared.CommonFailureTypes;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Blessings.UseCases.Tests.Unit.CharacterBlessingMappings;

public class AddBlessingToCharacterUseCaseTests
{
    private readonly AddBlessingToCharacterUseCase _useCase;
    private readonly IBlessingRepository _blessingRepository;
    private readonly ICharacterBlessingRepository _mappingRepository;
    private readonly ICharacterRepository _characterRepository;
    private readonly IXpRepository _xpRepository;
    private readonly AddBlessingToCharacterModel _model;

    private readonly Blessing _blessingDbModel;
    private readonly CharacterXpView _characterMappingDbModel;
    private readonly BlessingLevel _blessingLevelDbModel;

    public AddBlessingToCharacterUseCaseTests()
    {
        _model = new AddBlessingToCharacterModel()
        {
            BlessingLevelId = 1,
            CharacterId = 2,
            BlessingId = 3,
            Notes = "123",
        };

        _blessingDbModel = new Blessing()
        {
            Name = "test",
            Description = "test",
            SubCategory = "Test",
            Type = "Disadvantage",
        };

        _blessingLevelDbModel = new BlessingLevel() { XpCost = 4 };

        _characterMappingDbModel = new CharacterXpView() { SectionCap = 8, SpentXp = 0 };

        _characterRepository = A.Fake<ICharacterRepository>();
        _blessingRepository = A.Fake<IBlessingRepository>();
        _mappingRepository = A.Fake<ICharacterBlessingRepository>();
        _xpRepository = A.Fake<IXpRepository>();

        A.CallTo(() => _blessingRepository.IsExistingBlessing(_model.BlessingId)).Returns(true);
        A.CallTo(() => _characterRepository.CharacterExistsAsync(_model.CharacterId)).Returns(true);
        A.CallTo(() => _blessingRepository.BlessingLevelExists(_model.BlessingLevelId))
            .Returns(true);
        A.CallTo(() =>
                _mappingRepository.MappingAlreadyExists(_model.BlessingId, _model.CharacterId)
            )
            .Returns(false);

        A.CallTo(() =>
                _xpRepository.GetCharacterXpMapping(
                    _model.CharacterId,
                    (int)XpSectionTypes.Advantages
                )
            )
            .Returns(_characterMappingDbModel);

        A.CallTo(() =>
                _xpRepository.GetCharacterXpMapping(
                    _model.CharacterId,
                    (int)XpSectionTypes.Disadvantages
                )
            )
            .Returns(_characterMappingDbModel);

        A.CallTo(() => _blessingRepository.GetBlessingLevel(_model.BlessingLevelId))
            .Returns(_blessingLevelDbModel);

        A.CallTo(() => _characterRepository.GetCharacterState(_model.CharacterId))
            .Returns(new CharacterStatusDto() { IsInCharacterCreation = true });

        A.CallTo(() => _blessingRepository.GetBlessingForEditing(_model.BlessingId))
            .Returns(_blessingDbModel);

        A.CallTo(() => _xpRepository.GetAvailableDiscretionary(_model.CharacterId)).Returns(16);

        var validator = new AddBlessingToCharacterModelValidator(
            _blessingRepository,
            _characterRepository,
            _mappingRepository
        );

        _useCase = new AddBlessingToCharacterUseCase(
            _mappingRepository,
            _blessingRepository,
            _characterRepository,
            _xpRepository,
            validator,
            CancellationToken.None
        );
    }

    [Fact]
    public async Task ValidationFor_BlessinglId_WillFail_WhenItsEmpty()
    {
        _model.BlessingId = 0;
        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(AddBlessingToCharacterModel.BlessingId),
            "Blessing Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_BlessingId_WillFail_WhenItDoesNotExist()
    {
        A.CallTo(() => _blessingRepository.IsExistingBlessing(_model.BlessingId)).Returns(false);
        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(AddBlessingToCharacterModel.BlessingId),
            "The Blessing does not exist."
        );
    }

    [Fact]
    public async Task ValidationFor_CharacterId_WillFail_WhenItsEmpty()
    {
        _model.CharacterId = 0;
        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(AddBlessingToCharacterModel.CharacterId),
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
            nameof(AddBlessingToCharacterModel.CharacterId),
            "The Character does not exist."
        );
    }

    [Fact]
    public async Task ValidationFor_BlessingLevelId_WillFail_WhenItsEmpty()
    {
        _model.BlessingLevelId = 0;
        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(AddBlessingToCharacterModel.BlessingLevelId),
            "Blessing Level Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_BlessingLevelId_WillFail_WhenItDoesNotExist()
    {
        A.CallTo(() => _blessingRepository.BlessingLevelExists(_model.BlessingLevelId))
            .Returns(false);
        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(AddBlessingToCharacterModel.BlessingLevelId),
            "The Blessing Level does not exist."
        );
    }

    [Fact]
    public async Task ValidationFor_DuplicateMapping_WillFail_WhenItDoesExist()
    {
        A.CallTo(() =>
                _mappingRepository.MappingAlreadyExists(_model.BlessingId, _model.CharacterId)
            )
            .Returns(true);
        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(AddBlessingToCharacterModel.BlessingId),
            "The Blessing already exists for this character."
        );
    }

    [Fact]
    public async Task ValidationFor_Notes_WillFail_WhenMaxLengthIsGreaterThan5000()
    {
        _model.Notes = new string('x', 5001);
        var result = await _useCase.ExecuteAsync(_model);
        result.MustHaveValidationError(
            nameof(AddBlessingToCharacterModel.Notes),
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
    public async Task UseCase_GetsBlessingLevel()
    {
        await _useCase.ExecuteAsync(_model);

        A.CallTo(() => _blessingRepository.GetBlessingLevel(_model.BlessingLevelId))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_ReturnsError_WhenModifyingOutsideOfCharacterCreation()
    {
        A.CallTo(() => _characterRepository.GetCharacterState(_model.CharacterId))
            .Returns(new CharacterStatusDto() { IsInCharacterCreation = false });

        var result = await _useCase.ExecuteAsync(_model);

        Assert.False(result.IsSuccess);
        Assert.Equal(
            "You cannot add Advantages or Disadvantages outside of character creation.",
            result.Errors.First().Message
        );
    }

    [Theory]
    [InlineData("Advantage", "Advantages")]
    [InlineData("Disadvantage", "Disadvantages")]
    public async Task UseCase_ReturnsError_WhenAddingMoreThan8PointsOfAdvantagesOrDisadvantages(
        string blessingType,
        string messageString
    )
    {
        _blessingDbModel.Type = blessingType;
        _characterMappingDbModel.SpentXp = 20;

        var result = await _useCase.ExecuteAsync(_model);

        Assert.False(result.IsSuccess);
        Assert.Equal(
            $"You cannot add more than 8 points of {messageString}.",
            result.Errors.First().Message
        );
    }

    [Fact]
    public async Task UseCase_WillReturnNotEnoughXp_WhenOutOfXp()
    {
        _blessingDbModel.Type = "advantage";
        _characterMappingDbModel.SpentXp = 4;
        _blessingLevelDbModel.XpCost = 2;
        A.CallTo(() => _xpRepository.GetAvailableDiscretionary(_model.CharacterId)).Returns(4);

        var result = await _useCase.ExecuteAsync(_model);
        Assert.True(result.HasError<NotEnoughXPFailure>());
        Assert.Equal(4, ((NotEnoughXPFailure)result.Errors[0]).AvailableXP);
        Assert.Equal(2, ((NotEnoughXPFailure)result.Errors[0]).AmountTryingToSpend);
    }

    [Fact]
    public async Task UseCase_AddCharacterBlessingMapping_WhenItHasEnoughXp()
    {
        await _useCase.ExecuteAsync(_model);

        A.CallTo(() =>
                _mappingRepository.AddCharacterBlessingMapping(
                    A<CharacterBlessingMapping>.That.Matches(x =>
                        x.Notes == _model.Notes
                        && x.BlessingId == _model.BlessingId
                        && x.CharacterId == _model.CharacterId
                        && x.BlessingLevelId == _model.BlessingLevelId
                    )
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

        var result = await _useCase.ExecuteAsync(_model);
        Assert.True(result.IsSuccess);

        A.CallTo(() =>
                _mappingRepository.AddCharacterBlessingMapping(
                    A<CharacterBlessingMapping>.That.Matches(x => x.Notes == savedValue)
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_WillReturn_BlessingId_IfSuccessful()
    {
        A.CallTo(() =>
                _mappingRepository.AddCharacterBlessingMapping(A<CharacterBlessingMapping>._)
            )
            .Returns(5);
        var result = await _useCase.ExecuteAsync(_model);
        Assert.Equal(5, result.Value);
    }
}

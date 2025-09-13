using ExpressedRealms.Blessings.Repository.Blessings;
using ExpressedRealms.Blessings.Repository.CharacterBlessings;
using ExpressedRealms.Blessings.UseCases.CharacterBlessingMappings.Create;
using ExpressedRealms.Characters.Repository;
using ExpressedRealms.DB.Models.Blessings.BlessingLevelSetup;
using ExpressedRealms.DB.Models.Blessings.CharacterBlessingMappings;
using ExpressedRealms.Shared;
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
    private readonly AddBlessingToCharacterModel _blessingToCharacterModel;

    public AddBlessingToCharacterUseCaseTests()
    {
        _blessingToCharacterModel = new AddBlessingToCharacterModel()
        {
            BlessingLevelId = 1,
            CharacterId = 2,
            BlessingId = 3,
            Notes = "123",
        };

        _characterRepository = A.Fake<ICharacterRepository>();
        _blessingRepository = A.Fake<IBlessingRepository>();
        _mappingRepository = A.Fake<ICharacterBlessingRepository>();

        A.CallTo(() => _blessingRepository.IsExistingBlessing(_blessingToCharacterModel.BlessingId))
            .Returns(true);
        A.CallTo(() =>
                _characterRepository.CharacterExistsAsync(_blessingToCharacterModel.CharacterId)
            )
            .Returns(true);
        A.CallTo(() =>
                _blessingRepository.BlessingLevelExists(_blessingToCharacterModel.BlessingLevelId)
            )
            .Returns(true);
        A.CallTo(() =>
                _mappingRepository.MappingAlreadyExists(
                    _blessingToCharacterModel.BlessingId,
                    _blessingToCharacterModel.CharacterId
                )
            )
            .Returns(false);
        A.CallTo(() =>
                _mappingRepository.GetSpentXpForBlessingType(
                    _blessingToCharacterModel.CharacterId,
                    _blessingToCharacterModel.BlessingId
                )
            )
            .Returns(0);
        A.CallTo(() =>
                _blessingRepository.GetBlessingLevel(_blessingToCharacterModel.BlessingLevelId)
            )
            .Returns(new BlessingLevel() { XpCost = 4 });

        var validator = new AddBlessingToCharacterModelValidator(
            _blessingRepository,
            _characterRepository,
            _mappingRepository
        );

        _useCase = new AddBlessingToCharacterUseCase(
            _mappingRepository,
            _blessingRepository,
            validator,
            CancellationToken.None
        );
    }

    [Fact]
    public async Task ValidationFor_BlessinglId_WillFail_WhenItsEmpty()
    {
        _blessingToCharacterModel.BlessingId = 0;
        var result = await _useCase.ExecuteAsync(_blessingToCharacterModel);

        result.MustHaveValidationError(
            nameof(AddBlessingToCharacterModel.BlessingId),
            "Blessing Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_BlessingId_WillFail_WhenItDoesNotExist()
    {
        A.CallTo(() => _blessingRepository.IsExistingBlessing(_blessingToCharacterModel.BlessingId))
            .Returns(false);
        var result = await _useCase.ExecuteAsync(_blessingToCharacterModel);

        result.MustHaveValidationError(
            nameof(AddBlessingToCharacterModel.BlessingId),
            "The Blessing does not exist."
        );
    }

    [Fact]
    public async Task ValidationFor_CharacterId_WillFail_WhenItsEmpty()
    {
        _blessingToCharacterModel.CharacterId = 0;
        var result = await _useCase.ExecuteAsync(_blessingToCharacterModel);

        result.MustHaveValidationError(
            nameof(AddBlessingToCharacterModel.CharacterId),
            "Character Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_CharacterId_WillFail_WhenItDoesNotExist()
    {
        A.CallTo(() =>
                _characterRepository.CharacterExistsAsync(_blessingToCharacterModel.CharacterId)
            )
            .Returns(false);
        var result = await _useCase.ExecuteAsync(_blessingToCharacterModel);

        result.MustHaveValidationError(
            nameof(AddBlessingToCharacterModel.CharacterId),
            "The Character does not exist."
        );
    }

    [Fact]
    public async Task ValidationFor_BlessingLevelId_WillFail_WhenItsEmpty()
    {
        _blessingToCharacterModel.BlessingLevelId = 0;
        var result = await _useCase.ExecuteAsync(_blessingToCharacterModel);

        result.MustHaveValidationError(
            nameof(AddBlessingToCharacterModel.BlessingLevelId),
            "Blessing Level Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_BlessingLevelId_WillFail_WhenItDoesNotExist()
    {
        A.CallTo(() =>
                _blessingRepository.BlessingLevelExists(_blessingToCharacterModel.BlessingLevelId)
            )
            .Returns(false);
        var result = await _useCase.ExecuteAsync(_blessingToCharacterModel);

        result.MustHaveValidationError(
            nameof(AddBlessingToCharacterModel.BlessingLevelId),
            "The Blessing Level does not exist."
        );
    }

    [Fact]
    public async Task ValidationFor_DuplicateMapping_WillFail_WhenItDoesExist()
    {
        A.CallTo(() =>
                _mappingRepository.MappingAlreadyExists(
                    _blessingToCharacterModel.BlessingId,
                    _blessingToCharacterModel.CharacterId
                )
            )
            .Returns(true);
        var result = await _useCase.ExecuteAsync(_blessingToCharacterModel);

        result.MustHaveValidationError(
            nameof(AddBlessingToCharacterModel.BlessingId),
            "The Blessing already exists for this character."
        );
    }

    [Fact]
    public async Task ValidationFor_Notes_WillFail_WhenMaxLengthIsGreaterThan5000()
    {
        _blessingToCharacterModel.Notes = new string('x', 5001);
        var result = await _useCase.ExecuteAsync(_blessingToCharacterModel);
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
        _blessingToCharacterModel.Notes = notes;
        var result = await _useCase.ExecuteAsync(_blessingToCharacterModel);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task UseCase_GetsExperienceSpentOnBlessingsForCharacter()
    {
        await _useCase.ExecuteAsync(_blessingToCharacterModel);

        A.CallTo(() =>
                _mappingRepository.GetSpentXpForBlessingType(
                    _blessingToCharacterModel.CharacterId,
                    _blessingToCharacterModel.BlessingId
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_GetsBlessingLevel()
    {
        await _useCase.ExecuteAsync(_blessingToCharacterModel);

        A.CallTo(() =>
                _blessingRepository.GetBlessingLevel(_blessingToCharacterModel.BlessingLevelId)
            )
            .MustHaveHappenedOnceExactly();
    }

    [Theory]
    [InlineData(4)]
    [InlineData(2)]
    public async Task UseCase_CalculatesAvailableXP_Correctly(int xpAmount)
    {
        A.CallTo(() =>
                _blessingRepository.GetBlessingLevel(_blessingToCharacterModel.BlessingLevelId)
            )
            .Returns(new BlessingLevel() { XpCost = StartingExperience.StartingBlessings });
        A.CallTo(() =>
                _mappingRepository.GetSpentXpForBlessingType(
                    _blessingToCharacterModel.CharacterId,
                    _blessingToCharacterModel.BlessingId
                )
            )
            .Returns(xpAmount);

        var result = await _useCase.ExecuteAsync(_blessingToCharacterModel);

        Assert.Equal(
            StartingExperience.StartingBlessings - xpAmount,
            ((NotEnoughXPFailure)result.Errors[0]).AvailableXP
        );
    }

    [Fact]
    public async Task UseCase_WillReturnNotEnoughXp_WhenOutOfXp()
    {
        A.CallTo(() =>
                _mappingRepository.GetSpentXpForBlessingType(
                    _blessingToCharacterModel.CharacterId,
                    _blessingToCharacterModel.BlessingId
                )
            )
            .Returns(StartingExperience.StartingBlessings);
        ;

        var result = await _useCase.ExecuteAsync(_blessingToCharacterModel);
        Assert.True(result.HasError<NotEnoughXPFailure>());
        Assert.Equal(0, ((NotEnoughXPFailure)result.Errors[0]).AvailableXP);
        Assert.Equal(4, ((NotEnoughXPFailure)result.Errors[0]).AmountTryingToSpend);
    }

    [Fact]
    public async Task UseCase_AddCharacterBlessingMapping_WhenItHasEnoughXp()
    {
        await _useCase.ExecuteAsync(_blessingToCharacterModel);

        A.CallTo(() =>
                _mappingRepository.AddCharacterBlessingMapping(
                    A<CharacterBlessingMapping>.That.Matches(x =>
                        x.Notes == _blessingToCharacterModel.Notes
                        && x.BlessingId == _blessingToCharacterModel.BlessingId
                        && x.CharacterId == _blessingToCharacterModel.CharacterId
                        && x.BlessingLevelId == _blessingToCharacterModel.BlessingLevelId
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
        _blessingToCharacterModel.Notes = notes;

        var result = await _useCase.ExecuteAsync(_blessingToCharacterModel);
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
        var result = await _useCase.ExecuteAsync(_blessingToCharacterModel);
        Assert.Equal(5, result.Value);
    }
}

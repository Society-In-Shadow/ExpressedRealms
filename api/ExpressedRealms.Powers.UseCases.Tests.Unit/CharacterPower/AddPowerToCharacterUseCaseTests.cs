using ExpressedRealms.Characters.Repository;
using ExpressedRealms.DB.Models.Powers.CharacterPowerMappingSetup;
using ExpressedRealms.Powers.Repository.CharacterPower;
using ExpressedRealms.Powers.Repository.Powers;
using ExpressedRealms.Powers.UseCases.CharacterPower.Create;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using ExpressedRealms.UseCases.Shared.CommonFailureTypes;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Powers.UseCases.Tests.Unit.CharacterPower;

public class AddPowerToCharacterUseCaseTests
{
    private readonly AddPowerToCharacterUseCase _useCase;
    private readonly IPowerRepository _powerRepository;
    private readonly ICharacterPowerRepository _mappingRepository;
    private readonly ICharacterRepository _characterRepository;
    private readonly AddPowerToCharacterModel _powerToCharacterModel;

    public AddPowerToCharacterUseCaseTests()
    {
        _powerToCharacterModel = new AddPowerToCharacterModel()
        {
            PowerLevelId = 1,
            CharacterId = 2,
            PowerId = 3,
            Notes = "123",
        };

        _characterRepository = A.Fake<ICharacterRepository>();
        _powerRepository = A.Fake<IPowerRepository>();
        _mappingRepository = A.Fake<ICharacterPowerRepository>();

        A.CallTo(() =>
                _powerRepository.IsValidPower(_powerToCharacterModel.PowerId)
            )
            .Returns(true);
        A.CallTo(() =>
                _characterRepository.CharacterExistsAsync(_powerToCharacterModel.CharacterId)
            )
            .Returns(true);
        A.CallTo(() =>
                _powerRepository.IsValidPowerLevel(_powerToCharacterModel.PowerLevelId)
            )
            .Returns(true);
        A.CallTo(() =>
                _mappingRepository.MappingExistsAsync(
                    _powerToCharacterModel.PowerId,
                    _powerToCharacterModel.CharacterId
                )
            )
            .Returns(false);
        A.CallTo(() =>
                _mappingRepository.GetExperienceSpentOnPowersForCharacter(
                    _powerToCharacterModel.CharacterId
                )
            )
            .Returns(0);
        A.CallTo(() =>
            _powerRepository.GetPowerLevelExperience(_powerToCharacterModel.PowerLevelId)
        ).Returns(4);
        A.CallTo(() =>
                _mappingRepository.GetSelectablePowersForCharacter(_powerToCharacterModel.CharacterId)
            )
            .Returns(new List<int>(){ _powerToCharacterModel.PowerId });

        var validator = new AddPowerToCharacterModelValidator(
            _powerRepository,
            _characterRepository,
            _mappingRepository
        );

        _useCase = new AddPowerToCharacterUseCase(
            _mappingRepository,
            _powerRepository,
            validator,
            CancellationToken.None
        );
    }

    [Fact]
    public async Task ValidationFor_PowerId_WillFail_WhenItsEmpty()
    {
        _powerToCharacterModel.PowerId = 0;
        var result = await _useCase.ExecuteAsync(_powerToCharacterModel);

        result.MustHaveValidationError(
            nameof(AddPowerToCharacterModel.PowerId),
            "Power Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_PowerId_WillFail_WhenItDoesNotExist()
    {
        A.CallTo(() =>
                _powerRepository.IsValidPower(_powerToCharacterModel.PowerId)
            )
            .Returns(false);
        var result = await _useCase.ExecuteAsync(_powerToCharacterModel);

        result.MustHaveValidationError(
            nameof(AddPowerToCharacterModel.PowerId),
            "The Power does not exist."
        );
    }
    
    [Fact]
    public async Task ValidationFor_PowerId_WillFail_WhenCharacterDoesNotHaveThePrerequisites()
    {
        _powerToCharacterModel.PowerId = 2;
        
        var result = await _useCase.ExecuteAsync(_powerToCharacterModel);

        result.MustHaveValidationError(
            nameof(AddPowerToCharacterModel.PowerId),
            "The character does not have the powers required to use this power."
        );
    }

    [Fact]
    public async Task ValidationFor_CharacterId_WillFail_WhenItsEmpty()
    {
        _powerToCharacterModel.CharacterId = 0;
        var result = await _useCase.ExecuteAsync(_powerToCharacterModel);

        result.MustHaveValidationError(
            nameof(AddPowerToCharacterModel.CharacterId),
            "Character Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_CharacterId_WillFail_WhenItDoesNotExist()
    {
        A.CallTo(() =>
                _characterRepository.CharacterExistsAsync(_powerToCharacterModel.CharacterId)
            )
            .Returns(false);
        var result = await _useCase.ExecuteAsync(_powerToCharacterModel);

        result.MustHaveValidationError(
            nameof(AddPowerToCharacterModel.CharacterId),
            "The Character does not exist."
        );
    }

    [Fact]
    public async Task ValidationFor_PowerLevelId_WillFail_WhenItsEmpty()
    {
        _powerToCharacterModel.PowerLevelId = 0;
        var result = await _useCase.ExecuteAsync(_powerToCharacterModel);

        result.MustHaveValidationError(
            nameof(AddPowerToCharacterModel.PowerLevelId),
            "Power Level Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_PowerLevelId_WillFail_WhenItDoesNotExist()
    {
        A.CallTo(() =>
                _powerRepository.IsValidPowerLevel(_powerToCharacterModel.PowerLevelId)
            )
            .Returns(false);
        var result = await _useCase.ExecuteAsync(_powerToCharacterModel);

        result.MustHaveValidationError(
            nameof(AddPowerToCharacterModel.PowerLevelId),
            "The Power Level does not exist."
        );
    }

    [Fact]
    public async Task ValidationFor_DuplicateMapping_WillFail_WhenItDoesExist()
    {
        A.CallTo(() =>
                _mappingRepository.MappingExistsAsync(
                    _powerToCharacterModel.PowerId,
                    _powerToCharacterModel.CharacterId
                )
            )
            .Returns(true);
        var result = await _useCase.ExecuteAsync(_powerToCharacterModel);

        result.MustHaveValidationError(
            nameof(AddPowerToCharacterModel.PowerId),
            "The power already exists for this character."
        );
    }

    [Fact]
    public async Task ValidationFor_Notes_WillFail_WhenMaxLengthIsGreaterThan5000()
    {
        _powerToCharacterModel.Notes = new string('x', 5001);
        var result = await _useCase.ExecuteAsync(_powerToCharacterModel);
        result.MustHaveValidationError(
            nameof(AddPowerToCharacterModel.Notes),
            "Notes must be less than 5000 characters."
        );
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task ValidationFor_Notes_AreOptional(string? notes)
    {
        _powerToCharacterModel.Notes = notes;
        var result = await _useCase.ExecuteAsync(_powerToCharacterModel);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task UseCase_GetsExperienceSpentOnPowersForCharacter()
    {
        await _useCase.ExecuteAsync(_powerToCharacterModel);

        A.CallTo(() =>
                _mappingRepository.GetExperienceSpentOnPowersForCharacter(
                    _powerToCharacterModel.CharacterId
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_GetsPowerLevelExperience()
    {
        await _useCase.ExecuteAsync(_powerToCharacterModel);

        A.CallTo(() =>
                _powerRepository.GetPowerLevelExperience(_powerToCharacterModel.PowerLevelId)
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_GrabsCorrectXp()
    {
        A.CallTo(() =>
                _powerRepository.GetPowerLevelExperience(_powerToCharacterModel.PowerLevelId)
            )
            .Returns(4);

        A.CallTo(() =>
                _mappingRepository.GetExperienceSpentOnPowersForCharacter(
                    _powerToCharacterModel.CharacterId
                )
            )
            .Returns(100);

        var result = await _useCase.ExecuteAsync(_powerToCharacterModel);

        Assert.Equal(4, ((NotEnoughXPFailure)result.Errors[0]).AmountTryingToSpend);
    }

    [Theory]
    [InlineData(4)]
    [InlineData(2)]
    public async Task UseCase_CalculatesAvailableXP_Correctly(int xpAmount)
    {
        A.CallTo(() =>
                _powerRepository.GetPowerLevelExperience(_powerToCharacterModel.PowerLevelId)
            )
            .Returns(20);
        A.CallTo(() =>
                _mappingRepository.GetExperienceSpentOnPowersForCharacter(
                    _powerToCharacterModel.CharacterId
                )
            )
            .Returns(xpAmount);

        var result = await _useCase.ExecuteAsync(_powerToCharacterModel);

        Assert.Equal(20 - xpAmount, ((NotEnoughXPFailure)result.Errors[0]).AvailableXP);
    }

    [Fact]
    public async Task UseCase_WillReturnNotEnoughXp_WhenOutOfXp()
    {
        A.CallTo(() =>
                _mappingRepository.GetExperienceSpentOnPowersForCharacter(
                    _powerToCharacterModel.CharacterId
                )
            )
            .Returns(20);

        var result = await _useCase.ExecuteAsync(_powerToCharacterModel);
        Assert.True(result.HasError<NotEnoughXPFailure>());
        Assert.Equal(0, ((NotEnoughXPFailure)result.Errors[0]).AvailableXP);
        // Should be the amount of xp needed to level up to the next level.
        Assert.Equal(4, ((NotEnoughXPFailure)result.Errors[0]).AmountTryingToSpend);
    }

    [Fact]
    public async Task UseCase_AddCharacterPowerMapping_WhenItHasEnoughXp()
    {
        await _useCase.ExecuteAsync(_powerToCharacterModel);

        A.CallTo(() =>
                _mappingRepository.AddCharacterPowerMapping(
                    A<CharacterPowerMapping>.That.Matches(x =>
                        x.Notes == _powerToCharacterModel.Notes
                        && x.PowerId == _powerToCharacterModel.PowerId
                        && x.CharacterId == _powerToCharacterModel.CharacterId
                        && x.PowerLevelId == _powerToCharacterModel.PowerLevelId
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
        _powerToCharacterModel.Notes = notes;

        var result = await _useCase.ExecuteAsync(_powerToCharacterModel);
        Assert.True(result.IsSuccess);

        A.CallTo(() =>
                _mappingRepository.AddCharacterPowerMapping(
                    A<CharacterPowerMapping>.That.Matches(x => x.Notes == savedValue)
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_WillReturn_PowerId_IfSuccessful()
    {
        A.CallTo(() =>
                _mappingRepository.AddCharacterPowerMapping(A<CharacterPowerMapping>._)
            )
            .Returns(5);
        var result = await _useCase.ExecuteAsync(_powerToCharacterModel);
        Assert.Equal(5, result.Value);
    }
}

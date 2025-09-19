using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.Repository.Xp;
using ExpressedRealms.Characters.Repository.Xp.Dtos;
using ExpressedRealms.DB.Characters.xpTables;
using ExpressedRealms.DB.Models.Powers;
using ExpressedRealms.DB.Models.Powers.CharacterPowerMappingSetup;
using ExpressedRealms.Powers.Repository.CharacterPower;
using ExpressedRealms.Powers.Repository.Powers;
using ExpressedRealms.Powers.UseCases.CharacterPower.Create;
using ExpressedRealms.Shared;
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
    private readonly IXpRepository _xpRepository = A.Fake<IXpRepository>();
    private readonly AddPowerToCharacterModel _dbModel;
    private readonly PowerLevel _powerLevel;

    public AddPowerToCharacterUseCaseTests()
    {
        _dbModel = new AddPowerToCharacterModel()
        {
            CharacterId = 2,
            PowerId = 3,
            Notes = "123",
        };

        _powerLevel = new PowerLevel() { Id = 1, Xp = 4 };

        _characterRepository = A.Fake<ICharacterRepository>();
        _powerRepository = A.Fake<IPowerRepository>();
        _mappingRepository = A.Fake<ICharacterPowerRepository>();
        _xpRepository = A.Fake<IXpRepository>();

        A.CallTo(() => _powerRepository.IsValidPower(_dbModel.PowerId)).Returns(true);
        A.CallTo(() =>
                _characterRepository.CharacterExistsAsync(_dbModel.CharacterId)
            )
            .Returns(true);
        A.CallTo(() =>
                _mappingRepository.MappingExistsAsync(
                    _dbModel.PowerId,
                    _dbModel.CharacterId
                )
            )
            .Returns(false);
        A.CallTo(() =>
                _xpRepository.GetAvailableXpForSection(_dbModel.CharacterId, XpSectionTypeEnum.Powers)
            )
            .Returns(new SectionXpDto()
            {
                AvailableXp = StartingExperience.StartingPowers,
                SpentXp = 0
            });
        A.CallTo(() => _powerRepository.GetPowerLevelForPower(_dbModel.PowerId))
            .Returns(_powerLevel);
        A.CallTo(() =>
                _mappingRepository.GetSelectablePowersForCharacter(
                    _dbModel.CharacterId
                )
            )
            .Returns(new List<int>() { _dbModel.PowerId });

        A.CallTo(() =>
                _powerRepository.IsValidPowerForCharacter(
                    _dbModel.CharacterId,
                    _dbModel.PowerId
                )
            )
            .Returns(true);

        var validator = new AddPowerToCharacterModelValidator(
            _powerRepository,
            _characterRepository,
            _mappingRepository
        );

        _useCase = new AddPowerToCharacterUseCase(
            _mappingRepository,
            _powerRepository,
            validator,
            _xpRepository,
            CancellationToken.None
        );
    }

    [Fact]
    public async Task ValidationFor_PowerId_WillFail_WhenItsEmpty()
    {
        _dbModel.PowerId = 0;
        var result = await _useCase.ExecuteAsync(_dbModel);

        result.MustHaveValidationError(
            nameof(AddPowerToCharacterModel.PowerId),
            "Power Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_PowerId_WillFail_WhenItDoesNotExist()
    {
        A.CallTo(() => _powerRepository.IsValidPower(_dbModel.PowerId))
            .Returns(false);
        var result = await _useCase.ExecuteAsync(_dbModel);

        result.MustHaveValidationError(
            nameof(AddPowerToCharacterModel.PowerId),
            "The Power does not exist."
        );
    }

    [Fact]
    public async Task ValidationFor_PowerId_WillFail_WhenCharacterDoesNotHaveThePrerequisites()
    {
        _dbModel.PowerId = 2;

        var result = await _useCase.ExecuteAsync(_dbModel);

        result.MustHaveValidationError(
            nameof(AddPowerToCharacterModel.PowerId),
            "The character does not have the powers required to use this power."
        );
    }

    [Fact]
    public async Task ValidationFor_CharacterId_WillFail_WhenItsEmpty()
    {
        _dbModel.CharacterId = 0;
        var result = await _useCase.ExecuteAsync(_dbModel);

        result.MustHaveValidationError(
            nameof(AddPowerToCharacterModel.CharacterId),
            "Character Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_CharacterId_WillFail_WhenItDoesNotExist()
    {
        A.CallTo(() =>
                _characterRepository.CharacterExistsAsync(_dbModel.CharacterId)
            )
            .Returns(false);
        var result = await _useCase.ExecuteAsync(_dbModel);

        result.MustHaveValidationError(
            nameof(AddPowerToCharacterModel.CharacterId),
            "The Character does not exist."
        );
    }

    [Fact]
    public async Task ValidationFor_DuplicateMapping_WillFail_WhenItDoesExist()
    {
        A.CallTo(() =>
                _mappingRepository.MappingExistsAsync(
                    _dbModel.PowerId,
                    _dbModel.CharacterId
                )
            )
            .Returns(true);
        var result = await _useCase.ExecuteAsync(_dbModel);

        result.MustHaveValidationError(
            nameof(AddPowerToCharacterModel.PowerId),
            "The power already exists for this character."
        );
    }

    [Fact]
    public async Task ValidationFor_PowerId_WillFail_WhenItIsNotPartOfTheCharacterExpression()
    {
        A.CallTo(() =>
                _powerRepository.IsValidPowerForCharacter(
                    _dbModel.CharacterId,
                    _dbModel.PowerId
                )
            )
            .Returns(false);
        var result = await _useCase.ExecuteAsync(_dbModel);

        result.MustHaveValidationError(
            nameof(AddPowerToCharacterModel.PowerId),
            "The Power is not part of the Expression for the Character."
        );
    }

    [Fact]
    public async Task ValidationFor_Notes_WillFail_WhenMaxLengthIsGreaterThan5000()
    {
        _dbModel.Notes = new string('x', 5001);
        var result = await _useCase.ExecuteAsync(_dbModel);
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
        _dbModel.Notes = notes;
        var result = await _useCase.ExecuteAsync(_dbModel);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task UseCase_GetsPowerLevelExperience()
    {
        await _useCase.ExecuteAsync(_dbModel);

        A.CallTo(() => _powerRepository.GetPowerLevelForPower(_dbModel.PowerId))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_GrabsCorrectXp()
    {
        _powerLevel.Xp = 4;

        A.CallTo(() =>
                _xpRepository.GetAvailableXpForSection(_dbModel.CharacterId, XpSectionTypeEnum.Powers)
            )
            .Returns(new SectionXpDto()
            {
                AvailableXp = StartingExperience.StartingPowers,
                SpentXp = StartingExperience.StartingPowers + 1
            });

        var result = await _useCase.ExecuteAsync(_dbModel);

        Assert.Equal(4, ((NotEnoughXPFailure)result.Errors[0]).AmountTryingToSpend);
    }

    [Theory]
    [InlineData(4)]
    [InlineData(2)]
    public async Task UseCase_CalculatesAvailableXP_Correctly(int xpAmount)
    {
        _powerLevel.Xp = StartingExperience.StartingPowers;
        A.CallTo(() =>
                _xpRepository.GetAvailableXpForSection(_dbModel.CharacterId, XpSectionTypeEnum.Powers)
            )
            .Returns(new SectionXpDto()
            {
                AvailableXp = StartingExperience.StartingPowers,
                SpentXp = xpAmount
            });

        var result = await _useCase.ExecuteAsync(_dbModel);

        Assert.Equal(
            StartingExperience.StartingPowers - xpAmount,
            ((NotEnoughXPFailure)result.Errors[0]).AvailableXP
        );
    }

    [Fact]
    public async Task UseCase_WillReturnNotEnoughXp_WhenOutOfXp()
    {
        A.CallTo(() =>
                _xpRepository.GetAvailableXpForSection(_dbModel.CharacterId, XpSectionTypeEnum.Powers)
            )
            .Returns(new SectionXpDto()
            {
                AvailableXp = StartingExperience.StartingPowers,
                SpentXp = StartingExperience.StartingPowers
            });

        var result = await _useCase.ExecuteAsync(_dbModel);
        Assert.True(result.HasError<NotEnoughXPFailure>());
        Assert.Equal(0, ((NotEnoughXPFailure)result.Errors[0]).AvailableXP);
        // Should be the amount of xp needed to level up to the next level.
        Assert.Equal(4, ((NotEnoughXPFailure)result.Errors[0]).AmountTryingToSpend);
    }

    [Fact]
    public async Task UseCase_AddCharacterPowerMapping_WhenItHasEnoughXp()
    {
        await _useCase.ExecuteAsync(_dbModel);

        A.CallTo(() =>
                _mappingRepository.AddCharacterPowerMapping(
                    A<CharacterPowerMapping>.That.Matches(x =>
                        x.Notes == _dbModel.Notes
                        && x.PowerId == _dbModel.PowerId
                        && x.CharacterId == _dbModel.CharacterId
                        && x.PowerLevelId == _powerLevel.Id
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
        _dbModel.Notes = notes;

        var result = await _useCase.ExecuteAsync(_dbModel);
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
        A.CallTo(() => _mappingRepository.AddCharacterPowerMapping(A<CharacterPowerMapping>._))
            .Returns(5);
        var result = await _useCase.ExecuteAsync(_dbModel);
        Assert.Equal(5, result.Value);
    }
}

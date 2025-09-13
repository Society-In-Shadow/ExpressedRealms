using ExpressedRealms.Blessings.Repository.Blessings;
using ExpressedRealms.Blessings.Repository.CharacterBlessings;
using ExpressedRealms.Blessings.UseCases.CharacterBlessingMappings.Edit;
using ExpressedRealms.Characters.Repository;
using ExpressedRealms.DB.Models.Blessings.BlessingLevelSetup;
using ExpressedRealms.DB.Models.Blessings.CharacterBlessingMappings;
using ExpressedRealms.Shared;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using ExpressedRealms.UseCases.Shared.CommonFailureTypes;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Blessings.UseCases.Tests.Unit.CharacterBlessingMappings;

public class UpdateBlessingForCharacterUseCaseTests
{
    private readonly UpdateBlessingForCharacterUseCase _useCase;
    private readonly IBlessingRepository _blessingRepository;
    private readonly ICharacterBlessingRepository _mappingRepository;
    private readonly ICharacterRepository _characterRepository;
    private readonly UpdateBlessingForCharacterModel _blessingForCharacterModel;
    private readonly CharacterBlessingMapping _dbModel;

    public UpdateBlessingForCharacterUseCaseTests()
    {
        _blessingForCharacterModel = new UpdateBlessingForCharacterModel()
        {
            MappingId = 4,
            BlessingLevelId = 1,
            CharacterId = 2,
            Notes = "123",
        };

        _dbModel = new CharacterBlessingMapping()
        {
            BlessingLevelId = 6,
            CharacterId = 2,
            BlessingId = 3,
            Notes = "123",
        };

        _characterRepository = A.Fake<ICharacterRepository>();
        _blessingRepository = A.Fake<IBlessingRepository>();
        _mappingRepository = A.Fake<ICharacterBlessingRepository>();

        A.CallTo(() =>
                _characterRepository.CharacterExistsAsync(_blessingForCharacterModel.CharacterId)
            )
            .Returns(true);
        A.CallTo(() =>
                _blessingRepository.BlessingLevelExists(_blessingForCharacterModel.BlessingLevelId)
            )
            .Returns(true);
        A.CallTo(() =>
                _mappingRepository.MappingAlreadyExists(_blessingForCharacterModel.MappingId)
            )
            .Returns(true);
        A.CallTo(() =>
                _mappingRepository.GetExperienceSpentOnBlessingsForCharacter(
                    _blessingForCharacterModel.CharacterId
                )
            )
            .Returns(0);
        A.CallTo(() =>
                _blessingRepository.GetBlessingLevel(_blessingForCharacterModel.BlessingLevelId)
            )
            .Returns(new BlessingLevel() { XpCost = 4 });

        A.CallTo(() =>
                _mappingRepository.GetCharacterBlessingMappingForEditing(
                    _blessingForCharacterModel.MappingId
                )
            )
            .Returns(_dbModel);

        A.CallTo(() => _blessingRepository.GetBlessingLevel(_dbModel.BlessingLevelId))
            .Returns(new BlessingLevel() { XpCost = 4 });

        var validator = new UpdateBlessingForCharacterModelValidator(
            _blessingRepository,
            _characterRepository,
            _mappingRepository
        );

        _useCase = new UpdateBlessingForCharacterUseCase(
            _mappingRepository,
            _blessingRepository,
            validator,
            CancellationToken.None
        );
    }

    [Fact]
    public async Task ValidationFor_CharacterId_WillFail_WhenItsEmpty()
    {
        _blessingForCharacterModel.CharacterId = 0;
        var result = await _useCase.ExecuteAsync(_blessingForCharacterModel);

        result.MustHaveValidationError(
            nameof(UpdateBlessingForCharacterModel.CharacterId),
            "Character Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_CharacterId_WillFail_WhenItDoesNotExist()
    {
        A.CallTo(() =>
                _characterRepository.CharacterExistsAsync(_blessingForCharacterModel.CharacterId)
            )
            .Returns(false);
        var result = await _useCase.ExecuteAsync(_blessingForCharacterModel);

        result.MustHaveValidationError(
            nameof(UpdateBlessingForCharacterModel.CharacterId),
            "The Character does not exist."
        );
    }

    [Fact]
    public async Task ValidationFor_BlessingLevelId_WillFail_WhenItsEmpty()
    {
        _blessingForCharacterModel.BlessingLevelId = 0;
        var result = await _useCase.ExecuteAsync(_blessingForCharacterModel);

        result.MustHaveValidationError(
            nameof(UpdateBlessingForCharacterModel.BlessingLevelId),
            "Blessing Level Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_BlessingLevelId_WillFail_WhenItDoesNotExist()
    {
        A.CallTo(() =>
                _blessingRepository.BlessingLevelExists(_blessingForCharacterModel.BlessingLevelId)
            )
            .Returns(false);
        var result = await _useCase.ExecuteAsync(_blessingForCharacterModel);

        result.MustHaveValidationError(
            nameof(UpdateBlessingForCharacterModel.BlessingLevelId),
            "The Blessing Level does not exist."
        );
    }

    [Fact]
    public async Task ValidationFor_Notes_WillFail_WhenMaxLengthIsGreaterThan5000()
    {
        _blessingForCharacterModel.Notes = new string('x', 5001);
        var result = await _useCase.ExecuteAsync(_blessingForCharacterModel);
        result.MustHaveValidationError(
            nameof(UpdateBlessingForCharacterModel.Notes),
            "Notes must be less than 5000 characters."
        );
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task ValidationFor_Notes_AreOptional(string? notes)
    {
        _blessingForCharacterModel.Notes = notes;
        var result = await _useCase.ExecuteAsync(_blessingForCharacterModel);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task ValidationFor_MappingId_ChecksIfItExists()
    {
        A.CallTo(() =>
                _mappingRepository.MappingAlreadyExists(_blessingForCharacterModel.MappingId)
            )
            .Returns(false);
        var result = await _useCase.ExecuteAsync(_blessingForCharacterModel);
        result.MustHaveValidationError(
            nameof(UpdateBlessingForCharacterModel.MappingId),
            "The Blessing Mapping does not exist."
        );
    }

    [Fact]
    public async Task UseCase_GetsExperienceSpentOnBlessingsForCharacter()
    {
        await _useCase.ExecuteAsync(_blessingForCharacterModel);

        A.CallTo(() =>
                _mappingRepository.GetSpentXpForBlessingType(
                    _blessingForCharacterModel.CharacterId, _dbModel.BlessingId
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_GetsBlessingLevel()
    {
        await _useCase.ExecuteAsync(_blessingForCharacterModel);

        A.CallTo(() =>
                _blessingRepository.GetBlessingLevel(_blessingForCharacterModel.BlessingLevelId)
            )
            .MustHaveHappenedOnceExactly();
    }

    [Theory]
    [InlineData(4)]
    [InlineData(2)]
    public async Task UseCase_CalculatesAvailableXP_Correctly(int xpAmount)
    {
        A.CallTo(() =>
                _blessingRepository.GetBlessingLevel(_blessingForCharacterModel.BlessingLevelId)
            )
            .Returns(new BlessingLevel() { XpCost = StartingExperience.StartingBlessings });
        A.CallTo(() =>
                _mappingRepository.GetSpentXpForBlessingType(
                    _blessingForCharacterModel.CharacterId, _dbModel.BlessingId
                )
            )
            .Returns(xpAmount);

        var result = await _useCase.ExecuteAsync(_blessingForCharacterModel);

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
                    _blessingForCharacterModel.CharacterId, _dbModel.BlessingId
                )
            )
            .Returns(StartingExperience.StartingBlessings);

        var result = await _useCase.ExecuteAsync(_blessingForCharacterModel);
        Assert.True(result.HasError<NotEnoughXPFailure>());
        Assert.Equal(0, ((NotEnoughXPFailure)result.Errors[0]).AvailableXP);
        Assert.Equal(4, ((NotEnoughXPFailure)result.Errors[0]).AmountTryingToSpend);
    }

    [Fact]
    public async Task UseCase_PassesThrough_TheDbBlessing()
    {
        await _useCase.ExecuteAsync(_blessingForCharacterModel);
        A.CallTo(() =>
                _mappingRepository.UpdateMapping(
                    A<CharacterBlessingMapping>.That.IsSameAs(_dbModel)
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
        _blessingForCharacterModel.Notes = notes;

        var result = await _useCase.ExecuteAsync(_blessingForCharacterModel);
        Assert.True(result.IsSuccess);

        A.CallTo(() =>
                _mappingRepository.UpdateMapping(
                    A<CharacterBlessingMapping>.That.Matches(x => x.Notes == savedValue)
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_WillSave_BlessingLevelId()
    {
        _blessingForCharacterModel.BlessingLevelId = 20;
        A.CallTo(() =>
                _blessingRepository.BlessingLevelExists(_blessingForCharacterModel.BlessingLevelId)
            )
            .Returns(true);

        var result = await _useCase.ExecuteAsync(_blessingForCharacterModel);
        Assert.True(result.IsSuccess);

        A.CallTo(() =>
                _mappingRepository.UpdateMapping(
                    A<CharacterBlessingMapping>.That.Matches(x =>
                        x.BlessingLevelId == _blessingForCharacterModel.BlessingLevelId
                    )
                )
            )
            .MustHaveHappenedOnceExactly();
    }
}

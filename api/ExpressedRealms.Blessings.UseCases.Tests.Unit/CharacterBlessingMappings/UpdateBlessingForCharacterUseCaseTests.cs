using ExpressedRealms.Blessings.Repository.Blessings;
using ExpressedRealms.Blessings.Repository.CharacterBlessings;
using ExpressedRealms.Blessings.UseCases.CharacterBlessingMappings.Edit;
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

public class UpdateBlessingForCharacterUseCaseTests
{
    private readonly UpdateBlessingForCharacterUseCase _useCase;
    private readonly IBlessingRepository _blessingRepository;
    private readonly ICharacterBlessingRepository _mappingRepository;
    private readonly ICharacterRepository _characterRepository;
    private readonly IXpRepository _xpRepository;
    private readonly UpdateBlessingForCharacterModel _model;
    private readonly CharacterBlessingMapping _dbModel;

    private readonly Blessing _blessingDbModel;
    private readonly CharacterXpMapping _characterMappingDbModel;
    private readonly BlessingLevel _blessingLevelDbModel;
    
    public UpdateBlessingForCharacterUseCaseTests()
    {
        _model = new UpdateBlessingForCharacterModel()
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

        _blessingDbModel = new Blessing()
        {
            Name = "test",
            Description = "test",
            SubCategory = "Test",
            Type = "Disadvantage"
        };

        _blessingLevelDbModel = new BlessingLevel() { XpCost = 4 };

        _characterMappingDbModel = new CharacterXpMapping() { SectionCap = 8, SpentXp = 0 };
        
        _characterRepository = A.Fake<ICharacterRepository>();
        _blessingRepository = A.Fake<IBlessingRepository>();
        _mappingRepository = A.Fake<ICharacterBlessingRepository>();
        _xpRepository = A.Fake<IXpRepository>();

        A.CallTo(() =>
                _characterRepository.CharacterExistsAsync(_model.CharacterId)
            )
            .Returns(true);
        A.CallTo(() =>
                _blessingRepository.BlessingLevelExists(_model.BlessingLevelId)
            )
            .Returns(true);
        A.CallTo(() =>
                _mappingRepository.MappingAlreadyExists(_model.MappingId)
            )
            .Returns(true);
        A.CallTo(() => _xpRepository.GetCharacterXpMapping(_model.CharacterId, (int)XpSectionTypeEnum.Advantages))
            .Returns(_characterMappingDbModel);
        A.CallTo(() => _xpRepository.GetCharacterXpMapping(_model.CharacterId, (int)XpSectionTypeEnum.Disadvantages))
            .Returns(_characterMappingDbModel);
        A.CallTo(() =>
                _blessingRepository.GetBlessingLevel(_model.BlessingLevelId)
            )
            .Returns(_blessingLevelDbModel);

        A.CallTo(() =>
                _mappingRepository.GetCharacterBlessingMappingForEditing(
                    _model.MappingId
                )
            )
            .Returns(_dbModel);

        A.CallTo(() => _blessingRepository.GetBlessingLevel(_dbModel.BlessingLevelId))
            .Returns(_blessingLevelDbModel);
        
        A.CallTo(() => _characterRepository.GetCharacterState(_model.CharacterId))
            .Returns(new CharacterStatusDto() { IsInCharacterCreation = true });

        A.CallTo(() => _blessingRepository.GetBlessingForEditing(_dbModel.BlessingId))
            .Returns(_blessingDbModel);
        
        A.CallTo(() => _xpRepository.GetAvailableDiscretionary(_model.CharacterId))
            .Returns(16);

        var validator = new UpdateBlessingForCharacterModelValidator(
            _blessingRepository,
            _characterRepository,
            _mappingRepository
        );

        _useCase = new UpdateBlessingForCharacterUseCase(
            _mappingRepository,
            _blessingRepository,
            _characterRepository,
            _xpRepository,
            validator,
            CancellationToken.None
        );
    }

    [Fact]
    public async Task ValidationFor_CharacterId_WillFail_WhenItsEmpty()
    {
        _model.CharacterId = 0;
        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(UpdateBlessingForCharacterModel.CharacterId),
            "Character Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_CharacterId_WillFail_WhenItDoesNotExist()
    {
        A.CallTo(() =>
                _characterRepository.CharacterExistsAsync(_model.CharacterId)
            )
            .Returns(false);
        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(UpdateBlessingForCharacterModel.CharacterId),
            "The Character does not exist."
        );
    }

    [Fact]
    public async Task ValidationFor_BlessingLevelId_WillFail_WhenItsEmpty()
    {
        _model.BlessingLevelId = 0;
        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(UpdateBlessingForCharacterModel.BlessingLevelId),
            "Blessing Level Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_BlessingLevelId_WillFail_WhenItDoesNotExist()
    {
        A.CallTo(() =>
                _blessingRepository.BlessingLevelExists(_model.BlessingLevelId)
            )
            .Returns(false);
        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(UpdateBlessingForCharacterModel.BlessingLevelId),
            "The Blessing Level does not exist."
        );
    }

    [Fact]
    public async Task ValidationFor_Notes_WillFail_WhenMaxLengthIsGreaterThan5000()
    {
        _model.Notes = new string('x', 5001);
        var result = await _useCase.ExecuteAsync(_model);
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
        _model.Notes = notes;
        var result = await _useCase.ExecuteAsync(_model);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task ValidationFor_MappingId_ChecksIfItExists()
    {
        A.CallTo(() =>
                _mappingRepository.MappingAlreadyExists(_model.MappingId)
            )
            .Returns(false);
        var result = await _useCase.ExecuteAsync(_model);
        result.MustHaveValidationError(
            nameof(UpdateBlessingForCharacterModel.MappingId),
            "The Blessing Mapping does not exist."
        );
    }

    [Fact]
    public async Task UseCase_ReturnsError_WhenModifyingOutsideOfCharacterCreation()
    {
        A.CallTo(() => _characterRepository.GetCharacterState(_model.CharacterId))
            .Returns(new CharacterStatusDto() { IsInCharacterCreation = false });
        
        var result = await _useCase.ExecuteAsync(_model);

        Assert.False(result.IsSuccess);
        Assert.Equal("You cannot edit Advantages or Disadvantages outside of character creation.", result.Errors.First().Message);
    }
    
    [Theory]
    [InlineData("Advantage", "Advantages")]
    [InlineData("Disadvantage", "Disadvantages")]
    public async Task UseCase_ReturnsError_WhenAddingMoreThan8PointsOfAdvantagesOrDisadvantages(string blessingType, string messageString)
    {
        _blessingDbModel.Type = blessingType;
        _characterMappingDbModel.SpentXp = 20;
        
        var result = await _useCase.ExecuteAsync(_model);

        Assert.False(result.IsSuccess);
        Assert.Equal($"You cannot add more than 8 points of {messageString}.", result.Errors.First().Message);
    }

    [Fact]
    public async Task UseCase_CalculatesSpentXp_AsSumOfOldSpentXpAndNewLevelCost_ForAdvantages()
    {
        _blessingDbModel.Type = "advantage";
        _characterMappingDbModel.SpentXp = 4;
        _blessingLevelDbModel.XpCost = 2;

        await _useCase.ExecuteAsync(_model);

        A.CallTo(() => _xpRepository.UpdateXpInfo(
            A<CharacterXpMapping>.That.Matches(x => x.SpentXp == 6)) 
        ).MustHaveHappenedOnceExactly();
    }
    
    [Fact]
    public async Task UseCase_CalculatesDiscretionXp_AsSumOfOldSpentXpAndNewLevelCost_ForAdvantages()
    {
        _blessingDbModel.Type = "advantage";
        _characterMappingDbModel.SpentXp = 4;
        _blessingLevelDbModel.XpCost = 2;

        await _useCase.ExecuteAsync(_model);

        A.CallTo(() => _xpRepository.UpdateXpInfo(
            A<CharacterXpMapping>.That.Matches(x => x.DiscretionXp == 6)) 
        ).MustHaveHappenedOnceExactly();
    }
    
    /// <summary>
    /// For blessings, we don't need to take into consideration the section cap, so this is just the straight value
    /// of the old + new level cost.
    /// </summary>
    [Fact]
    public async Task UseCase_CalculatesTotalCharacterCreationXp_AsSumOfOldSpentXpAndNewLevelCost_ForAdvantages()
    {
        _blessingDbModel.Type = "advantage";
        _characterMappingDbModel.SpentXp = 4;
        _blessingLevelDbModel.XpCost = 2;

        await _useCase.ExecuteAsync(_model);

        A.CallTo(() => _xpRepository.UpdateXpInfo(
            A<CharacterXpMapping>.That.Matches(x => x.TotalCharacterCreationXp == 6)) 
        ).MustHaveHappenedOnceExactly();
    }
    
    /// <summary>
    /// Advantages and Disadvantages are not relevant to the Level XP calculation, so this is just 0.
    /// </summary>
    [Fact]
    public async Task UseCase_CalculatesLevelXp_As0_ForAdvantages()
    {
        _blessingDbModel.Type = "advantage";
        
        await _useCase.ExecuteAsync(_model);

        A.CallTo(() => _xpRepository.UpdateXpInfo(
            A<CharacterXpMapping>.That.Matches(x => x.LevelXp == 0)) 
        ).MustHaveHappenedOnceExactly();
    }
    
        [Fact]
    public async Task UseCase_CalculatesSpentXp_AsSumOfOldSpentXpAndNewLevelCost_ForDisadvantages()
    {
        _blessingDbModel.Type = "disadvantage";
        _characterMappingDbModel.SpentXp = 4;
        _blessingLevelDbModel.XpGain = 2;

        await _useCase.ExecuteAsync(_model);

        A.CallTo(() => _xpRepository.UpdateXpInfo(
            A<CharacterXpMapping>.That.Matches(x => x.SpentXp == 6)) 
        ).MustHaveHappenedOnceExactly();
    }
    
    [Fact]
    public async Task UseCase_CalculatesDiscretionXp_AsSumOfOldSpentXpAndNewLevelCost_ForDisadvantages()
    {
        _blessingDbModel.Type = "disadvantage";
        _characterMappingDbModel.SpentXp = 4;
        _blessingLevelDbModel.XpGain = 2;

        await _useCase.ExecuteAsync(_model);

        A.CallTo(() => _xpRepository.UpdateXpInfo(
            A<CharacterXpMapping>.That.Matches(x => x.DiscretionXp == 6)) 
        ).MustHaveHappenedOnceExactly();
    }
    
    /// <summary>
    /// For blessings, we don't need to take into consideration the section cap, so this is just the straight value
    /// of the old + new level cost.
    /// </summary>
    [Fact]
    public async Task UseCase_CalculatesTotalCharacterCreationXp_AsSumOfOldSpentXpAndNewLevelCost_ForDisadvantages()
    {
        _blessingDbModel.Type = "disadvantage";
        _characterMappingDbModel.SpentXp = 4;
        _blessingLevelDbModel.XpGain = 2;

        await _useCase.ExecuteAsync(_model);

        A.CallTo(() => _xpRepository.UpdateXpInfo(
            A<CharacterXpMapping>.That.Matches(x => x.TotalCharacterCreationXp == 6)) 
        ).MustHaveHappenedOnceExactly();
    }
    
    /// <summary>
    /// Advantages and Disadvantages are not relevant to the Level XP calculation, so this is just 0.
    /// </summary>
    [Fact]
    public async Task UseCase_CalculatesLevelXp_As0_ForDisadvantages()
    {
        _blessingDbModel.Type = "disadvantage";

        await _useCase.ExecuteAsync(_model);

        A.CallTo(() => _xpRepository.UpdateXpInfo(
            A<CharacterXpMapping>.That.Matches(x => x.LevelXp == 0)) 
        ).MustHaveHappenedOnceExactly();
    }
    
    [Fact]
    public async Task UseCase_CanHandle_GainingXp()
    {
        _blessingDbModel.Type = "advantage";
        _characterMappingDbModel.SpentXp = 4;
        _blessingLevelDbModel.XpCost = 2;
        A.CallTo(() => _xpRepository.GetAvailableDiscretionary(_model.CharacterId))
            .Returns(6);
        
        A.CallTo(() => _blessingRepository.GetBlessingLevel(_dbModel.BlessingLevelId))
            .Returns(new BlessingLevel() { XpCost = 4 });

        await _useCase.ExecuteAsync(_model);
        A.CallTo(() => _xpRepository.UpdateXpInfo(
            A<CharacterXpMapping>.That.Matches(x => x.DiscretionXp == 2)) 
        ).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_WillReturnNotEnoughXp_WhenOutOfXp()
    {
        _blessingDbModel.Type = "advantage";
        _characterMappingDbModel.SpentXp = 4;
        _blessingLevelDbModel.XpCost = 2;
        A.CallTo(() => _xpRepository.GetAvailableDiscretionary(_model.CharacterId))
            .Returns(4);

        var result = await _useCase.ExecuteAsync(_model);
        Assert.True(result.HasError<NotEnoughXPFailure>());
        Assert.Equal(4, ((NotEnoughXPFailure)result.Errors[0]).AvailableXP);
        Assert.Equal(2, ((NotEnoughXPFailure)result.Errors[0]).AmountTryingToSpend);
    }

    [Fact]
    public async Task UseCase_PassesThrough_TheDbBlessing()
    {
        await _useCase.ExecuteAsync(_model);
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
        _model.Notes = notes;

        var result = await _useCase.ExecuteAsync(_model);
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
        _model.BlessingLevelId = 20;
        A.CallTo(() =>
                _blessingRepository.BlessingLevelExists(_model.BlessingLevelId)
            )
            .Returns(true);

        var result = await _useCase.ExecuteAsync(_model);
        Assert.True(result.IsSuccess);

        A.CallTo(() =>
                _mappingRepository.UpdateMapping(
                    A<CharacterBlessingMapping>.That.Matches(x =>
                        x.BlessingLevelId == _model.BlessingLevelId
                    )
                )
            )
            .MustHaveHappenedOnceExactly();
    }
}

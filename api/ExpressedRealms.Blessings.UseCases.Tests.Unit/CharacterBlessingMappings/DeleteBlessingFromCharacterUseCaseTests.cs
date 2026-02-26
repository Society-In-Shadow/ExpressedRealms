using ExpressedRealms.Blessings.Repository.Blessings;
using ExpressedRealms.Blessings.Repository.CharacterBlessings;
using ExpressedRealms.Blessings.UseCases.CharacterBlessingMappings.Delete;
using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.Repository.DTOs;
using ExpressedRealms.Characters.Repository.Xp;
using ExpressedRealms.DB.Models.Blessings.BlessingLevelSetup;
using ExpressedRealms.DB.Models.Blessings.BlessingSetup;
using ExpressedRealms.DB.Models.Blessings.CharacterBlessingMappings;
using ExpressedRealms.DB.Models.Characters.XpTables;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Blessings.UseCases.Tests.Unit.CharacterBlessingMappings;

public class DeleteBlessingFromCharacterUseCaseTests
{
    private readonly DeleteBlessingFromCharacterUseCase _useCase;
    private readonly ICharacterBlessingRepository _mappingRepository;
    private readonly ICharacterRepository _characterRepository;
    private readonly IBlessingRepository _blessingRepository;
    private readonly IXpRepository _xpRepository;
    private readonly DeleteBlessingFromCharacterModel _model;
    private readonly CharacterBlessingMapping _dbModel;

    private readonly Blessing _blessingDbModel;
    private readonly CharacterXpView _characterMappingDbModel;
    private readonly BlessingLevel _blessingLevelDbModel;

    public DeleteBlessingFromCharacterUseCaseTests()
    {
        _model = new DeleteBlessingFromCharacterModel() { MappingId = 4, CharacterId = 2 };

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
            Type = "Disadvantage",
        };

        _blessingLevelDbModel = new BlessingLevel() { XpCost = 4 };

        _characterMappingDbModel = new CharacterXpView() { SectionCap = 8, SpentXp = 0 };

        _characterRepository = A.Fake<ICharacterRepository>();
        _mappingRepository = A.Fake<ICharacterBlessingRepository>();
        _blessingRepository = A.Fake<IBlessingRepository>();
        _xpRepository = A.Fake<IXpRepository>();

        A.CallTo(() => _characterRepository.CharacterExistsAsync(_model.CharacterId)).Returns(true);

        A.CallTo(() => _mappingRepository.MappingAlreadyExists(_model.MappingId)).Returns(true);

        A.CallTo(() => _mappingRepository.GetCharacterBlessingMappingForEditing(_model.MappingId))
            .Returns(_dbModel);

        A.CallTo(() => _characterRepository.GetCharacterState(_model.CharacterId))
            .Returns(new CharacterStatusDto() { IsInCharacterCreation = true });

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

        A.CallTo(() => _blessingRepository.GetBlessingLevel(_dbModel.BlessingLevelId))
            .Returns(_blessingLevelDbModel);

        A.CallTo(() => _blessingRepository.GetBlessingForEditing(_dbModel.BlessingId))
            .Returns(_blessingDbModel);

        var validator = new DeleteBlessingFromCharacterModelValidator(
            _characterRepository,
            _mappingRepository
        );

        _useCase = new DeleteBlessingFromCharacterUseCase(
            _mappingRepository,
            validator,
            _characterRepository,
            CancellationToken.None
        );
    }

    [Fact]
    public async Task ValidationFor_CharacterId_WillFail_WhenItsEmpty()
    {
        _model.CharacterId = 0;
        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(DeleteBlessingFromCharacterModel.CharacterId),
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
            nameof(DeleteBlessingFromCharacterModel.CharacterId),
            "The Character does not exist."
        );
    }

    [Fact]
    public async Task ValidationFor_MappingId_WillFail_WhenItsEmpty()
    {
        _model.MappingId = 0;
        var result = await _useCase.ExecuteAsync(_model);
        result.MustHaveValidationError(
            nameof(DeleteBlessingFromCharacterModel.MappingId),
            "Mapping Id is required."
        );
    }

    [Fact]
    public async Task UseCase_ReturnsError_WhenModifyingOutsideOfCharacterCreation()
    {
        A.CallTo(() => _characterRepository.GetCharacterState(_model.CharacterId))
            .Returns(new CharacterStatusDto() { IsInCharacterCreation = false });

        var result = await _useCase.ExecuteAsync(_model);

        Assert.False(result.IsSuccess);
        Assert.Equal(
            "You cannot delete Advantages or Disadvantages outside of character creation.",
            result.Errors.First().Message
        );
    }

    [Fact]
    public async Task ValidationFor_MappingId_ChecksIfItExists()
    {
        A.CallTo(() => _mappingRepository.MappingAlreadyExists(_model.MappingId)).Returns(false);
        var result = await _useCase.ExecuteAsync(_model);
        result.MustHaveValidationError(
            nameof(DeleteBlessingFromCharacterModel.MappingId),
            "The Blessing Mapping does not exist."
        );
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

    [Fact]
    public async Task UseCase_WillUpdateDeleteFields()
    {
        var result = await _useCase.ExecuteAsync(_model);
        Assert.True(result.IsSuccess);

        A.CallTo(() =>
                _mappingRepository.UpdateMapping(
                    A<CharacterBlessingMapping>.That.Matches(x => x.IsDeleted)
                )
            )
            .MustHaveHappenedOnceExactly();
    }
}

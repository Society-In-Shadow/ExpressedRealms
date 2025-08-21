using ExpressedRealms.Characters.Repository;
using ExpressedRealms.DB.Models.Powers.CharacterPowerMappingSetup;
using ExpressedRealms.Powers.Repository.CharacterPower;
using ExpressedRealms.Powers.Repository.Powers;
using ExpressedRealms.Powers.UseCases.CharacterPower.Delete;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Powers.UseCases.Tests.Unit.CharacterPower;

public class DeletePowerFromCharacterUseCaseTests
{
    private readonly DeletePowerFromCharacterUseCase _useCase;
    private readonly ICharacterPowerRepository _mappingRepository;
    private readonly DeletePowerFromCharacterModel _powerToCharacterModel;
    private readonly CharacterPowerMapping _mapping;
    private readonly ICharacterRepository _characterRepository;
    private readonly IPowerRepository _powerRepository;

    public DeletePowerFromCharacterUseCaseTests()
    {
        _powerToCharacterModel = new DeletePowerFromCharacterModel()
        {
            CharacterId = 1,
            PowerId = 2,
        };

        _mapping = new CharacterPowerMapping()
        {
            CharacterId = 1,
            PowerId = 2,
            Notes = "123",
            Id = 4,
        };

        _mappingRepository = A.Fake<ICharacterPowerRepository>();

        _mappingRepository = A.Fake<ICharacterPowerRepository>();
        _characterRepository = A.Fake<ICharacterRepository>();
        _powerRepository = A.Fake<IPowerRepository>();

        A.CallTo(() =>
                _characterRepository.CharacterExistsAsync(_powerToCharacterModel.CharacterId)
            )
            .Returns(true);

        A.CallTo(() => _powerRepository.IsValidPower(_powerToCharacterModel.PowerId)).Returns(true);

        A.CallTo(() =>
                _mappingRepository.MappingExistsAsync(
                    _powerToCharacterModel.PowerId,
                    _powerToCharacterModel.CharacterId
                )
            )
            .Returns(true);

        A.CallTo(() =>
                _mappingRepository.GetCharacterPowerMapping(
                    _powerToCharacterModel.CharacterId,
                    _powerToCharacterModel.PowerId
                )
            )
            .Returns(_mapping);
        
        A.CallTo(() =>
                _mappingRepository.IsPowerPartOfPrerequisite(
                    _powerToCharacterModel.CharacterId,
                    _powerToCharacterModel.PowerId
                )
            )
            .Returns(false);

        var validator = new DeletePowerFromCharacterModelValidator(
            _characterRepository,
            _powerRepository,
            _mappingRepository
        );

        _useCase = new DeletePowerFromCharacterUseCase(
            _mappingRepository,
            validator,
            CancellationToken.None
        );
    }

    [Fact]
    public async Task ValidationFor_CharacterId_WillFail_WhenItsEmpty()
    {
        _powerToCharacterModel.CharacterId = 0;
        var result = await _useCase.ExecuteAsync(_powerToCharacterModel);

        result.MustHaveValidationError(
            nameof(DeletePowerFromCharacterModel.CharacterId),
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
            nameof(DeletePowerFromCharacterModel.CharacterId),
            "The Character does not exist."
        );
    }

    [Fact]
    public async Task ValidationFor_PowerId_WillFail_WhenItsEmpty()
    {
        _powerToCharacterModel.PowerId = 0;
        var result = await _useCase.ExecuteAsync(_powerToCharacterModel);

        result.MustHaveValidationError(
            nameof(DeletePowerFromCharacterModel.PowerId),
            "Power Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_PowerId_WillFail_WhenItDoesNotExist()
    {
        A.CallTo(() => _powerRepository.IsValidPower(_powerToCharacterModel.PowerId))
            .Returns(false);
        var result = await _useCase.ExecuteAsync(_powerToCharacterModel);

        result.MustHaveValidationError(
            nameof(DeletePowerFromCharacterModel.PowerId),
            "The Power does not exist."
        );
    }

    [Fact]
    public async Task ValidationFor_PowerId_WillFail_WhenMappingDoesNotExist()
    {
        A.CallTo(() =>
                _mappingRepository.MappingExistsAsync(
                    _powerToCharacterModel.PowerId,
                    _powerToCharacterModel.CharacterId
                )
            )
            .Returns(false);
        var result = await _useCase.ExecuteAsync(_powerToCharacterModel);

        result.MustHaveValidationError(
            nameof(DeletePowerFromCharacterModel.PowerId),
            "The Mapping does not exist."
        );
    }
    
    [Fact]
    public async Task ValidationFor_PowerId_WillFail_WhenMappingIsPartOfAPrerequisite()
    {
        A.CallTo(() =>
                _mappingRepository.IsPowerPartOfPrerequisite(
                    _powerToCharacterModel.CharacterId,
                    _powerToCharacterModel.PowerId
                )
            )
            .Returns(true);
        var result = await _useCase.ExecuteAsync(_powerToCharacterModel);

        result.MustHaveValidationError(
            nameof(DeletePowerFromCharacterModel.PowerId),
            "The Power is Part of a Prerequisite."
        );
    }

    [Fact]
    public async Task UseCase_SaveMapping()
    {
        var result = await _useCase.ExecuteAsync(_powerToCharacterModel);
        Assert.True(result.IsSuccess);
        A.CallTo(() => _mappingRepository.UpdateCharacterPowerMapping(_mapping))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_UpdatesDeleteStatus()
    {
        var result = await _useCase.ExecuteAsync(_powerToCharacterModel);
        Assert.True(result.IsSuccess);
        A.CallTo(() =>
                _mappingRepository.UpdateCharacterPowerMapping(
                    A<CharacterPowerMapping>.That.Matches(x => x.IsDeleted == true)
                )
            )
            .MustHaveHappenedOnceExactly();
    }
}

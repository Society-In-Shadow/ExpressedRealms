using ExpressedRealms.Characters.Repository;
using ExpressedRealms.DB.Models.Powers.CharacterPowerMappingSetup;
using ExpressedRealms.Powers.Repository.CharacterPower;
using ExpressedRealms.Powers.Repository.Powers;
using ExpressedRealms.Powers.UseCases.CharacterPower.Create;
using ExpressedRealms.Powers.UseCases.CharacterPower.Edit;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Powers.UseCases.Tests.Unit.CharacterPower;

public class UpdatePowerForCharacterUseCaseTests
{
    private readonly UpdatePowerForCharacterUseCase _useCase;
    private readonly ICharacterPowerRepository _mappingRepository;
    private readonly UpdatePowerForCharacterModel _powerToCharacterModel;
    private readonly CharacterPowerMapping _mapping;
    private readonly ICharacterRepository _characterRepository;
    private readonly IPowerRepository _powerRepository;

    public UpdatePowerForCharacterUseCaseTests()
    {
        _powerToCharacterModel = new UpdatePowerForCharacterModel()
        {
            CharacterId = 1,
            PowerId = 2,
            Notes = "123",
        };

        _mapping = new CharacterPowerMapping()
        {
            CharacterId = 1,
            PowerId = 2,
            Notes = "123",
            Id = 4,
        };

        _mappingRepository = A.Fake<ICharacterPowerRepository>();
        _characterRepository = A.Fake<ICharacterRepository>();
        _powerRepository = A.Fake<IPowerRepository>();

        A.CallTo(() => _characterRepository.CharacterExistsAsync(_powerToCharacterModel.CharacterId))
            .Returns(true);
        
        A.CallTo(() => _powerRepository.IsValidPower(_powerToCharacterModel.PowerId))
            .Returns(true);
        
        A.CallTo(() =>
                _mappingRepository.GetCharacterPowerMapping(_powerToCharacterModel.CharacterId, _powerToCharacterModel.PowerId)
            )
            .Returns(_mapping);

        var validator = new UpdatePowerForCharacterModelValidator(_characterRepository, _powerRepository);

        _useCase = new UpdatePowerForCharacterUseCase(
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
            nameof(UpdatePowerForCharacterModel.CharacterId),
            "Character Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_CharacterId_WillFail_WhenItDoesNotExist()
    {
        A.CallTo(() => _characterRepository.CharacterExistsAsync(_powerToCharacterModel.CharacterId))
            .Returns(false);
        var result = await _useCase.ExecuteAsync(_powerToCharacterModel);

        result.MustHaveValidationError(
            nameof(UpdatePowerForCharacterModel.CharacterId),
            "The Character does not exist."
        );
    }
    
    [Fact]
    public async Task ValidationFor_PowerId_WillFail_WhenItsEmpty()
    {
        _powerToCharacterModel.PowerId = 0;
        var result = await _useCase.ExecuteAsync(_powerToCharacterModel);

        result.MustHaveValidationError(
            nameof(UpdatePowerForCharacterModel.PowerId),
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
            nameof(UpdatePowerForCharacterModel.PowerId),
            "The Power does not exist."
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
                _mappingRepository.UpdateCharacterPowerMapping(
                    A<CharacterPowerMapping>.That.Matches(x => x.Notes == savedValue)
                )
            )
            .MustHaveHappenedOnceExactly();
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
    public async Task UseCase_WillUpdate_Notes()
    {
        _powerToCharacterModel.Notes = "test";
        var result = await _useCase.ExecuteAsync(_powerToCharacterModel);
        Assert.True(result.IsSuccess);
        A.CallTo(() =>
                _mappingRepository.UpdateCharacterPowerMapping(
                    A<CharacterPowerMapping>.That.Matches(x =>
                        x.Notes == _powerToCharacterModel.Notes
                    )
                )
            )
            .MustHaveHappenedOnceExactly();
    }
}

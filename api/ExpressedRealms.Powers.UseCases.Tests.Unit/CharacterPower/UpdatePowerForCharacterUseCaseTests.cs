using ExpressedRealms.DB.Models.Powers.CharacterPowerMappingSetup;
using ExpressedRealms.Powers.Repository.CharacterPower;
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

    public UpdatePowerForCharacterUseCaseTests()
    {
        _powerToCharacterModel = new UpdatePowerForCharacterModel()
        {
            MappingId = 1,
            Notes = "123",
        };

        _mappingRepository = A.Fake<ICharacterPowerRepository>();

        A.CallTo(() => _mappingRepository.IsValidMapping(_powerToCharacterModel.MappingId))
            .Returns(true);
        
        var validator = new UpdatePowerForCharacterModelValidator(
            _mappingRepository
        );

        _useCase = new UpdatePowerForCharacterUseCase(
            _mappingRepository,
            validator,
            CancellationToken.None
        );
    }

    [Fact]
    public async Task ValidationFor_MappingId_WillFail_WhenItsEmpty()
    {
        _powerToCharacterModel.MappingId = 0;
        var result = await _useCase.ExecuteAsync(_powerToCharacterModel);

        result.MustHaveValidationError(
            nameof(UpdatePowerForCharacterModel.MappingId),
            "Mapping Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_MappingId_WillFail_WhenItDoesNotExist()
    {
        A.CallTo(() => _mappingRepository.IsValidMapping(_powerToCharacterModel.MappingId))
            .Returns(false);
        var result = await _useCase.ExecuteAsync(_powerToCharacterModel);

        result.MustHaveValidationError(
            nameof(UpdatePowerForCharacterModel.MappingId),
            "The Mapping does not exist."
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
}

using ExpressedRealms.DB.Models.Powers.CharacterPowerMappingSetup;
using ExpressedRealms.Powers.Repository.CharacterPower;
using ExpressedRealms.Powers.UseCases.CharacterPower.Delete;
using ExpressedRealms.Powers.UseCases.CharacterPower.Edit;
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


    public DeletePowerFromCharacterUseCaseTests()
    {
        _powerToCharacterModel = new DeletePowerFromCharacterModel()
        {
            MappingId = 1
        };

        _mapping = new CharacterPowerMapping()
        {
            CharacterId = 1,
            PowerId = 2,
            Notes = "123",
            Id = 4
        };
        
        _mappingRepository = A.Fake<ICharacterPowerRepository>();

        A.CallTo(() => _mappingRepository.IsValidMapping(_powerToCharacterModel.MappingId))
            .Returns(true);
        
        A.CallTo(() => _mappingRepository.GetCharacterPowerMapping(_powerToCharacterModel.MappingId)).Returns(_mapping);

        var validator = new DeletePowerFromCharacterModelValidator(_mappingRepository);

        _useCase = new DeletePowerFromCharacterUseCase(
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
    public async Task UseCase_SaveMapping()
    {
        var result = await _useCase.ExecuteAsync(_powerToCharacterModel);
        Assert.True(result.IsSuccess);
        A.CallTo( () => _mappingRepository.UpdateCharacterPowerMapping(_mapping)).MustHaveHappenedOnceExactly();
    }
    
    [Fact]
    public async Task UseCase_UpdatesDeleteStatus()
    {
        var result = await _useCase.ExecuteAsync(_powerToCharacterModel);
        Assert.True(result.IsSuccess);
        A.CallTo( () => _mappingRepository.UpdateCharacterPowerMapping(A<CharacterPowerMapping>.That.Matches(x => x.IsDeleted == true))).MustHaveHappenedOnceExactly();
    }
    
}

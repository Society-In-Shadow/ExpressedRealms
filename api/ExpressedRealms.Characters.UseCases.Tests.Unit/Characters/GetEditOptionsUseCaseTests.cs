using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.UseCases.Characters.GetEditOptions;
using ExpressedRealms.DB.Characters;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Characters.UseCases.Tests.Unit.Characters;

public class GetEditOptionsUseCaseTests
{
    private readonly GetEditOptionsUseCase _useCaseTests;
    private readonly ICharacterRepository _characterRepository;

    private readonly GetEditOptionsModel _model;

    public GetEditOptionsUseCaseTests()
    {
        _model = new GetEditOptionsModel() { Id = 4 };

        _characterRepository = A.Fake<ICharacterRepository>();

        A.CallTo(() => _characterRepository.FindCharacterAsync(_model.Id)).Returns(new Character());

        A.CallTo(() => _characterRepository.FindCharacterAsync(_model.Id)).Returns(new Character());

        var validator = new GetEditOptionsModelValidator(_characterRepository);

        _useCaseTests = new GetEditOptionsUseCase(
            _characterRepository,
            validator,
            CancellationToken.None
        );
    }

    [Fact]
    public async Task ValidationFor_CharacterId_WillFail_WhenItsEmpty()
    {
        _model.Id = 0;

        var results = await _useCaseTests.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(GetEditOptionsModel.Id),
            "Character Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_CharacterId_WillFail_WhenIt_DoesNotExist()
    {
        A.CallTo(() => _characterRepository.FindCharacterAsync(_model.Id))
            .Returns(Task.FromResult<Character?>(null));

        var results = await _useCaseTests.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(GetEditOptionsModel.Id),
            "The Character Id does not exist."
        );
    }

    [Fact]
    public async Task UseCase_WithEventId_PassesThrough_AllAssignedXp_ForSaidEvent()
    {
        A.CallTo(() => _characterRepository.CanUpdatePrimaryCharacterStatus(_model.Id))
            .Returns(true);

        var results = await _useCaseTests.ExecuteAsync(_model);

        Assert.True(results.Value.CanModifyPrimaryCharacter);
    }
}

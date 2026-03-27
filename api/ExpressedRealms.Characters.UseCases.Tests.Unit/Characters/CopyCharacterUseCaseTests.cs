using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.UseCases.Characters.CopyCharacter;
using ExpressedRealms.Repositories.Shared.ExternalDependencies;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Characters.UseCases.Tests.Unit.Characters;

public class CopyCharacterUseCaseTests
{
    private readonly CopyCharacterUseCase _useCase;

    private readonly ICharacterRepository _characterRepository;
    private readonly IUserContext _userContext;

    private readonly CopyCharacterModel _model;
    private readonly Guid PlayerId = Guid.NewGuid();

    public CopyCharacterUseCaseTests()
    {
        _model = new CopyCharacterModel() { Id = 10, CharacterName = "Updated Name" };

        _characterRepository = A.Fake<ICharacterRepository>();
        _userContext = A.Fake<IUserContext>();

        var userId = Guid.NewGuid();
        A.CallTo(() => _userContext.CurrentUserId()).Returns(userId.ToString());
        A.CallTo(() => _characterRepository.CharacterExistsAsync(_model.Id)).Returns(true);
        A.CallTo(() => _characterRepository.GetPlayerId(userId.ToString())).Returns(PlayerId);
        A.CallTo(() =>
                _characterRepository.CopyCharacterAsync(_model.Id, PlayerId, _model.CharacterName)
            )
            .Returns(3);

        var validator = new CopyCharacterModelValidator(_characterRepository);

        _useCase = new CopyCharacterUseCase(
            _characterRepository,
            _userContext,
            validator,
            CancellationToken.None
        );
    }

    [Fact]
    public async Task ValidationFor_Id_WillFail_WhenItsEmpty()
    {
        _model.Id = 0;
        var result = await _useCase.ExecuteAsync(_model);
        result.MustHaveValidationError(nameof(CopyCharacterModel.Id), "'Id' must not be empty.");
    }

    [Fact]
    public async Task ValidationFor_Id_WillFail_WhenItDoesNotExist()
    {
        A.CallTo(() => _characterRepository.CharacterExistsAsync(_model.Id)).Returns(false);

        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(nameof(CopyCharacterModel.Id), "Character does not exist.");
    }

    [Fact]
    public async Task ValidationFor_CharacterName_WillFail_WhenItsEmpty()
    {
        _model.CharacterName = string.Empty;

        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(CopyCharacterModel.CharacterName),
            "'Character Name' must not be empty."
        );
    }

    [Fact]
    public async Task ValidationFor_Name_WillFail_WhenItIsOver150Characters()
    {
        _model.CharacterName = new string('x', 151);

        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(CopyCharacterModel.CharacterName),
            "The length of 'Character Name' must be 150 characters or fewer. You entered 151 characters."
        );
    }

    [Fact]
    public async Task UseCase_WillCopy_Character()
    {
        await _useCase.ExecuteAsync(_model);

        A.CallTo(() =>
                _characterRepository.CopyCharacterAsync(_model.Id, PlayerId, _model.CharacterName)
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_WillReturn_NewCharacterId()
    {
        var response = await _useCase.ExecuteAsync(_model);

        Assert.Equal(3, response.Value);
    }
}

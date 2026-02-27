using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.UseCases.Characters.Edit;
using ExpressedRealms.DB.Models.Characters;
using ExpressedRealms.Expressions.Repository.ProgressionPaths;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Characters.UseCases.Tests.Unit.Characters;

public class EditCharacterUseCaseTests
{
    private readonly EditCharacterUseCase _useCase;

    private readonly ICharacterRepository _characterRepository;
    private readonly IProgressionPathRepository _progressionPathRepository;

    private readonly EditCharacterModel _model;
    private readonly Character _dbModel;

    public EditCharacterUseCaseTests()
    {
        _model = new EditCharacterModel()
        {
            Id = 10,
            Name = "Updated Name",
            Background = "Updated Background",
            FactionId = 7,
            IsPrimaryCharacter = false,
            PrimaryProgressionId = 111,
            SecondaryProgressionId = 222,
        };

        _dbModel = new Character()
        {
            Id = _model.Id,
            Name = "Original Name",
            Background = "Original Background",
            ExpressionId = 3, // valid for PrimaryProgression
            FactionId = null,
            IsPrimaryCharacter = false,
            PrimaryProgressionId = null,
            SecondaryProgressionId = null,
        };

        _characterRepository = A.Fake<ICharacterRepository>();
        _progressionPathRepository = A.Fake<IProgressionPathRepository>();

        A.CallTo(() => _characterRepository.FindCharacterAsync(_model.Id)).Returns(_dbModel);

        A.CallTo(() =>
                _progressionPathRepository.ProgressionPathExists(_model.PrimaryProgressionId!.Value)
            )
            .Returns(true);
        A.CallTo(() =>
                _progressionPathRepository.ProgressionPathExists(
                    _model.SecondaryProgressionId!.Value
                )
            )
            .Returns(true);

        A.CallTo(() => _characterRepository.CanUpdatePrimaryCharacterStatus(_model.Id))
            .Returns(true);

        var validator = new EditCharacterModelValidator(
            _progressionPathRepository,
            _characterRepository
        );

        _useCase = new EditCharacterUseCase(
            _characterRepository,
            validator,
            CancellationToken.None
        );
    }

    [Fact]
    public async Task ValidationFor_Id_WillFail_WhenItsEmpty()
    {
        _model.Id = 0;

        // keep this test focused on NotEmpty instead of also failing "does not exist"
        A.CallTo(() => _characterRepository.FindCharacterAsync(_model.Id)).Returns(_dbModel);

        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(nameof(EditCharacterModel.Id), "'Id' must not be empty.");
    }

    [Fact]
    public async Task ValidationFor_Id_WillFail_WhenItsNegative()
    {
        _model.Id = -1;

        // keep this test focused on GreaterThan instead of also failing "does not exist"
        A.CallTo(() => _characterRepository.FindCharacterAsync(_model.Id)).Returns(_dbModel);

        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(EditCharacterModel.Id),
            "'Id' must be greater than '0'."
        );
    }

    [Fact]
    public async Task ValidationFor_Id_WillFail_WhenItDoesNotExist()
    {
        A.CallTo(() => _characterRepository.FindCharacterAsync(_model.Id))
            .Returns(Task.FromResult<Character?>(null));

        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(nameof(EditCharacterModel.Id), "Character does not exist.");
    }

    [Fact]
    public async Task ValidationFor_Name_WillFail_WhenItsEmpty()
    {
        _model.Name = string.Empty;

        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(EditCharacterModel.Name),
            "'Name' must not be empty."
        );
    }

    [Fact]
    public async Task ValidationFor_Name_WillFail_WhenItIsOver150Characters()
    {
        _model.Name = new string('x', 151);

        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(EditCharacterModel.Name),
            "The length of 'Name' must be 150 characters or fewer. You entered 151 characters."
        );
    }

    [Fact]
    public async Task ValidationFor_PrimaryProgressionId_WillFail_WhenProgressionPathDoesNotExist()
    {
        A.CallTo(() =>
                _progressionPathRepository.ProgressionPathExists(_model.PrimaryProgressionId!.Value)
            )
            .Returns(false);

        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(EditCharacterModel.PrimaryProgressionId),
            "The primary progression does not exist."
        );
    }

    [Fact]
    public async Task ValidationFor_SecondaryProgressionId_WillFail_WhenProgressionPathDoesNotExist()
    {
        A.CallTo(() =>
                _progressionPathRepository.ProgressionPathExists(
                    _model.SecondaryProgressionId!.Value
                )
            )
            .Returns(false);

        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(EditCharacterModel.SecondaryProgressionId),
            "The secondary progression does not exist."
        );
    }

    [Fact]
    public async Task ValidationFor_IsPrimaryCharacter_WillFail_WhenPrimaryCharacterAlreadyExists()
    {
        _model.IsPrimaryCharacter = true;

        A.CallTo(() => _characterRepository.CanUpdatePrimaryCharacterStatus(_model.Id))
            .Returns(false);

        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(EditCharacterModel.IsPrimaryCharacter),
            "A primary character already exists."
        );
    }

    [Fact]
    public async Task ValidationFor_IsPrimaryCharacter_WillSkip_WhenItIsNotPrimaryCharacter()
    {
        _model.IsPrimaryCharacter = false;

        await _useCase.ExecuteAsync(_model);

        A.CallTo(() => _characterRepository.CanUpdatePrimaryCharacterStatus(_model.Id))
            .MustNotHaveHappened();
    }

    [Fact]
    public async Task UseCase_WillEditBasicFields_AndPassThroughDbObject()
    {
        await _useCase.ExecuteAsync(_model);

        A.CallTo(() => _characterRepository.EditAsync(A<Character>.That.IsSameAs(_dbModel)))
            .MustHaveHappenedOnceExactly();

        Assert.Equal(_model.Name, _dbModel.Name);
        Assert.Equal(_model.Background, _dbModel.Background);
        Assert.Equal(_model.IsPrimaryCharacter, _dbModel.IsPrimaryCharacter);
    }

    [Theory]
    [InlineData(3)] // Adepts
    [InlineData(4)] // Shammas
    [InlineData(9)] // Vampyres
    public async Task UseCase_PassesThrough_OnlyPrimaryProgression_ForNonSorcererAllowedExpressions(
        int expressionId
    )
    {
        _dbModel.ExpressionId = expressionId;

        _dbModel.PrimaryProgressionId = null;
        _dbModel.SecondaryProgressionId = null;

        await _useCase.ExecuteAsync(_model);

        Assert.Equal(_model.PrimaryProgressionId, _dbModel.PrimaryProgressionId);
        Assert.Null(_dbModel.SecondaryProgressionId);
    }

    [Fact]
    public async Task UseCase_PassesThrough_BothPrimaryAndSecondaryProgression_ForSorcerer()
    {
        _dbModel.ExpressionId = 8; // Sorcerer

        _dbModel.PrimaryProgressionId = null;
        _dbModel.SecondaryProgressionId = null;

        await _useCase.ExecuteAsync(_model);

        Assert.Equal(_model.PrimaryProgressionId, _dbModel.PrimaryProgressionId);
        Assert.Equal(_model.SecondaryProgressionId, _dbModel.SecondaryProgressionId);
    }

    [Fact]
    public async Task UseCase_DoesNotPassThrough_AnyProgressions_ForExpressionsThatDoNotSupportProgressions()
    {
        _dbModel.ExpressionId = 2; // not in {3,4,8,9}

        _dbModel.PrimaryProgressionId = null;
        _dbModel.SecondaryProgressionId = null;

        await _useCase.ExecuteAsync(_model);

        Assert.Null(_dbModel.PrimaryProgressionId);
        Assert.Null(_dbModel.SecondaryProgressionId);
    }
}

using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.UseCases.Characters.EditCharacterGoFields;
using ExpressedRealms.DB.Models.Characters;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Characters.UseCases.Tests.Unit.Characters;

public class EditCharacterGoFieldsUseCaseTests
{
    private readonly EditCharacterGoFieldsUseCase _useCase;
    private readonly ICharacterRepository _characterRepository;
    private readonly EditCharacterGoFieldsModel _model;
    private readonly Character _dbModel;

    public EditCharacterGoFieldsUseCaseTests()
    {
        _model = new EditCharacterGoFieldsModel
        {
            Id = 10,
            WealthLevel = 1,
            PrimaFragments = 1,
            Motes = 1,
            VoidFragments = 1,
        };

        _dbModel = new Character
        {
            Id = _model.Id,
            WealthLevel = 0,
            PrimaFragments = 0,
            Motes = 0,
            VoidFragments = 0,
        };

        _characterRepository = A.Fake<ICharacterRepository>();
        A.CallTo(() => _characterRepository.FindCharacterAsync(_model.Id)).Returns(_dbModel);

        var validator = new EditCharacterGoFieldsModelValidator(_characterRepository);

        _useCase = new EditCharacterGoFieldsUseCase(
            _characterRepository,
            validator,
            CancellationToken.None
        );
    }

    [Fact]
    public async Task ValidationFor_Id_WillFail_WhenItsEmpty()
    {
        _model.Id = 0;
        A.CallTo(() => _characterRepository.FindCharacterAsync(_model.Id)).Returns(_dbModel);

        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(EditCharacterGoFieldsModel.Id),
            "'Id' must not be empty."
        );
    }

    [Fact]
    public async Task ValidationFor_Id_WillFail_WhenItsNegative()
    {
        _model.Id = -1;
        A.CallTo(() => _characterRepository.FindCharacterAsync(_model.Id)).Returns(_dbModel);

        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(EditCharacterGoFieldsModel.Id),
            "'Id' must be greater than '0'."
        );
    }

    [Fact]
    public async Task ValidationFor_Id_WillFail_WhenItDoesNotExist()
    {
        A.CallTo(() => _characterRepository.FindCharacterAsync(_model.Id))
            .Returns(Task.FromResult<Character?>(null));

        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(EditCharacterGoFieldsModel.Id),
            "Character does not exist."
        );
    }

    [Fact]
    public async Task ValidationFor_WealthLevel_WillFail_WhenItIsNegative()
    {
        _model.WealthLevel = -1;

        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(EditCharacterGoFieldsModel.WealthLevel),
            "Prima Level must be at least 0."
        );
    }

    [Fact]
    public async Task ValidationFor_PrimaFragments_WillFail_WhenItIsOutsideTheInclusiveRange()
    {
        _model.PrimaFragments = 6;

        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(EditCharacterGoFieldsModel.PrimaFragments),
            "Prima fragments must be at least 0."
        );
    }

    [Theory]
    [InlineData(-8)]
    [InlineData(8)]
    public async Task ValidationFor_Motes_WillFail_WhenItIsOutsideTheInclusiveRange(int motes)
    {
        _model.Motes = motes;

        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(EditCharacterGoFieldsModel.Motes),
            "Prima motes must be between -7 and 7."
        );
    }

    [Fact]
    public async Task ValidationFor_VoidFragments_WillFail_WhenItIsNegative()
    {
        _model.VoidFragments = -1;

        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(EditCharacterGoFieldsModel.VoidFragments),
            "Void fragments must be at least 0."
        );
    }

    [Fact]
    public async Task UseCase_WillEditGoFields_AndPassThroughDbObject()
    {
        await _useCase.ExecuteAsync(_model);

        A.CallTo(() => _characterRepository.EditAsync(A<Character>.That.IsSameAs(_dbModel)))
            .MustHaveHappenedOnceExactly();

        Assert.Equal(_model.WealthLevel, _dbModel.WealthLevel);
        Assert.Equal(_model.PrimaFragments, _dbModel.PrimaFragments);
        Assert.Equal(_model.Motes, _dbModel.Motes);
        Assert.Equal(_model.VoidFragments, _dbModel.VoidFragments);
    }
}

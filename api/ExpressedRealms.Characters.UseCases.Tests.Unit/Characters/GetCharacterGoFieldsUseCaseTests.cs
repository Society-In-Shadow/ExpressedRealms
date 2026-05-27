using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.UseCases.Characters.GetCharacterGoFields;
using ExpressedRealms.DB.Models.Characters;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Characters.UseCases.Tests.Unit.Characters;

public class GetCharacterGoFieldsUseCaseTests
{
    private readonly GetCharacterGoFieldsUseCase _useCase;
    private readonly ICharacterRepository _characterRepository;
    private readonly GetCharacterGoFieldsModel _model;
    private readonly Character _dbModel;

    public GetCharacterGoFieldsUseCaseTests()
    {
        _model = new GetCharacterGoFieldsModel
        {
            Id = 10,
        };

        _dbModel = new Character
        {
            Id = _model.Id,
            WealthLevel = 1,
            PrimaFragments = 2,
            Motes = 3,
            VoidFragments = 4,
        };

        _characterRepository = A.Fake<ICharacterRepository>();
        A.CallTo(() => _characterRepository.FindCharacterAsync(_model.Id)).Returns(_dbModel);

        var validator = new GetCharacterGoFieldsModelValidator(_characterRepository);

        _useCase = new GetCharacterGoFieldsUseCase(
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
            nameof(GetCharacterGoFieldsModel.Id),
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
            nameof(GetCharacterGoFieldsModel.Id),
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
            nameof(GetCharacterGoFieldsModel.Id),
            "Character does not exist."
        );
    }

    [Fact]
    public async Task UseCase_WillEditGoFields_AndPassThroughDbObject()
    {
        var result = await _useCase.ExecuteAsync(_model);
        
        Assert.Equal(result.Value.WealthLevel, _dbModel.WealthLevel);
        Assert.Equal(result.Value.PrimaFragments, _dbModel.PrimaFragments);
        Assert.Equal(result.Value.Motes, _dbModel.Motes);
        Assert.Equal(result.Value.VoidFragments, _dbModel.VoidFragments);
    }
}

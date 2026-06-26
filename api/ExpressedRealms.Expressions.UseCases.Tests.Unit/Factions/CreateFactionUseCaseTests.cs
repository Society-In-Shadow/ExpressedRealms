using ExpressedRealms.DB.Models.Factions.FactionModels;
using ExpressedRealms.Expressions.Repository.Expressions;
using ExpressedRealms.Expressions.Repository.Factions;
using ExpressedRealms.Expressions.UseCases.FactionUseCases.CreateFaction;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Expressions.UseCases.Tests.Unit.Factions;

public class CreateFactionUseCaseTests
{
    private readonly CreateFactionUseCase _useCase;
    private readonly IFactionRepository _factionRepository;
    private readonly IExpressionRepository _expressionRepository;
    private readonly CreateFactionModel _model;

    public CreateFactionUseCaseTests()
    {
        _model = new CreateFactionModel()
        {
            Name = "parse",
            Background = "View",
            ExpressionId = 1,
        };

        _factionRepository = A.Fake<IFactionRepository>();
        _expressionRepository = A.Fake<IExpressionRepository>();

        A.CallTo(() => _expressionRepository.ExpressionIsExpressionType(_model.ExpressionId))
            .Returns(true);
        A.CallTo(() => _factionRepository.HasDuplicateName(_model.Name)).Returns(false);

        var validator = new CreateFactionModelValidator();

        _useCase = new CreateFactionUseCase(
            _factionRepository,
            _expressionRepository,
            validator,
            CancellationToken.None
        );
    }

    [Fact]
    public async Task ValidationFor_Name_WillFail_WhenName_IsEmpty()
    {
        _model.Name = string.Empty;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(nameof(CreateFactionModel.Name), "Name is required.");
    }

    [Fact]
    public async Task ValidationFor_Name_WillFail_WhenName_IsOver250Characters()
    {
        _model.Name = new string('x', 251);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateFactionModel.Name),
            "Name cannot exceed 250 characters."
        );
    }

    [Fact]
    public async Task ValidationFor_Name_WillFail_WhenName_IsADuplicateName()
    {
        A.CallTo(() => _factionRepository.HasDuplicateName(_model.Name)).Returns(true);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateFactionModel.Name),
            "This name already exists."
        );
    }

    [Fact]
    public async Task ValidationFor_Background_WillFail_WhenBackground_IsEmpty()
    {
        _model.Background = string.Empty;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateFactionModel.Background),
            "Background is required."
        );
    }

    [Fact]
    public async Task ValidationFor_Background_WillFail_WhenBackground_IsOver20000Characters()
    {
        _model.Background = new string('x', 20001);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateFactionModel.Background),
            "Background cannot exceed 20000 characters."
        );
    }

    [Fact]
    public async Task ValidationFor_ExpressionId_WillFail_WhenItIsEmpty()
    {
        _model.ExpressionId = 0;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateFactionModel.ExpressionId),
            "Expression Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_ExpressionId_WillFail_WhenExpressionId_IsNotAnExpression()
    {
        A.CallTo(() => _expressionRepository.ExpressionIsExpressionType(_model.ExpressionId))
            .Returns(false);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateFactionModel.ExpressionId),
            "Expression Id is not of an expression type."
        );
    }

    [Fact]
    public async Task UseCase_WillCreateTheFaction()
    {
        var faction = new Faction()
        {
            Name = _model.Name,
            Background = _model.Background,
            ExpressionId = _model.ExpressionId,
        };

        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _factionRepository.CreateFactionAsync(
                    A<Faction>.That.Matches(k =>
                        k.Name == faction.Name
                        && k.Background == faction.Background
                        && k.ExpressionId == faction.ExpressionId
                    )
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_WillReturn_FactionId_IfSuccessful()
    {
        A.CallTo(() => _factionRepository.CreateFactionAsync(A<Faction>._)).Returns(5);

        var result = await _useCase.ExecuteAsync(_model);
        Assert.Equal(5, result.Value);
    }
}

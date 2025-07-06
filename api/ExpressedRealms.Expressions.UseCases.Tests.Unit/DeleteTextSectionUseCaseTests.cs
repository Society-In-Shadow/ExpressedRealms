using ExpressedRealms.DB.Models.Expressions.ExpressionSectionSetup;
using ExpressedRealms.DB.Models.Expressions.ExpressionSetup;
using ExpressedRealms.Expressions.Repository.Expressions;
using ExpressedRealms.Expressions.Repository.ExpressionTextSections;
using ExpressedRealms.Expressions.UseCases.ExpressionTextSections.DeleteTextSection;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Expressions.UseCases.Tests.Unit;

public class DeleteTextSectionUseCaseTests
{
    private readonly DeleteTextSectionUseCase _useCase;
    private readonly IExpressionTextSectionRepository _textRepository;
    private readonly IExpressionRepository _expressionRepository;
    private readonly DeleteTextSectionModel _model;
    
    public DeleteTextSectionUseCaseTests()
    {
        _model = new DeleteTextSectionModel() { Id = 3, ExpressionId = 5};

        _textRepository = A.Fake<IExpressionTextSectionRepository>();
        _expressionRepository = A.Fake<IExpressionRepository>();

        A.CallTo(() => _expressionRepository.GetExpressionForDeletion(_model.ExpressionId)).Returns(new Expression()
        {
            Id = _model.ExpressionId,
        });
        
        A.CallTo(() => _textRepository.GetExpressionSectionForDeletion(_model.Id)).Returns(new ExpressionSection()
        {
            Id = _model.Id,
        });

        var validator = new DeleteTextSectionModelValidator(_textRepository, _expressionRepository);

        _useCase = new DeleteTextSectionUseCase(_textRepository, validator, CancellationToken.None);
    }

    [Fact]
    public async Task ValidationFor_ExpressionId_WillFail_IfExpressionIdDoesNotExist()
    {
        A.CallTo(() => _expressionRepository.GetExpressionForDeletion(A<int>.Ignored)).Returns(Task.FromResult<Expression?>(null));

        var results = await _useCase.ExecuteAsync(_model);
        results.HasValidationError(
            nameof(DeleteTextSectionModel.ExpressionId),
            "This is not a valid expression."
        );
    }

    [Fact]
    public async Task ValidationFor_ExpressionId_WillFail_IfExpressionIdIsEmpty()
    {
        _model.ExpressionId = 0;

        var results = await _useCase.ExecuteAsync(_model);
        results.HasValidationError(nameof(DeleteTextSectionModel.ExpressionId), "ExpressionId is required.");
    }
}
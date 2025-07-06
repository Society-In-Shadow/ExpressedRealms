using ExpressedRealms.DB.Models.Expressions;
using ExpressedRealms.DB.Models.Expressions.ExpressionSectionSetup;
using ExpressedRealms.DB.Models.Expressions.ExpressionSetup;
using ExpressedRealms.Expressions.Repository.Expressions;
using ExpressedRealms.Expressions.Repository.ExpressionTextSections;
using ExpressedRealms.Expressions.UseCases.ExpressionTextSections.DeleteTextSection;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using ExpressedRealms.UseCases.Shared.CommonFailureTypes;
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
        _model = new DeleteTextSectionModel() { Id = 3, ExpressionId = 5 };

        _textRepository = A.Fake<IExpressionTextSectionRepository>();
        _expressionRepository = A.Fake<IExpressionRepository>();

        A.CallTo(() => _expressionRepository.GetExpressionForDeletion(_model.ExpressionId))
            .Returns(new Expression() { Id = _model.ExpressionId });

        A.CallTo(() =>
                _textRepository.GetExpressionSectionForDeletion(_model.ExpressionId, _model.Id)
            )
            .Returns(
                new ExpressionSection()
                {
                    Id = _model.Id,
                    SectionType = new ExpressionSectionType() { Name = "Section" },
                }
            );

        var validator = new DeleteTextSectionModelValidator(_textRepository, _expressionRepository);

        _useCase = new DeleteTextSectionUseCase(_textRepository, validator, CancellationToken.None);
    }

    [Fact]
    public async Task ValidationFor_ExpressionId_WillFail_IfExpressionIdDoesNotExist()
    {
        A.CallTo(() => _expressionRepository.GetExpressionForDeletion(A<int>.Ignored))
            .Returns(Task.FromResult<Expression?>(null));

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError<NotFoundFailure>(
            nameof(DeleteTextSectionModel.ExpressionId),
            "This is not a valid expression."
        );
    }

    [Fact]
    public async Task ValidationFor_ExpressionId_WillFail_IfExpressionHasBeenDeleted()
    {
        A.CallTo(() => _expressionRepository.GetExpressionForDeletion(A<int>.Ignored))
            .Returns(new Expression() { IsDeleted = true });

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError<AlreadyDeletedFailure>(
            nameof(DeleteTextSectionModel.ExpressionId),
            "This expression has been deleted."
        );
    }

    [Fact]
    public async Task ValidationFor_ExpressionId_WillFail_IfIsEmpty()
    {
        _model.ExpressionId = 0;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(DeleteTextSectionModel.ExpressionId),
            "ExpressionId is required."
        );
    }

    [Fact]
    public async Task ValidationFor_Id_WillFail_IfExpressionSectionDoesNotExist()
    {
        A.CallTo(() =>
                _textRepository.GetExpressionSectionForDeletion(_model.ExpressionId, _model.Id)
            )
            .Returns(Task.FromResult<ExpressionSection?>(null));

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError<NotFoundFailure>(
            nameof(DeleteTextSectionModel.Id),
            "This is not a valid expression section."
        );
    }

    [Fact]
    public async Task ValidationFor_Id_WillFail_IfExpressionHasBeenDeleted()
    {
        A.CallTo(() =>
                _textRepository.GetExpressionSectionForDeletion(_model.ExpressionId, _model.Id)
            )
            .Returns(new ExpressionSection() { IsDeleted = true });

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError<AlreadyDeletedFailure>(
            nameof(DeleteTextSectionModel.Id),
            "This expression section has been deleted."
        );
    }

    [Fact]
    public async Task ValidationFor_Id_WillFail_IfIsEmpty()
    {
        _model.Id = 0;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(nameof(DeleteTextSectionModel.Id), "Id is required.");
    }

    [Fact]
    public async Task UseCase_WillGrab_TheCorrespondingExpressionSection()
    {
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _textRepository.GetExpressionSectionForDeletion(_model.ExpressionId, _model.Id)
            )
            .MustHaveHappened(1, Times.OrMore);
    }

    [Fact]
    public async Task UseCase_WillFail_IfItsTheKnowledgeSection_FromTheRulebook()
    {
        A.CallTo(() =>
                _textRepository.GetExpressionSectionForDeletion(_model.ExpressionId, _model.Id)
            )
            .Returns(
                new ExpressionSection()
                {
                    Id = 1,
                    SectionType = new ExpressionSectionType() { Name = "Knowledges Section" },
                }
            );

        var result = await _useCase.ExecuteAsync(_model);

        Assert.False(result.IsSuccess);
        Assert.Equal(
            "You cannot delete the systems knowledge section.",
            result.Errors.First().Message
        );
    }

    [Fact]
    public async Task UseCase_WillDelete_TheExpression()
    {
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _textRepository.DeleteExpressionTextSectionAsync(_model.ExpressionId, _model.Id)
            )
            .MustHaveHappenedOnceExactly();
    }
}

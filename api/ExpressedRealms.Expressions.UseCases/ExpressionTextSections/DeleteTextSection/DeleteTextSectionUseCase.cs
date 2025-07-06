using ExpressedRealms.Expressions.Repository.ExpressionTextSections;
using ExpressedRealms.UseCases.Shared;
using FluentResults;
using JetBrains.Annotations;

namespace ExpressedRealms.Expressions.UseCases.ExpressionTextSections.DeleteTextSection;

[UsedImplicitly]
internal sealed class DeleteTextSectionUseCase(
    IExpressionTextSectionRepository repository,
    DeleteTextSectionModelValidator validator,
    CancellationToken cancellationToken
) : IDeleteTextSectionUseCase
{
    public async Task<Result> ExecuteAsync(DeleteTextSectionModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var expressionSection = await repository.GetExpressionSectionForDeletion(
            model.ExpressionId,
            model.Id
        );

        if (expressionSection!.SectionType.Name == "Knowledges Section")
        {
            return Result.Fail("You cannot delete the systems knowledge section.");
        }

        await repository.DeleteExpressionTextSectionAsync(model.ExpressionId, model.Id);

        return Result.Ok();
    }
}

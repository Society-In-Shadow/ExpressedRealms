using ExpressedRealms.Expressions.Repository.ExpressionTextSections;
using ExpressedRealms.Expressions.UseCases.ExpressionTextSections.DeleteTextSection;
using ExpressedRealms.UseCases.Shared;
using FluentResults;
using JetBrains.Annotations;

namespace ExpressedRealms.Expressions.UseCases.Expressions.DeleteTextSection;

[UsedImplicitly]
internal sealed class GetNavigationMenuUseCase(
    IExpressionTextSectionRepository repository,
    GetNavigationMenuModelValidator validator,
    CancellationToken cancellationToken
) : IGetNavigationMenuUseCase
{
    public async Task<Result> ExecuteAsync(GetNavigationMenuModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        /*var expressionSection = await repository.GetExpressionSectionForDeletion(
            model.ExpressionId,
            model.Id
        );*/

        /*if (expressionSection!.SectionType.Name == "Knowledges Section")
        {
            return Result.Fail("You cannot delete the systems knowledge section.");
        }

        if (expressionSection!.SectionType.Name == "Blessings Section")
        {
            return Result.Fail("You cannot delete the systems blessings section.");
        }*/

        //await repository.DeleteExpressionTextSectionAsync(model.ExpressionId, model.Id);

        return Result.Ok();
    }
}

using ExpressedRealms.Expressions.Repository.Expressions;
using ExpressedRealms.Expressions.Repository.ExpressionTextSections;
using ExpressedRealms.Expressions.Repository.ExpressionTextSections.DTOs;
using ExpressedRealms.UseCases.Shared;
using FluentResults;
using JetBrains.Annotations;

namespace ExpressedRealms.Expressions.UseCases.ExpressionTextSections.DeleteTextSection;

[UsedImplicitly]
internal sealed class DeleteTextSectionUseCase(
    IExpressionTextSectionRepository repository,
    IExpressionRepository expressionRepository,
    DeleteTextSectionModelValidator validator,
    TimeProvider dateTime,
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

        var authResult = await expressionRepository.CheckUserPermissionsForExpressionDelete(
            model.ExpressionId
        );
        if (authResult.IsFailed)
            return authResult.ToResult();

        var expressionSection = await repository.GetExpressionSectionForDeletion(
            model.ExpressionId,
            model.Id
        );

        if (expressionSection!.SectionType.Name == "Knowledges Section")
        {
            return Result.Fail("You cannot delete the systems knowledge section.");
        }

        if (expressionSection.SectionType.Name == "Blessings Section")
        {
            return Result.Fail("You cannot delete the systems blessings section.");
        }

        var allSections = await repository.GetAllSectionsForExpression(model.ExpressionId);

        var queuedDescendents = GetDescendantsToDelete(model.Id, allSections);
        await repository.DeleteSections(queuedDescendents, dateTime.GetUtcNow());

        return Result.Ok();
    }

    private static List<int> GetDescendantsToDelete(
        int targetParent,
        List<CommonDeleteHierarchyDto> items
    )
    {
        var deletedDescendent = items.First(x => x.Id == targetParent);
        var lookupTable = items.Where(x => x.ParentId.HasValue).ToLookup(x => x.ParentId!.Value);
        var queuedDescendents = GetDescendantsToDelete(deletedDescendent, lookupTable);
        queuedDescendents.Add(deletedDescendent.Id);

        return queuedDescendents;
    }

    private static List<int> GetDescendantsToDelete(
        CommonDeleteHierarchyDto parent,
        ILookup<int, CommonDeleteHierarchyDto> childrenByParentId
    )
    {
        var idsToDelete = new List<int>();

        foreach (var child in childrenByParentId[parent.Id])
        {
            if (!child.IsDeleted)
            {
                idsToDelete.Add(child.Id);
            }

            idsToDelete.AddRange(GetDescendantsToDelete(child, childrenByParentId));
        }

        return idsToDelete;
    }
}

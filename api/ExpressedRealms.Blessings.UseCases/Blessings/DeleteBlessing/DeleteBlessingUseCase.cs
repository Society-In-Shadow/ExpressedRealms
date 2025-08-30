using ExpressedRealms.Blessings.Repository.Blessings;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.Knowledges.UseCases.Knowledges.DeleteKnowledge;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Blessings.UseCases.Blessings.DeleteBlessing;

internal sealed class DeleteBlessingUseCase(
    IBlessingRepository knowledgeRepository,
    DeleteBlessingModelValidator validator,
    CancellationToken cancellationToken
) : IDeleteBlessingUseCase
{
    public async Task<Result> ExecuteAsync(DeleteBlessingModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var knowledge = await knowledgeRepository.GetBlessingForEditing(model.Id);

        knowledge.SoftDelete();

        await knowledgeRepository.EditBlessingAsync(knowledge);

        return Result.Ok();
    }
}

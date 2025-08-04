using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.Knowledges.Repository.Knowledges;
using ExpressedRealms.Knowledges.Repository.KnowledgeSpecializations;
using ExpressedRealms.Knowledges.UseCases.Knowledges.DeleteKnowledge;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Knowledges.UseCases.KnowledgeSpecializations.DeleteSpecialization;

internal sealed class DeleteSpecializationUseCase(
    IKnowledgeSpecializationRepository knowledgeRepository,
    DeleteSpecializationModelValidator validator,
    CancellationToken cancellationToken
) : IDeleteSpecializationUseCase
{
    public async Task<Result> ExecuteAsync(DeleteSpecializationModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var knowledge = await knowledgeRepository.GetSpecialization(model.Id);

        knowledge.SoftDelete();

        await knowledgeRepository.UpdateSpecialization(knowledge);

        return Result.Ok();
    }
}

using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.Expressions.Repository.CharacterFactions;
using ExpressedRealms.Knowledges.Repository.KnowledgeSpecializations;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Knowledges.UseCases.KnowledgeSpecializations.DeleteSpecialization;

internal sealed class DeleteSpecializationUseCase(
    IKnowledgeSpecializationRepository knowledgeRepository,
    ICharacterFactionRepository characterFactionRepository,
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
        var factionKnowledge = await characterFactionRepository.GetLatestPlayerFactionLevels(model.CharacterId);

        if (factionKnowledge.Any(x => x.KnowledgeSpecialization == knowledge.Name))
        {
            return ValidationHelper.AddSingleValidationFailure(nameof(model.Id),
                "Your faction level prevents you from removing this knowledge specialization");
        }

        knowledge.SoftDelete();

        await knowledgeRepository.UpdateSpecialization(knowledge);

        return Result.Ok();
    }
}

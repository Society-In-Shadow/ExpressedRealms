using ExpressedRealms.Expressions.Repository.CharacterFactions;
using ExpressedRealms.Knowledges.Repository.KnowledgeSpecializations;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Knowledges.UseCases.KnowledgeSpecializations.EditSpecialization;

internal sealed class EditSpecializationUseCase(
    IKnowledgeSpecializationRepository specializationRepository,
    ICharacterFactionRepository characterFactionRepository,
    EditSpecializationModelValidator validator,
    CancellationToken cancellationToken
) : IEditSpecializationUseCase
{
    public async Task<Result> ExecuteAsync(EditSpecializationModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var specialization = await specializationRepository.GetSpecialization(model.Id);
        var factionKnowledge = await characterFactionRepository.GetLatestPlayerFactionLevels(model.CharacterId);

        if (factionKnowledge.Any(x => x.KnowledgeSpecialization == specialization.Name && specialization.Name != model.Name))
        {
            return ValidationHelper.AddSingleValidationFailure(nameof(model.Name),
                "Your faction level prevents you from renaming this knowledge specialization");
        }

        specialization.Name = model.Name;
        specialization.Description = model.Description;
        specialization.Notes = model.Notes?.Trim() == string.Empty ? null : model.Notes?.Trim();

        await specializationRepository.UpdateSpecialization(specialization);

        return Result.Ok();
    }
}

using ExpressedRealms.DB.Models.Knowledges.CharacterKnowledgeSpecializations;
using ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings;
using ExpressedRealms.Knowledges.Repository.KnowledgeSpecializations;
using ExpressedRealms.Knowledges.UseCases.KnowledgeSpecializations.CreateSpecialization;
using ExpressedRealms.UseCases.Shared;
using ExpressedRealms.UseCases.Shared.CommonFailureTypes;
using FluentResults;

namespace ExpressedRealms.Knowledges.UseCases.KnowledgeSpecializations.EditSpecialization;

internal sealed class EditSpecializationUseCase(
    IKnowledgeSpecializationRepository specializationRepository,
    ICharacterKnowledgeRepository mappingRepository,
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
        
        var mapping = await mappingRepository.GetCharacterKnowledgeMappingForEditing(
            specialization.KnowledgeMappingId
        );

        const int maxKnowledge = 7;

        var spentXp = await mappingRepository.GetExperienceSpentOnKnowledgesForCharacter(
            mapping.CharacterId
        );

        const int newSpecializationCost = 2;
        if (maxKnowledge - spentXp - newSpecializationCost < 0)
        {
            return Result.Fail(
                new NotEnoughXPFailure(maxKnowledge - spentXp, newSpecializationCost)
            );
        }

        var counts = await mappingRepository.GetSpecializationCountForMapping(
            specialization.KnowledgeMappingId
        );

        if (counts.MaxCount <= counts.CurrentCount + 1)
        {
            return Result.Fail(
                "You have reached the maximum number of specializations allowed for this knowledge."
            );
        }
        
        specialization.Name = model.Name;
        specialization.Description = model.Description;
        specialization.Notes = model.Notes?.Trim() == string.Empty ? null : model.Notes?.Trim();
        
        await specializationRepository.UpdateSpecialization(specialization);
        
        return Result.Ok();
    }
}

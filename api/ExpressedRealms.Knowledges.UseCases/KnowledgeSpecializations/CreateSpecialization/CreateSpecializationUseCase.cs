using ExpressedRealms.Characters.Repository.Xp;
using ExpressedRealms.DB.Characters.xpTables;
using ExpressedRealms.DB.Models.Knowledges.CharacterKnowledgeSpecializations;
using ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings;
using ExpressedRealms.Knowledges.Repository.KnowledgeSpecializations;
using ExpressedRealms.UseCases.Shared;
using ExpressedRealms.UseCases.Shared.CommonFailureTypes;
using FluentResults;

namespace ExpressedRealms.Knowledges.UseCases.KnowledgeSpecializations.CreateSpecialization;

internal sealed class CreateSpecializationUseCase(
    IKnowledgeSpecializationRepository specializationRepository,
    ICharacterKnowledgeRepository mappingRepository,
    IXpRepository xpRepository,
    CreateSpecializationModelValidator validator,
    CancellationToken cancellationToken
) : ICreateSpecializationUseCase
{
    public async Task<Result<int>> ExecuteAsync(CreateSpecializationModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var mapping = await mappingRepository.GetCharacterKnowledgeMappingForEditing(
            model.KnowledgeMappingId
        );

        var xpInfo = await xpRepository.GetAvailableXpForSection(mapping.CharacterId, XpSectionTypeEnum.Knowledge);

        const int newSpecializationCost = 2;
        if (xpInfo.SpentXp + newSpecializationCost > xpInfo.AvailableXp)
        {
            return Result.Fail(
                new NotEnoughXPFailure(xpInfo.AvailableXp - xpInfo.SpentXp, newSpecializationCost)
            );
        }

        var counts = await mappingRepository.GetSpecializationCountForMapping(
            model.KnowledgeMappingId
        );

        if (counts.MaxCount < counts.CurrentCount + 1)
        {
            return Result.Fail(
                "You have reached the maximum number of specializations allowed for this knowledge."
            );
        }

        var knowledgeId = await specializationRepository.CreateSpecialization(
            new CharacterKnowledgeSpecialization()
            {
                Name = model.Name,
                Description = model.Description,
                Notes = model.Notes?.Trim() == string.Empty ? null : model.Notes?.Trim(),
                KnowledgeMappingId = model.KnowledgeMappingId,
            }
        );

        return Result.Ok(knowledgeId);
    }
}

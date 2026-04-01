using ExpressedRealms.Knowledges.Repository.KnowledgeSpecializations;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Knowledges.UseCases.KnowledgeSpecializations.EditSpecialization;

internal sealed class EditSpecializationUseCase(
    IKnowledgeSpecializationRepository specializationRepository,
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

        specialization.Name = model.Name;
        specialization.Description = model.Description;
        specialization.Notes = model.Notes?.Trim() == string.Empty ? null : model.Notes?.Trim();

        await specializationRepository.UpdateSpecialization(specialization);

        return Result.Ok();
    }
}

using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.Repository.Xp;
using ExpressedRealms.DB.Models.Characters.AssignedXP.AssignedXpMappingModels;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Characters.UseCases.AssignedXp.Delete;

[UsedImplicitly]
internal sealed class DeleteAssignedXpMappingModelValidator
    : AbstractValidator<DeleteAssignedXpMappingModel>
{
    public DeleteAssignedXpMappingModelValidator(
        ICharacterRepository characterRepository,
        IAssignedXpMappingRepository assignedXpMappingRepository
    )
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .MustAsync(
                async (x, y) =>
                    await assignedXpMappingRepository.FindAsync<AssignedXpMapping>(x) is not null
            )
            .WithMessage("The Id does not exist.");

        RuleFor(x => x.CharacterId)
            .NotEmpty()
            .WithMessage("Character Id is required.")
            .MustAsync(async (x, y) => await characterRepository.FindCharacterAsync(x) is not null)
            .WithMessage("The Character Id does not exist.");
    }
}

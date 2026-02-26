using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.Repository.Players;
using ExpressedRealms.Characters.Repository.Xp;
using ExpressedRealms.DB.Models.Characters.AssignedXP.AssignedXpTypeModels;
using ExpressedRealms.Events.API.Repositories.Events;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Characters.UseCases.AssignedXp.Create;

[UsedImplicitly]
internal sealed class CreateAssignedXpMappingModelValidator
    : AbstractValidator<CreateAssignedXpMappingModel>
{
    public CreateAssignedXpMappingModelValidator(
        ICharacterRepository characterRepository,
        IEventRepository eventRepository,
        IPlayerRepository playerRepository,
        IAssignedXpMappingRepository assignedXpMappingRepository
    )
    {
        RuleFor(x => x.CharacterId)
            .NotEmpty()
            .WithMessage("Character Id is required.")
            .MustAsync(async (x, y) => await characterRepository.FindCharacterAsync(x) is not null)
            .WithMessage("The Character does not exist.");

        RuleFor(x => x.EventId)
            .NotEmpty()
            .WithMessage("Event Id is required.")
            .MustAsync(async (x, y) => await eventRepository.FindEventAsync(x) is not null)
            .WithMessage("The Event Id does not exist.");

        RuleFor(x => x.AssignedXpTypeId)
            .NotEmpty()
            .WithMessage("Assigned Xp Type Id is required.")
            .MustAsync(
                async (x, y) =>
                    await assignedXpMappingRepository.FindAsync<AssignedXpType>(x) is not null
            )
            .WithMessage("The Assigned Xp Type Id does not exist.");

        RuleFor(x => x.Amount)
            .NotEmpty()
            .WithMessage("Amount is required.")
            .GreaterThan(0)
            .WithMessage("Amount must be greater than 0.");

        RuleFor(x => x.Reason)
            .MaximumLength(1500)
            .When(x => !string.IsNullOrWhiteSpace(x.Reason))
            .WithMessage("Reason must be between 1 and 1500 characters.");
    }
}

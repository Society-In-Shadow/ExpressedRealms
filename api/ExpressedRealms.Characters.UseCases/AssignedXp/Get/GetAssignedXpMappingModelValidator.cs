using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.Repository.Xp;
using ExpressedRealms.Events.API.Repositories.Events;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Characters.UseCases.AssignedXp.Get;

[UsedImplicitly]
internal sealed class GetAssignedXpMappingModelValidator
    : AbstractValidator<GetAssignedXpMappingModel>
{
    public GetAssignedXpMappingModelValidator(
        ICharacterRepository characterRepository,
        IEventRepository eventRepository,
        IAssignedXpMappingRepository assignedXpMappingRepository
    )
    {
        RuleFor(x => x)
            .Must(x => (x.CharacterId != 0) ^ (x.EventId != 0))
            .WithMessage("Either Character Id or Event Id must be specified.");

        RuleFor(x => x.CharacterId)
            .NotEmpty()
            .WithMessage("Character Id is required.")
            .When(x => x.EventId != 0)
            .MustAsync(async (x, y) => await characterRepository.FindCharacterAsync(x) is not null)
            .WithMessage("The Character Id does not exist.")
            .When(x => x.EventId != 0);
        
        RuleFor(x => x.EventId)
            .NotEmpty()
            .WithMessage("Event Id is required.")
            .When(x => x.Id != 0)
            .MustAsync(async (x, y) => await eventRepository.FindEventAsync(x) is not null)
            .WithMessage("The Event Id does not exist.")
            .When(x => x.Id != 0);
    }
}

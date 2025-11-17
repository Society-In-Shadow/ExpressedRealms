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
            .WithName(nameof(GetAssignedXpMappingModel.CharacterId))
            .WithMessage("Either Character Id or Event Id must be specified.");

        RuleFor(x => x.CharacterId)
            .MustAsync(async (x, y) => await characterRepository.FindCharacterAsync(x) is not null)
            .WithMessage("The Character Id does not exist.")
            .When(x => x.CharacterId != 0);

        RuleFor(x => x.EventId)
            .MustAsync(async (x, y) => await eventRepository.FindEventAsync(x) is not null)
            .WithMessage("The Event Id does not exist.")
            .When(x => x.EventId != 0);
    }
}

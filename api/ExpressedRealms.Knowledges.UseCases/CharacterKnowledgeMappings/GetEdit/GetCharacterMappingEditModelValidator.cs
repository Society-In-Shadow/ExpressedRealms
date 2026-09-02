using ExpressedRealms.Knowledges.Repository.Knowledges;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Knowledges.UseCases.CharacterKnowledgeMappings.GetEdit;

[UsedImplicitly]
internal sealed class GetCharacterMappingEditModelValidator : AbstractValidator<GetCharacterMappingEditModel>
{
    public GetCharacterMappingEditModelValidator(IKnowledgeRepository repository)
    {
        RuleFor(x => x.MappingId)
            .NotEmpty()
            .WithMessage("Mapping Id is required.");
        
        RuleFor(x => x.CharacterId)
            .NotEmpty()
            .WithMessage("Character Id is required.");
    }
}

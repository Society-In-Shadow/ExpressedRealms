using ExpressedRealms.Characters.Repository.Players;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Admin.UseCases.GetPlayer;

[UsedImplicitly]
internal sealed class GetPlayerModelValidator : AbstractValidator<GetPlayerModel>
{
    public GetPlayerModelValidator(IPlayerRepository repository)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .MustAsync(async (x, y) => await repository.PlayerExistsAsync(x))
            .WithMessage("Character does not exist.");
    }
}

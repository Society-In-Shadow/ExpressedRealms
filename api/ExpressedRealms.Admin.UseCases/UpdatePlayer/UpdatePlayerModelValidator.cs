using ExpressedRealms.Characters.Repository.Players;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Admin.UseCases.UpdatePlayer;

[UsedImplicitly]
internal sealed class UpdatePlayerModelValidator : AbstractValidator<UpdatePlayerModel>
{
    public UpdatePlayerModelValidator(IPlayerRepository repository)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .MustAsync(async (x, y) => await repository.FindPlayerAsync(x) is not null)
            .WithMessage("Character does not exist.");

        RuleFor(x => x.PlayerNumber)
            .NotEmpty()
            .WithMessage("Player Number is required.")
            .GreaterThanOrEqualTo(0)
            .WithMessage("Player Number be greater than or equal to 0.")
            .MustAsync(async (x, y) => await repository.PlayerNumberExists(x))
            .WithMessage("Player Number Already Exists.");
    }
}

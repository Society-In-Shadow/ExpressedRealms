using ExpressedRealms.Admin.Repository;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Admin.UseCases.Users.UpdatePlayer;

[UsedImplicitly]
internal sealed class UpdatePlayerModelValidator : AbstractValidator<UpdatePlayerModel>
{
    public UpdatePlayerModelValidator(IUsersRepository repository)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .MustAsync(async (x, y) => await repository.UserExistsAsync(x.ToString()))
            .WithMessage("Character does not exist.");

        RuleFor(x => x.PlayerNumber)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Player Number be greater than or equal to 0.")
            .MustAsync(async (x, y) => !await repository.PlayerNumberExists(x))
            .WithMessage("Player Number Already Exists.")
            .MustAsync(async (x, y) => !await repository.PlayerNumberExceedsMaxSequenceValue(x))
            .WithMessage("Player Number is too high, you need to go lower");
    }
}

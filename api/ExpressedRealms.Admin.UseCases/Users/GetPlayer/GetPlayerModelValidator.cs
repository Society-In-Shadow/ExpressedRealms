using ExpressedRealms.Admin.Repository;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Admin.UseCases.Users.GetPlayer;

[UsedImplicitly]
internal sealed class GetPlayerModelValidator : AbstractValidator<GetPlayerModel>
{
    public GetPlayerModelValidator(IUsersRepository repository)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .MustAsync(async (x, y) => await repository.UserExistsAsync(x.ToString()))
            .WithMessage("User does not exist.");
    }
}

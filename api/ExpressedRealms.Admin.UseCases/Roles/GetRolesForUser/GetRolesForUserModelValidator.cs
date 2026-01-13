using ExpressedRealms.Admin.Repository;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Admin.UseCases.Roles.GetRolesForUser;

[UsedImplicitly]
internal sealed class GetRolesForUserModelValidator : AbstractValidator<GetRolesForUserModel>
{
    public GetRolesForUserModelValidator(IUsersRepository repository)
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User Id is required.")
            .MustAsync(async (x, y) => await repository.UserExistsAsync(x))
            .WithErrorCode("NotFound")
            .WithMessage("User does not exist.");
    }
}

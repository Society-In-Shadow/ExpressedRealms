using ExpressedRealms.Admin.Repository;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Admin.UseCases.Roles.AddUserToRole;

[UsedImplicitly]
internal sealed class AddUserToRoleModelValidator : AbstractValidator<AddUserToRoleModel>
{
    public AddUserToRoleModelValidator(
        IRolesRepository repository,
        IUsersRepository usersRepository
    )
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User Id is required.")
            .MustAsync(async (x, y) => await usersRepository.UserExistsAsync(x))
            .WithErrorCode("NotFound")
            .WithMessage("User does not exist.");

        RuleFor(x => x.RoleId)
            .NotEmpty()
            .WithMessage("Role Id is required.")
            .MustAsync(async (x, y) => await repository.RoleExistsAsync(x))
            .WithErrorCode("NotFound")
            .WithMessage("Role does not exist.");

        RuleFor(x => x.ExpireDate)
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Expiration date must be set to today or set in the future.")
            .When(x => x.ExpireDate is not null);
    }
}

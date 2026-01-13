using ExpressedRealms.Admin.Repository;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Admin.UseCases.Roles.DeleteUserRole;

[UsedImplicitly]
internal sealed class DeleteUserRoleModelValidator : AbstractValidator<DeleteUserRoleModel>
{
    public DeleteUserRoleModelValidator(
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
    }
}

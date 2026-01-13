using ExpressedRealms.Admin.Repository;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Admin.UseCases.Users.GetUsersForRole;

[UsedImplicitly]
internal sealed class GetUsersForRoleModelValidator : AbstractValidator<GetUsersForRoleModel>
{
    public GetUsersForRoleModelValidator(IRolesRepository repository)
    {
        RuleFor(x => x.RoleId)
            .NotEmpty()
            .WithMessage("Role Id is required.")
            .MustAsync(async (x, y) => await repository.RoleExistsAsync(x))
            .WithErrorCode("NotFound")
            .WithMessage("Role does not exist.");
    }
}

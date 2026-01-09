using ExpressedRealms.Admin.Repository;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Admin.UseCases.Roles.GetRole;

[UsedImplicitly]
internal sealed class GetRoleModelValidator : AbstractValidator<GetRoleModel>
{
    public GetRoleModelValidator(IRolesRepository repository)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .MustAsync(async (x, y) => await repository.RoleExistsAsync(x))
            .WithErrorCode("NotFound")
            .WithMessage("Role does not exist.");
    }
}

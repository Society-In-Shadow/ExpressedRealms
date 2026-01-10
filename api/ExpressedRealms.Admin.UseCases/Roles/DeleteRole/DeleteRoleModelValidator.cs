using ExpressedRealms.Admin.Repository;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Admin.UseCases.Roles.DeleteRole;

[UsedImplicitly]
internal sealed class DeleteRoleModelValidator : AbstractValidator<DeleteRoleModel>
{
    public DeleteRoleModelValidator(IRolesRepository repository)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .MustAsync(async (x, y) => await repository.RoleExistsAsync(x))
            .WithMessage("Role does not exist.")
            .WithErrorCode("NotFound");
    }
}

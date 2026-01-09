using ExpressedRealms.Admin.Repository;
using ExpressedRealms.UseCases.Shared;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Admin.UseCases.Roles.AddRole;

[UsedImplicitly]
internal sealed class AddRoleModelValidator : AbstractValidator<AddRoleModel>
{
    public AddRoleModelValidator(IRolesRepository repository)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(250)
            .WithMessage("Name must be at most 250 characters.")
            .MustAsync(async (x, y) => !await repository.RoleNameExistsAsync(x))
            .WithMessage("Name has already been taken.");

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .WithMessage("Description must be at most 1000 characters.");

        RuleFor(x => x.PermissionIds)
            .CustomAsync(
                async (x, context, y) =>
                {
                    var invalidPermissions = await repository.GetInvalidPermissions(x);

                    if (invalidPermissions.Count != 0)
                    {
                        context.AddFailure(
                            ValidationHelper.AddInvalidIdsFailure(
                                nameof(AddRoleModel.PermissionIds),
                                invalidPermissions
                            )
                        );
                    }
                }
            );
    }
}

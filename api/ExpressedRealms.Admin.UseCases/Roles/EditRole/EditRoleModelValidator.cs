using ExpressedRealms.Admin.Repository;
using ExpressedRealms.UseCases.Shared;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Admin.UseCases.Roles.EditRole;

[UsedImplicitly]
internal sealed class EditRoleModelValidator : AbstractValidator<EditRoleModel>
{
    public EditRoleModelValidator(IRolesRepository repository)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .MustAsync(async (x, y) => await repository.RoleExistsAsync(x))
            .WithMessage("Role does not exist.")
            .WithErrorCode("NotFound");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(250)
            .WithMessage("Name must be at most 250 characters.");

        RuleFor(x => x)
            .MustAsync(async (x, y) => !await repository.RoleNameExistsAsync(x.Id, x.Name))
            .WithMessage("Name has already been taken.")
            .WithName(nameof(EditRoleModel.Name));

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
                                nameof(EditRoleModel.PermissionIds),
                                invalidPermissions
                            )
                        );
                    }
                }
            );
    }
}

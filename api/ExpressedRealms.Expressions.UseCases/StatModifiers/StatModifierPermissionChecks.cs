using System.ComponentModel;
using ExpressedRealms.Authentication.PermissionCollection;
using ExpressedRealms.Expressions.UseCases.StatModifiers.Add;
using ExpressedRealms.Repositories.Shared.ExternalDependencies;
using ExpressedRealms.UseCases.Shared.CommonFailureTypes;
using FluentResults;

namespace ExpressedRealms.Expressions.UseCases.StatModifiers;

internal class StatModifierPermissionChecks(IUserContext userContext)
{
    internal bool HasPermissionPolicyForStatModifiers(
        SourceTableEnum sourceTableEnum,
        out Result fail
    )
    {
        switch (sourceTableEnum)
        {
            case SourceTableEnum.ProgressionLevels:
                if (
                    !userContext.CurrentUserHasPermission(Permissions.ProgressionPath.EditModifiers)
                )
                {
                    fail = Result.Fail(new NotFoundFailure("Stat Modifier", ""));
                    return true;
                }

                break;
            case SourceTableEnum.Blessings:
                if (!userContext.CurrentUserHasPermission(Permissions.Blessings.EditModifiers))
                {
                    fail = Result.Fail(new NotFoundFailure("Stat Modifier", ""));
                    return true;
                }

                break;
            case SourceTableEnum.Powers:
                if (!userContext.CurrentUserHasPermission(Permissions.Powers.EditModifiers))
                {
                    fail = Result.Fail(new NotFoundFailure("Stat Modifier", ""));
                    return true;
                }

                break;
            default:
                throw new InvalidEnumArgumentException(
                    nameof(sourceTableEnum),
                    (int)sourceTableEnum,
                    typeof(SourceTableEnum)
                );
        }

        fail = Result.Ok();

        return false;
    }
}

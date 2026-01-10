using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Admin.UseCases.Roles.GetPermissions;

public interface IGetPermissionsUseCase : IGenericUseCase<Result<PermissionsBaseReturnModel>> { }

using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Admin.UseCases.Roles.GetRole;

public interface IGetRoleUseCase : IGenericUseCase<Result<RoleBaseReturnModel>, GetRoleModel> { }

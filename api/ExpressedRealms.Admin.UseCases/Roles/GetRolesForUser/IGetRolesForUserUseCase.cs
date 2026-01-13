using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Admin.UseCases.Roles.GetRolesForUser;

public interface IGetRolesForUserUseCase
    : IGenericUseCase<Result<List<RoleForUserMappingReturnModel>>, GetRolesForUserModel> { }

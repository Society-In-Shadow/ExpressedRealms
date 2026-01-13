using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Admin.UseCases.Users.GetUsersForRole;

public interface IGetUsersForRoleUseCase
    : IGenericUseCase<Result<List<RoleForUserMappingReturnModel>>, GetUsersForRoleModel> { }

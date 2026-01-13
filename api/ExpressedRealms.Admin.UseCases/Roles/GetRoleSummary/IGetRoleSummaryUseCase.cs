using ExpressedRealms.DB.Shared;
using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Admin.UseCases.Roles.GetRoleSummary;

public interface IGetRoleSummaryUseCase : IGenericUseCase<Result<List<GenericListDto<int>>>> { }

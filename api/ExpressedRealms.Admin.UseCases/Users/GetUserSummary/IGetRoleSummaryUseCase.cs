using ExpressedRealms.DB.Shared;
using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Admin.UseCases.Users.GetUserSummary;

public interface IGetUserSummaryUseCase : IGenericUseCase<Result<List<GenericListDto<string>>>> { }

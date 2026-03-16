using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.GetStonePullInfo;

public interface IGetStonePullInfoUseCase
    : IGenericUseCase<Result<GetStonePullInfoReturnModel>, GetStonePullInfoModel> { }

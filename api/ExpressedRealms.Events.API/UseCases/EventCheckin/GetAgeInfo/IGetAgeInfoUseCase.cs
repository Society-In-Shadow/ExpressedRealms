using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.GetAgeInfo;

public interface IGetAgeInfoUseCase
    : IGenericUseCase<Result<GetAgeInfoReturnModel>, GetAgeInfoModel> { }

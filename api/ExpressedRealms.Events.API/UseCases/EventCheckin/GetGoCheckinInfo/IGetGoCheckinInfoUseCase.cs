using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.GetGoCheckinInfo;

public interface IGetGoCheckinInfoUseCase
    : IGenericUseCase<Result<GetGoCheckinInfoReturnModel>, GetGoCheckinInfoModel> { }

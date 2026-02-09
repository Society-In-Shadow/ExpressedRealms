using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.CheckinUserInfo;

public interface IGetInitialCheckinUserInfoUseCase
    : IGenericUseCase<Result<GetInitialCheckinUserInfoReturnModel>, GetInitialCheckinUserInfoModel> { }

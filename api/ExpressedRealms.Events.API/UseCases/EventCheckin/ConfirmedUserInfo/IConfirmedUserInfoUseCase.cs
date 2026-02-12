using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.ConfirmedUserInfo;

public interface IConfirmedUserInfoUseCase
    : IGenericUseCase<Result<ConfirmedUserInfoReturnModel>, ConfirmedUserInfoModel> { }

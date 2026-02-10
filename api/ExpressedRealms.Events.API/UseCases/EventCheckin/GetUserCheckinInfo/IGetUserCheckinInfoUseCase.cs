using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.GetUserCheckinInfo;

public interface IGetUserCheckinInfoUseCase
    : IGenericUseCase<Result<GetUserCheckinInfoReturnModel>> { }

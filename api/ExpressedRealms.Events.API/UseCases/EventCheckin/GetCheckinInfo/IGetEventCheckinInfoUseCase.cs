using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.GetCheckinInfo;

public interface IGetEventCheckinInfoUseCase
    : IGenericUseCase<Result<CheckinInfoReturnModel>> { }

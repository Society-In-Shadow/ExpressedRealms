using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.Events.GetEvent;

public interface IGetEventUseCase : IGenericUseCase<Result<GetEventReturnModel>, GetEventModel> { }

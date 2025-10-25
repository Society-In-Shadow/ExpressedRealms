using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.Events.Create;

public interface ICreateEventUseCase : IGenericUseCase<Result<int>, CreateEventModel> { }

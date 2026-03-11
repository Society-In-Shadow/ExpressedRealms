using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.UpdateAgeInformation;

public interface IUpdateAgeInformationUseCase
    : IGenericUseCase<Result, UpdateAgeInformationModel> { }

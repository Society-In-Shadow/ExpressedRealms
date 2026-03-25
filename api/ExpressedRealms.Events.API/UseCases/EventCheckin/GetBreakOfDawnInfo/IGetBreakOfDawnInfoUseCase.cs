using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.GetBreakOfDawnInfo;

public interface IGetBreakOfDawnInfoUseCase
    : IGenericUseCase<Result<GetBreakOfDawnInfoDto>, GetBreakOfDawnInfoModel> { }

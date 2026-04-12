using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Admin.UseCases.Users.GetPlayer;

public interface IGetPlayerUseCase
    : IGenericUseCase<Result<PlayerBasicInfoReturnModel>, GetPlayerModel> { }

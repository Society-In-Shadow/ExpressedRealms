using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Characters.UseCases.Characters.GetGoCheckinInfo;

public interface IGetGoCheckinInfoUseCase
    : IGenericUseCase<Result<GetCharacterGoFieldReturnModel>, GetGoCheckinInfoModel> { }

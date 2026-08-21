using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.UpdateCharacterStorageInfo;

public interface IUpdateCharacterStorageInfoUseCase
    : IGenericUseCase<Result, UpdateCharacterStorageInfoModel> { }

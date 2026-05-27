using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Characters.UseCases.Characters.GetCharacterGoFields;

public interface IGetCharacterGoFieldsUseCase
    : IGenericUseCase<Result<GetCharacterGoFieldReturnModel>, GetCharacterGoFieldsModel> { }

using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Characters.UseCases.Characters.GetEditOptions;

public interface IGetEditOptionsUseCase
    : IGenericUseCase<Result<EditCharacterOptionDto>, GetEditOptionsModel> { }

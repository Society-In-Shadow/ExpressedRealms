using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Characters.UseCases.Reports.GetCharacterBooklet
{
    public interface IGetCharacterBookletUseCase
        : IGenericUseCase<Result<MemoryStream>, GetCharacterBookletModel> { }
}

using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Characters.UseCases.Reports.GetCRB
{
    public interface IGetCharacterBookletUseCase
        : IGenericUseCase<Result<MemoryStream>, GetCharacterBookletModel> { }
}

using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Knowledges.UseCases.CharacterKnowledgeMappings.GetEdit;

public interface IGetCharacterMappingEditUseCase
    : IGenericUseCase<Result<GetCharacterMappingEditReturnModel>, GetCharacterMappingEditModel> { }

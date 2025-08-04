using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Knowledges.UseCases.CharacterKnowledgeMappings.Create;

public interface IAddKnowledgeToCharacterUseCase
    : IGenericUseCase<Result<int>, AddKnowledgeToCharacterModel> { }

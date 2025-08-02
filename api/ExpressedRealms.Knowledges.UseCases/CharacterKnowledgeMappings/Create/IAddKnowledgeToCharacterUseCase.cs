using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Knowledges.UseCases.CharacterKnowledgeMappings.Create;

internal interface IAddKnowledgeToCharacterUseCase : IGenericUseCase<Result<int>, AddKnowledgeToCharacterModel>
{
}
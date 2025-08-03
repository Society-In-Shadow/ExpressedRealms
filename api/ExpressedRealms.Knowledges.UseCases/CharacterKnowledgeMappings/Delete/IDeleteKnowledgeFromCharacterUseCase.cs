using ExpressedRealms.Knowledges.UseCases.Knowledges.DeleteKnowledge;
using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Knowledges.UseCases.CharacterKnowledgeMappings.Delete;

internal interface IDeleteKnowledgeFromCharacterUseCase : IGenericUseCase<Result, DeleteKnowledgeFromCharacterModel>
{
}
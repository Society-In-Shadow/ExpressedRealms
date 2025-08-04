using ExpressedRealms.Knowledges.UseCases.Knowledges.DeleteKnowledge;
using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Knowledges.UseCases.CharacterKnowledgeMappings.Delete;

public interface IDeleteKnowledgeFromCharacterUseCase
    : IGenericUseCase<Result, DeleteKnowledgeFromCharacterModel> { }

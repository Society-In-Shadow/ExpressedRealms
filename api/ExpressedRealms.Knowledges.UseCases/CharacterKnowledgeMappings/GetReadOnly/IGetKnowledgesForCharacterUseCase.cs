using ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings.Projections;
using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Knowledges.UseCases.CharacterKnowledgeMappings.GetReadOnly;

public interface IGetKnowledgesForCharacterUseCase
    : IGenericUseCase<
        Result<List<CharacterKnowledgeReturnModel>>,
        GetKnowledgesForCharacterModel
    > { }

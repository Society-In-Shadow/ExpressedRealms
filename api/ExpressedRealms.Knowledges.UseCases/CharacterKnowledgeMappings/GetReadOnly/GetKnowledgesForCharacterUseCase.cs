using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings;
using ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings.Projections;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Knowledges.UseCases.CharacterKnowledgeMappings.GetReadOnly;

internal sealed class GetKnowledgesForCharacterUseCase(
    ICharacterKnowledgeRepository mappingRepository,
    GetKnowledgesForCharacterModelValidator validator,
    CancellationToken cancellationToken
) : IGetKnowledgesForCharacterUseCase
{
    public async Task<Result<List<CharacterKnowledgeReturnModel>>> ExecuteAsync(
        GetKnowledgesForCharacterModel model
    )
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var knowledges = await mappingRepository.GetKnowledgesForCharacter(model.CharacterId);

        var items = knowledges
            .Select(x => new CharacterKnowledgeReturnModel()
            {
                MappingId = x.MappingId,
                Knowledge = new KnowledgeReturnModel()
                {
                    Id = x.Knowledge.Id,
                    Name = x.Knowledge.Name,
                    Description = x.Knowledge.Description,
                    Type = x.Knowledge.Type,
                },
                StoneModifier = x.StoneModifier,
                LevelName = x.LevelName,
                Level = x.Level,
                LevelId = x.LevelId,
                Notes = x.Notes,
                SpecializationCount = x.SpecializationCount,
                Specializations = x
                    .Specializations.Select(y => new SpecializationReturnModel()
                    {
                        Name = y.Name,
                        Description = y.Description,
                        Id = y.Id,
                        Notes = y.Notes,
                    })
                    .ToList(),
            })
            .ToList();

        var foo = 3;
        if (true)
            foo = 4;
        
        return Result.Ok(items.Take(foo).ToList());
    }
}

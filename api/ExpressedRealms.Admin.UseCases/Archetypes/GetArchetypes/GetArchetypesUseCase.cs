using ExpressedRealms.Characters.Repository;
using FluentResults;

namespace ExpressedRealms.Admin.UseCases.Archetypes.GetArchetypes;

internal sealed class GetArchetypesUseCase(ICharacterRepository characterRepository) : IGetArchetypesUseCase
{
    public async Task<Result<ArchetypeBaseReturnModel>> ExecuteAsync()
    {
        var archetypes = await characterRepository.GetBasicArchetypeListAsync();

        return Result.Ok(
            new ArchetypeBaseReturnModel()
            {
                Archetypes = archetypes
                    .Select(x => new ArchetypeModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        ExpressionName = x.ExpressionName,
                        Description = x.Description,
                    })
                    .ToList(),
            }
        );
    }
}

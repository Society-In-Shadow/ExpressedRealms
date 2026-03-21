using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.Repository.Proficiencies;
using ExpressedRealms.Characters.Repository.Xp;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Characters.UseCases.Characters.GetBreakOfDawnInfo;

internal sealed class GetBreakOfDawnInfoUseCase(
    ICharacterRepository repository,
    IProficiencyRepository profRepository,
    IXpRepository xpRepository,
    GetBreakOfDawnInfoModelValidator validator,
    CancellationToken cancellationToken
) : IGetBreakOfDawnInfoUseCase
{
    public async Task<Result<GetBreakOfDawnInfoDto>> ExecuteAsync(GetBreakOfDawnInfoModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);
        
        var proficiencies = await profRepository.GetBasicProficiencies(model.Id);
        var characterLevel = await xpRepository.GetCharacterXpLevel(model.Id);
        var expressionId = await repository.GetCharacterExpressionId(model.Id);

        var expressionType = expressionId switch
        {
            2 => 2, // Aeternari
            3 => 1, // Adepts
            4 => 3, // Shammas
            7 => 4, // Sidhe
            8 => 5, // Sorcerers
            9 => 6 // Vampyres
        };
        
        return Result.Ok(
            new GetBreakOfDawnInfoDto()
            {
                Blood = proficiencies.Value.Where(x => x.Id == 15).Select(x => x.Value).FirstOrDefault(),
                Rwp = proficiencies.Value.Where(x => x.Id == 22).Select(x => x.Value).FirstOrDefault(),
                Mortis = proficiencies.Value.Where(x => x.Id == 23).Select(x => x.Value).FirstOrDefault(),
                Health = proficiencies.Value.Where(x => x.Id == 14).Select(x => x.Value).FirstOrDefault(),
                Vitality = proficiencies.Value.Where(x => x.Id == 13).Select(x => x.Value).FirstOrDefault(),
                Psyche = proficiencies.Value.Where(x => x.Id == 17).Select(x => x.Value).FirstOrDefault(),
                CharacterLevel = characterLevel,
                ExpressionId = expressionType
            }
        );
    }
}

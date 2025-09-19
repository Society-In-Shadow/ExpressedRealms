using ExpressedRealms.Characters.Repository.Xp;
using ExpressedRealms.DB.Characters.xpTables;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Characters.UseCases.ExperienceBreakdown;

internal sealed class GetCharacterExperienceBreakdownUseCase(
    IXpRepository xpRepository,
    GetCharacterExperienceBreakdownModelValidator validator,
    CancellationToken cancellationToken
) : IGetCharacterExperienceBreakdownUseCase
{
    public async Task<Result<ExperienceBreakdownReturnModel>> ExecuteAsync(
        GetCharacterExperienceBreakdownModel model
    )
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var costs = new List<ExperienceTotalMax>();

        var xpInfo = await xpRepository.GetCharacterXpMappings(model.CharacterId);
        
        costs.AddRange(xpInfo
            .Select(x => 
                new ExperienceTotalMax(x.SectionName, 
                    x.SpentXp, 
                    x.SectionCap, 
                    true, 
                    true,
                    x.SectionTypeId)
            ).ToList());

        var advantage = costs.First(x => x.TypeId == (int)XpSectionTypeEnum.Advantages);
        advantage.IncludeInMax = false;

        var disadvantage = costs.First(x => x.TypeId == (int)XpSectionTypeEnum.Disadvantages);
        disadvantage.IncludeInTotal = false;
        
        var discretion = costs.First(x => x.TypeId == (int)XpSectionTypeEnum.Discretion);
        discretion.IncludeInTotal = false;
        discretion.Total = -1;

        /*var totalXp = costs.Where(x => x.IncludeInTotal).Sum(x => x.Total);
        var maxXp = costs.Where(x => x.IncludeInMax).Sum(x => x.Max);

        costs.Add(new ExperienceTotalMax("Total", totalXp, maxXp));*/

        return Result.Ok(new ExperienceBreakdownReturnModel()
        {
            ExperienceSections = costs,
            AvailableDiscretionary = await xpRepository.GetAvailableDiscretionary(model.CharacterId)
        });
    }
}

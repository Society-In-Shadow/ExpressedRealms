using ExpressedRealms.Powers.API.PowerEndpoints.GetPowerOptions;
using ExpressedRealms.Powers.Repository.Powers;
using Microsoft.AspNetCore.Http;

namespace ExpressedRealms.Powers.API.PowerPrerequisites.GetPrerequisiteOptions;

public static class GetPrerequisiteOptionsEndpoint
{
    public static async Task<IResult> Execute(int id, IPowerRepository powerRepository)
    {
        var powers = await powerRepository.GetPowersAsync(id);

        var requiredAmount = new List<DetailedEditInformation>();

        for (int i = 1; i <= powers.Value.Count; i++)
        {
            requiredAmount.Add(
                new DetailedEditInformation()
                {
                    Id = i,
                    Name = GetName(i),
                    Description = GetName(i),
                }
            );
        }

        string GetName(int index)
        {
            if (index == 1 && index != powers.Value.Count - 1)
                return "1 (Any)";

            if (index == powers.Value.Count - 1)
                return $"{index} (All)";

            return $"{index}";
        }

        return TypedResults.Ok(
            new PrerequisiteOptions()
            {
                RequiredAmount = requiredAmount,
                PrerequisitePowers = powers
                    .Value.Select(x => new DetailedEditInformation()
                    {
                        Id = x.Id,
                        Description = x.Description,
                        Name = x.Name,
                    })
                    .ToList(),
            }
        );
    }
}

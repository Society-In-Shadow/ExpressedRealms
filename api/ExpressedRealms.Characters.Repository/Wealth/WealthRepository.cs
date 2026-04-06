using ExpressedRealms.Characters.Repository.Wealth.Dtos;
using ExpressedRealms.DB;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Characters.Repository.Wealth;

public class WealthRepository(
    ExpressedRealmsDbContext context,
    CancellationToken cancellationToken) : IWealthRepository
{
    public async Task<WealthInfoDto> GetWealthInfoAsync(int characterId)
    {
        var targetBlessings = new List<string>() { "Destitute", "Wealthy" };

        var blessings = await context
            .CharacterBlessingMappings
            .Where(x => x.CharacterId == characterId && targetBlessings.Contains(x.Blessing.Name))
            .Select(x => new
            {
                Name = x.Blessing.Name,
                LevelName = x.BlessingLevel.Level,
            })
            .ToListAsync(cancellationToken);
            
        var destitute = blessings.FirstOrDefault(x =>
            x.Name == "Destitute"
        );
        var wealthy = blessings.FirstOrDefault(x =>
            x.Name == "Wealthy"
        );

        var wealthLevel = 1;
        double incomeModifier = 1;

        if (destitute is not null)
        {
            switch (destitute.LevelName)
            {
                case "2pt":
                    incomeModifier = 0.75;
                    break;
                case "4pt":
                    incomeModifier = 0.5;
                    break;
                case "6pt":
                    incomeModifier = 0.25;
                    break;
                case "8pt":
                    wealthLevel = 0;
                    incomeModifier = 0;
                    break;
            }
        }

        if (wealthy is not null)
        {
            switch (wealthy.LevelName)
            {
                case "2pt":
                    wealthLevel = 3;
                    break;
                case "4pt":
                    wealthLevel = 4;
                    incomeModifier += 0.05;
                    break;
                case "6pt":
                    wealthLevel = 5;
                    incomeModifier += 0.1;
                    break;
                case "8pt":
                    wealthLevel = 6;
                    incomeModifier += 0.25;
                    break;
            }
        }

        var sessionIncome = wealthLevel switch
        {
            0 => 0,
            1 => 50,
            2 => 100,
            _ => Math.Pow(2, wealthLevel - 2) * 100
        };

        var wealthIncome = sessionIncome * incomeModifier;

        var liquidation = wealthLevel switch
        {
            0 => 0,
            1 => 0,
            _ => wealthIncome * 15
        };

        return new WealthInfoDto()
        {
            WealthLevel = wealthLevel,
            WealthIncome = wealthIncome,
            BankedCash = wealthIncome * 30,
            Liquadation = liquidation,
            InitialBasicItemIncome = wealthIncome * 3
        };
    }
}
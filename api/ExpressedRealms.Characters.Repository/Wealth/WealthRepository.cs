using ExpressedRealms.Characters.Repository.Wealth.Dtos;
using ExpressedRealms.DB;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Characters.Repository.Wealth;

public class WealthRepository(ExpressedRealmsDbContext context, CancellationToken cancellationToken)
    : IWealthRepository
{
    public async Task<WealthInfoDto> GetWealthInfoAsync(int characterId)
    {
        var targetBlessings = new List<string>() { "Destitute", "Wealthy", "Disowned / Disfavored" };

        var blessings = await context
            .CharacterBlessingMappings.Where(x =>
                x.CharacterId == characterId && targetBlessings.Contains(x.Blessing.Name)
            )
            .Select(x => new { Name = x.Blessing.Name, LevelName = x.BlessingLevel.Level })
            .ToListAsync(cancellationToken);

        var destitute = blessings.FirstOrDefault(x => x.Name == "Destitute");
        var wealthy = blessings.FirstOrDefault(x => x.Name == "Wealthy");
        var disOwned = blessings.FirstOrDefault(x => x.Name == "Disowned / Disfavored");

        var wealthLevel = 1;
        double incomeModifier = 1;
        double bankCashModifier = 1;

        if (wealthy is not null)
        {
            switch (wealthy.LevelName)
            {
                case "2pt":
                    wealthLevel = 3;
                    break;
                case "4pt":
                    wealthLevel = 4;
                    incomeModifier = 0.05;
                    break;
                case "6pt":
                    wealthLevel = 5;
                    incomeModifier = 0.1;
                    break;
                case "8pt":
                    wealthLevel = 6;
                    incomeModifier = 0.25;
                    break;
            }
        }
        
        if (disOwned is not null && disOwned.LevelName == "8pt")
        {
            wealthLevel = 0;
            bankCashModifier += 0.1;
        }
        
        if (destitute is not null)
        {
            switch (destitute.LevelName)
            {
                case "2pt":
                    incomeModifier -= 0.25;
                    bankCashModifier += 0.25;
                    break;
                case "4pt":
                    incomeModifier -= 0.5;
                    bankCashModifier += 0.50;
                    break;
                case "6pt":
                    incomeModifier -= 0.75;
                    bankCashModifier += 0.50;
                    break;
                case "8pt":
                    wealthLevel = 0;
                    incomeModifier = 0;
                    bankCashModifier += 0.50;
                    break;
            }
        }

        var sessionIncome = SessionIncome(wealthLevel);

        var wealthIncome = sessionIncome * incomeModifier;

        var liquidation = wealthLevel switch
        {
            0 => 0,
            1 => 0,
            _ => sessionIncome * 15,
        };

        return new WealthInfoDto()
        {
            WealthLevel = wealthLevel,
            WealthIncome = wealthIncome,
            BankedCash = SessionIncome(wealthLevel + 1) * 30 * bankCashModifier,
            Liquadation = liquidation,
            InitialBasicItemIncome = wealthIncome * 3,
        };
    }

    private static double SessionIncome(int wealthLevel)
    {
        return wealthLevel switch
        {
            0 => 0,
            1 => 50,
            2 => 100,
            _ => Math.Pow(2, wealthLevel - 2) * 100,
        };
    }
}

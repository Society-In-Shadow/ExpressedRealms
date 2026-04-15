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
        var appliedBlessings = new List<KeyValuePair<string, string>>();

        if (wealthy is not null)
        {
            appliedBlessings.Add(new KeyValuePair<string, string>(wealthy.Name, wealthy.LevelName));
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
        
        if (disOwned is not null && disOwned.LevelName == "8pt")
        {
            appliedBlessings.Add(new KeyValuePair<string, string>(disOwned.Name, disOwned.LevelName));
            wealthLevel = 0;
            bankCashModifier += 0.1;
        }
        
        if (destitute is not null)
        {
            appliedBlessings.Add(new KeyValuePair<string, string>(destitute.Name, destitute.LevelName));
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

        var tableRows = new List<WealthTableRow>();
        var topRange = Math.Max(10, wealthLevel + 2);
        foreach (var level in Enumerable.Range(0, topRange + 1))
        {
            var sessionIncome = SessionIncome(level);

            var wealthIncome = sessionIncome * incomeModifier;

            var liquidation = level switch
            {
                0 => 0,
                1 => 0,
                _ => sessionIncome * 15,
            };

            tableRows.Add(new WealthTableRow()
            {
                Level = level,
                SessionIncome = wealthIncome,
                CashToLevelUp = SessionIncome(level + 1) * 30 * bankCashModifier,
                LiquidationValue = liquidation,
            });
        }

        return new WealthInfoDto()
        {
            WealthLevel = wealthLevel,
            InitialBasicItemIncome = tableRows.First(x => x.Level == wealthLevel).SessionIncome * 3,
            WealthTable = tableRows,
            AppliedBlessings = appliedBlessings,
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

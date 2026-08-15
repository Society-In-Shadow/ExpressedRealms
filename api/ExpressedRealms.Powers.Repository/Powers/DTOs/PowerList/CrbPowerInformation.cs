using System.Linq.Expressions;
using ExpressedRealms.DB.Models.Powers.PowerPathPowerMappingSetup;

namespace ExpressedRealms.Powers.Repository.Powers.DTOs.PowerList;

public class CrbPowerInformation
{
    public static Expression<Func<PowerPathPowerMapping, CrbPowerInformation>> Selector()
    {
        return x => new CrbPowerInformation
        {
            Id = x.Power.Id,
            Name = x.Power.Name,
            PathName = x.PowerPath.Name,
            Category = x
                .Power.CategoryMappings.Select(y => y.Category.Name)
                .ToList(),
            Description = x.Power.Description,
            GameMechanicEffect = x.Power.GameMechanicEffect ?? string.Empty,
            Limitation = x.Power.Limitation ?? string.Empty,
            PowerDuration = x.Power.PowerDuration.Name,
            AreaOfEffect = x.Power.PowerAreaOfEffectType.Name,
            PowerLevel = x.Power.PowerLevel.Name,
            PowerActivationType = x.Power.PowerActivationTimingType.Name,
            Other = x.Power.OtherFields,
            IsPowerUse = x.Power.IsPowerUse,
            Cost = x.Power.Cost,
            SortOrder = x.OrderIndex,
            Prerequisites =
                x.Power.Prerequisite != null
                    ? new PrerequisiteDetails
                    {
                        RequiredAmount = x.Power.Prerequisite.RequiredAmount,
                        Powers = x
                            .Power.Prerequisite.PrerequisitePowers.Select(pp => pp.Power.Name)
                            .ToList(),
                    }
                    : null,
            ModifierGroupId = x.Power.StatModifierGroupId,
        };
    }

    public required string PathName { get; set; }
    public int? ModifierGroupId { get; set; }
    public int Id { get; set; }
    public required string Name { get; set; }
    public List<string>? Category { get; set; }
    public required string Description { get; set; }
    public required string GameMechanicEffect { get; set; }
    public string? Limitation { get; set; }
    public required string PowerDuration { get; set; }
    public required string AreaOfEffect { get; set; }
    public required string PowerLevel { get; set; }
    public required string PowerActivationType { get; set; }
    public string? Other { get; set; }
    public bool IsPowerUse { get; set; }
    public string? Cost { get; set; }
    public int SortOrder { get; set; }
    public PrerequisiteDetails? Prerequisites { get; set; }
}

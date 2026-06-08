using System.Linq.Expressions;
using ExpressedRealms.DB.Models.Powers.PowerPathPowerMappingSetup;

namespace ExpressedRealms.Powers.Repository.Powers.DTOs.PowerList;

public class PowerInformation
{
    public static Expression<Func<PowerPathPowerMapping, PowerInformation>> Selector()
    {
        return x => new PowerInformation
        {
            Id = x.Power.Id,
            Name = x.Power.Name,
            Category = x
                .Power.CategoryMappings.Select(y => new DetailedInformation(
                    y.Category.Name,
                    y.Category.Description
                ))
                .ToList(),
            Description = x.Power.Description,
            GameMechanicEffect = x.Power.GameMechanicEffect ?? string.Empty,
            Limitation = x.Power.Limitation ?? string.Empty,
            PowerDuration = new DetailedInformation(
                x.Power.PowerDuration.Name,
                x.Power.PowerDuration.Description
            ),
            AreaOfEffect = new DetailedInformation(
                x.Power.PowerAreaOfEffectType.Name,
                x.Power.PowerAreaOfEffectType.Description
            ),
            PowerLevel = new DetailedInformation(
                x.Power.PowerLevel.Id,
                x.Power.PowerLevel.Name,
                x.Power.PowerLevel.Description
            ),
            PowerActivationType = new DetailedInformation(
                x.Power.PowerActivationTimingType.Name,
                x.Power.PowerActivationTimingType.Description
            ),
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

    public int? ModifierGroupId { get; set; }

    public int Id { get; set; }
    public required string Name { get; set; }
    public List<DetailedInformation>? Category { get; set; }
    public required string Description { get; set; }
    public required string GameMechanicEffect { get; set; }
    public string? Limitation { get; set; }
    public required DetailedInformation PowerDuration { get; set; }
    public required DetailedInformation AreaOfEffect { get; set; }
    public required DetailedInformation PowerLevel { get; set; }
    public required DetailedInformation PowerActivationType { get; set; }
    public string? Other { get; set; }
    public bool IsPowerUse { get; set; }
    public string? Cost { get; set; }
    public int SortOrder { get; set; }
    public PrerequisiteDetails? Prerequisites { get; set; }
}

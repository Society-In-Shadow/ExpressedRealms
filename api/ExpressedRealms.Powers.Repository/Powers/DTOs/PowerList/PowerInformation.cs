using System.Linq.Expressions;
using ExpressedRealms.DB.Models.Powers;

namespace ExpressedRealms.Powers.Repository.Powers.DTOs.PowerList;

public class PowerInformation
{
    public static readonly Expression<Func<Power, PowerInformation>> Selector =
        x => new PowerInformation
        {
            Id = x.Id,
            Name = x.Name,
            Category = x
                .CategoryMappings.Select(y => new DetailedInformation(
                    y.Category.Name,
                    y.Category.Description
                ))
                .ToList(),
            Description = x.Description,
            GameMechanicEffect = x.GameMechanicEffect ?? string.Empty,
            Limitation = x.Limitation ?? string.Empty,
            PowerDuration = new DetailedInformation(
                x.PowerDuration.Name,
                x.PowerDuration.Description
            ),
            AreaOfEffect = new DetailedInformation(
                x.PowerAreaOfEffectType.Name,
                x.PowerAreaOfEffectType.Description
            ),
            PowerLevel = new DetailedInformation(x.PowerLevel.Name, x.PowerLevel.Description),
            PowerActivationType = new DetailedInformation(
                x.PowerActivationTimingType.Name,
                x.PowerActivationTimingType.Description
            ),
            Other = x.OtherFields,
            IsPowerUse = x.IsPowerUse,
            Cost = x.Cost,
            SortOrder = x.OrderIndex,
            Prerequisites =
                x.Prerequisite != null
                    ? new PrerequisiteDetails
                    {
                        RequiredAmount = x.Prerequisite.RequiredAmount,
                        Powers = x
                            .Prerequisite.PrerequisitePowers.Select(pp => pp.Power.Name)
                            .ToList(),
                    }
                    : null,
        };

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

using ExpressedRealms.DB.Models.Powers;

namespace ExpressedRealms.Powers.Repository.Powers.DTOs.Options;

public class DetailedEditInformation
{
    public DetailedEditInformation(PowerCategory powerCategory)
    {
        Id = powerCategory.Id;
        Name = powerCategory.Name;
        Description = powerCategory.Description;
    }

    public DetailedEditInformation(PowerDuration powerDuration)
    {
        Id = powerDuration.Id;
        Name = powerDuration.Name;
        Description = powerDuration.Description;
    }

    public DetailedEditInformation(PowerAreaOfEffectType areaOfEffect)
    {
        Id = areaOfEffect.Id;
        Name = areaOfEffect.Name;
        Description = areaOfEffect.Description;
    }

    public DetailedEditInformation(PowerLevel powerLevel)
    {
        Id = powerLevel.Id;
        Name = powerLevel.Name;
        Description = powerLevel.Description;
    }

    public DetailedEditInformation(PowerActivationTimingType activationTiming)
    {
        Id = activationTiming.Id;
        Name = activationTiming.Name;
        Description = activationTiming.Description;
    }

    public DetailedEditInformation(int id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}

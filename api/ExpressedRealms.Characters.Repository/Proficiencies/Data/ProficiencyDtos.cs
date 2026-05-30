using ExpressedRealms.Characters.Repository.Proficiencies.DTOs;
using ExpressedRealms.Characters.Repository.Proficiencies.Enums;

namespace ExpressedRealms.Characters.Repository.Proficiencies.Data;

public static class ProficiencyDtos
{
    private const string Offensive = "Offensive";
    private const string Defensive = "Defensive";
    private const string Secondary = "Secondary";

    public static List<ProficiencyDto> GetProficiencies(int expressionId)
    {
        var standardProficiencies = new List<ProficiencyDto>()
        {
            new ProficiencyDto()
            {
                Id = 1,
                Name = "Strike",
                Type = Offensive,
                Modifiers = new List<ModifierType>()
                {
                    ModifierType.Dexterity,
                    ModifierType.Strength,
                    ModifierType.HandToHandOffense,
                    ModifierType.Strike,
                },
                SortOrder = 1,
            },
            new ProficiencyDto()
            {
                Id = 2,
                Name = "Dodge",
                Modifiers = new List<ModifierType>()
                {
                    ModifierType.Agility,
                    ModifierType.Strength,
                    ModifierType.HandToHandDefense,
                    ModifierType.Dodge,
                },
                Type = Defensive,
                SortOrder = 1,
            },
            new ProficiencyDto()
            {
                Id = 3,
                Name = "Thrust",
                Type = Offensive,
                Modifiers = new List<ModifierType>()
                {
                    ModifierType.Agility,
                    ModifierType.Dexterity,
                    ModifierType.MeleeOffense,
                    ModifierType.Thrust,
                },
                SortOrder = 2,
            },
            new ProficiencyDto()
            {
                Id = 4,
                Name = "Parry",
                Type = Defensive,
                Modifiers = new List<ModifierType>()
                {
                    ModifierType.Agility,
                    ModifierType.Strength,
                    ModifierType.MeleeDefense,
                    ModifierType.Parry,
                },
                SortOrder = 2,
            },
            new ProficiencyDto()
            {
                Id = 5,
                Name = "Throw",
                Type = Offensive,
                Modifiers = new List<ModifierType>()
                {
                    ModifierType.Dexterity,
                    ModifierType.Intelligence,
                    ModifierType.ThrownWeapons,
                    ModifierType.Throw,
                },
                SortOrder = 3,
            },
            new ProficiencyDto()
            {
                Id = 6,
                Name = "Evade Throw",
                Type = Defensive,
                Modifiers = new List<ModifierType>()
                {
                    ModifierType.Agility,
                    ModifierType.Intelligence,
                    ModifierType.Acrobatics,
                    ModifierType.EvadeThrow,
                },
                SortOrder = 3,
            },
            new ProficiencyDto()
            {
                Id = 7,
                Name = "Shoot",
                Type = Offensive,
                Modifiers = new List<ModifierType>()
                {
                    ModifierType.Dexterity,
                    ModifierType.Intelligence,
                    ModifierType.Marksmanship,
                    ModifierType.Shoot,
                },
                SortOrder = 4,
            },
            new ProficiencyDto()
            {
                Id = 8,
                Name = "Evade Shoot",
                Type = Defensive,
                Modifiers = new List<ModifierType>()
                {
                    ModifierType.Agility,
                    ModifierType.Intelligence,
                    ModifierType.Acrobatics,
                    ModifierType.EvadeShoot,
                },
                SortOrder = 4,
            },
            new ProficiencyDto()
            {
                Id = 9,
                Name = "Cast",
                Type = Offensive,
                Modifiers = new List<ModifierType>()
                {
                    ModifierType.Intelligence,
                    ModifierType.Willpower,
                    ModifierType.Spellcasting,
                    ModifierType.Cast,
                },
                SortOrder = 5,
            },
            new ProficiencyDto()
            {
                Id = 10,
                Name = "Ward",
                Type = Defensive,
                Modifiers = new List<ModifierType>()
                {
                    ModifierType.Constitution,
                    ModifierType.Willpower,
                    ModifierType.Spellwarding,
                    ModifierType.Ward,
                },
                SortOrder = 5,
            },
            new ProficiencyDto()
            {
                Id = 11,
                Name = "Project",
                Type = Offensive,
                Modifiers = new List<ModifierType>()
                {
                    ModifierType.Intelligence,
                    ModifierType.Willpower,
                    ModifierType.Projection,
                    ModifierType.Project,
                },
                SortOrder = 6,
            },
            new ProficiencyDto()
            {
                Id = 12,
                Name = "Deflect",
                Type = Defensive,
                Modifiers = new List<ModifierType>()
                {
                    ModifierType.Constitution,
                    ModifierType.Willpower,
                    ModifierType.Deflection,
                    ModifierType.Deflect,
                },
                SortOrder = 6,
            },
            new ProficiencyDto()
            {
                Id = 13,
                Name = expressionId == 2 ? "Vitality (PP)" : "Vitality",
                Type = Secondary,
                Modifiers = new List<ModifierType>()
                {
                    ModifierType.Constitution,
                    ModifierType.Constitution,
                    ModifierType.Strength,
                },
                SortOrder = 1,
            },
            new ProficiencyDto()
            {
                Id = 14,
                Name = "Health",
                Type = Secondary,
                Modifiers = new List<ModifierType>()
                {
                    ModifierType.Constitution,
                    ModifierType.Constitution,
                    ModifierType.Strength,
                },
                SortOrder = 2,
            },
            new ProficiencyDto()
            {
                Id = 15,
                Name = expressionId == 9 ? "Blood (PP)" : "Blood",
                Type = Secondary,
                Modifiers = new List<ModifierType>()
                {
                    ModifierType.Constitution,
                    ModifierType.Constitution,
                    ModifierType.Strength,
                },
                SortOrder = 3,
            },
            new ProficiencyDto()
            {
                Id = 16,
                Name = "Reaction",
                Type = Secondary,
                Modifiers = new List<ModifierType>()
                {
                    ModifierType.Agility,
                    ModifierType.Dexterity,
                    ModifierType.Intelligence,
                },
                SortOrder = 5,
            },
            new ProficiencyDto()
            {
                Id = 17,
                Name = "Psyche",
                Type = Secondary,
                Modifiers = new List<ModifierType>()
                {
                    ModifierType.Constitution,
                    ModifierType.Constitution,
                    ModifierType.Willpower,
                },
                SortOrder = 6,
            },
            new ProficiencyDto()
            {
                Id = 22,
                Name = "RWP",
                Type = Secondary,
                Modifiers = new List<ModifierType>() { ModifierType.Willpower, ModifierType.RWP },
                SortOrder = 4,
            },
            new ProficiencyDto()
            {
                Id = 23,
                Name = "Mortis",
                Type = Secondary,
                Modifiers = new List<ModifierType>() { ModifierType.Mortis },
                SortOrder = 7,
            },
            new ProficiencyDto()
            {
                Id = 24,
                Name = "Void Motes",
                Type = Secondary,
                Modifiers = new List<ModifierType>() { ModifierType.Motes },
                SortOrder = 12,
            },
            new ProficiencyDto()
            {
                Id = 25,
                Name = "Wealth Level",
                Type = Secondary,
                Modifiers = new List<ModifierType>() { ModifierType.WealthLevel },
                SortOrder = 13,
            },
        };

        const int Adepts = 3;
        const int Sidhe = 7;
        const int Sorcerers = 8;
        const int Shammas = 4;

        var expressionSpecificProficiency = expressionId switch
        {
            Adepts => new ProficiencyDto()
            {
                Id = 18,
                Name = "Chi (PP)",
                Type = Secondary,
                Modifiers = new List<ModifierType>()
                {
                    ModifierType.Constitution,
                    ModifierType.Constitution,
                    ModifierType.Intelligence,
                },
                SortOrder = 8,
            },
            Sidhe => new ProficiencyDto()
            {
                Id = 19,
                Name = "Essence (PP)",
                Type = Secondary,
                Modifiers = new List<ModifierType>()
                {
                    ModifierType.Constitution,
                    ModifierType.Willpower,
                    ModifierType.Willpower,
                },
                SortOrder = 9,
            },
            Sorcerers => new ProficiencyDto()
            {
                Id = 20,
                Name = "Mana (PP)",
                Type = Secondary,
                Modifiers = new List<ModifierType>()
                {
                    ModifierType.Intelligence,
                    ModifierType.Willpower,
                    ModifierType.Willpower,
                },
                SortOrder = 10,
            },
            Shammas => new ProficiencyDto()
            {
                Id = 21,
                Name = "Noumenon (PP)",
                Type = Secondary,
                Modifiers = new List<ModifierType>()
                {
                    ModifierType.Constitution,
                    ModifierType.Intelligence,
                    ModifierType.Intelligence,
                },
                SortOrder = 11,
            },
            _ => null,
        };

        if (expressionSpecificProficiency != null)
        {
            standardProficiencies.Add(expressionSpecificProficiency);
        }

        return standardProficiencies;
    }
}

using ExpressedRealms.Characters.Repository.Proficiencies.DTOs;
using ExpressedRealms.Characters.Repository.Proficiencies.Enums;

namespace ExpressedRealms.Characters.Repository.Proficiencies.Data;

public static class ProficiencyDtos
{
    private const string StaticDescription = "Lorem Ipsum";
    private const string Offensive = "Offensive";
    private const string Defensive = "Defensive";
    private const string Secondary = "Secondary";

    public static List<ProficiencyDto> GetProficiencies()
    {
        return new List<ProficiencyDto>()
        {
            new ProficiencyDto()
            {
                Id = 1,
                Name = "Strike",
                Description = StaticDescription,
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
                Description = StaticDescription,
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
                Description = StaticDescription,
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
                Description = StaticDescription,
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
                Description = StaticDescription,
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
                Name = "Evade",
                Description = StaticDescription,
                Type = Defensive,
                Modifiers = new List<ModifierType>()
                {
                    ModifierType.Agility,
                    ModifierType.Intelligence,
                    ModifierType.Acrobatics,
                    ModifierType.Evade,
                },
                SortOrder = 3,
            },
            new ProficiencyDto()
            {
                Id = 7,
                Name = "Shoot",
                Description = StaticDescription,
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
                Name = "Evade",
                Description = StaticDescription,
                Type = Defensive,
                Modifiers = new List<ModifierType>()
                {
                    ModifierType.Agility,
                    ModifierType.Intelligence,
                    ModifierType.Acrobatics,
                    ModifierType.Evade,
                },
                SortOrder = 4,
            },
            new ProficiencyDto()
            {
                Id = 9,
                Name = "Cast",
                Description = StaticDescription,
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
                Description = StaticDescription,
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
                Description = StaticDescription,
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
                Description = StaticDescription,
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
                Name = "Vitality",
                Description = StaticDescription,
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
                Description = StaticDescription,
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
                Name = "Blood",
                Description = StaticDescription,
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
                Description = StaticDescription,
                Type = Secondary,
                Modifiers = new List<ModifierType>()
                {
                    ModifierType.Agility,
                    ModifierType.Dexterity,
                    ModifierType.Intelligence,
                },
                SortOrder = 4,
            },
            new ProficiencyDto()
            {
                Id = 17,
                Name = "Psyche",
                Description = StaticDescription,
                Type = Secondary,
                Modifiers = new List<ModifierType>()
                {
                    ModifierType.Constitution,
                    ModifierType.Constitution,
                    ModifierType.Willpower,
                },
                SortOrder = 5,
            },
            new ProficiencyDto()
            {
                Id = 18,
                Name = "Chi",
                Description = StaticDescription,
                Type = Secondary,
                Modifiers = new List<ModifierType>()
                {
                    ModifierType.Constitution,
                    ModifierType.Constitution,
                    ModifierType.Intelligence,
                },
                SortOrder = 6,
            },
            new ProficiencyDto()
            {
                Id = 19,
                Name = "Essence",
                Description = StaticDescription,
                Type = Secondary,
                Modifiers = new List<ModifierType>()
                {
                    ModifierType.Constitution,
                    ModifierType.Willpower,
                    ModifierType.Willpower,
                },
                SortOrder = 7,
            },
            new ProficiencyDto()
            {
                Id = 20,
                Name = "Mana",
                Description = StaticDescription,
                Type = Secondary,
                Modifiers = new List<ModifierType>()
                {
                    ModifierType.Intelligence,
                    ModifierType.Willpower,
                    ModifierType.Willpower,
                },
                SortOrder = 8,
            },
            new ProficiencyDto()
            {
                Id = 21,
                Name = "Noumenon",
                Description = StaticDescription,
                Type = Secondary,
                Modifiers = new List<ModifierType>()
                {
                    ModifierType.Constitution,
                    ModifierType.Willpower,
                    ModifierType.Willpower,
                },
                SortOrder = 9,
            },
            new ProficiencyDto()
            {
                Id = 22,
                Name = "RWP",
                Description = "Reserve Will Power",
                Type = Secondary,
                Modifiers = new List<ModifierType>()
                {
                    ModifierType.Willpower,
                    ModifierType.RWP
                },
                SortOrder = 10,
            },
             
            new ProficiencyDto()
            {
                Id = 23,
                Name = "Mortis",
                Description = StaticDescription,
                Type = Secondary,
                Modifiers = new List<ModifierType>()
                {
                    ModifierType.Mortis
                },
                SortOrder = 11,
            },
        };
    }
}

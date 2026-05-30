using ExpressedRealms.Characters.Repository.Enums;
using ExpressedRealms.Characters.Repository.Proficiencies.DTOs;
using ExpressedRealms.Characters.Repository.Proficiencies.Enums;
using ExpressedRealms.Characters.Repository.Stats.Enums;

namespace ExpressedRealms.Characters.Repository.Proficiencies.Utilities;

internal static class ModiferConversions
{
    public static ModifierType GetModifierType(StatType statType)
    {
        return statType switch
        {
            StatType.Dexterity => ModifierType.Dexterity,
            StatType.Strength => ModifierType.Strength,
            StatType.Agility => ModifierType.Agility,
            StatType.Intelligence => ModifierType.Intelligence,
            StatType.Willpower => ModifierType.Willpower,
            StatType.Constitution => ModifierType.Constitution,
            _ => throw new ArgumentOutOfRangeException(nameof(statType), statType, null),
        };
    }

    public static ModifierType GetModifierType(SkillTypes statType)
    {
        return statType switch
        {
            SkillTypes.HandToHandDefense => ModifierType.HandToHandDefense,
            SkillTypes.HandToHandOffense => ModifierType.HandToHandOffense,
            SkillTypes.MeleeOffense => ModifierType.MeleeOffense,
            SkillTypes.Marksmanship => ModifierType.Marksmanship,
            SkillTypes.ThrownWeapons => ModifierType.ThrownWeapons,
            SkillTypes.Spellcasting => ModifierType.Spellcasting,
            SkillTypes.Projection => ModifierType.Projection,
            SkillTypes.MeleeDefense => ModifierType.MeleeDefense,
            SkillTypes.Acrobatics => ModifierType.Acrobatics,
            SkillTypes.Spellwarding => ModifierType.Spellwarding,
            SkillTypes.Deflection => ModifierType.Deflection,
            _ => throw new ArgumentOutOfRangeException(nameof(statType), statType, null),
        };
    }

    public static ModifierType GetModifierType(ProficiencyModifierInfoDto mapping)
    {
        return mapping.ModifierTypeId switch
        {
            1 => ModifierType.Vitality,
            2 => ModifierType.Health,
            3 => ModifierType.Blood,
            4 => ModifierType.Reaction,
            5 => ModifierType.Psyche,
            6 => ModifierType.RWP,
            7 => ModifierType.Mortis,
            8 => ModifierType.Chi,
            9 => ModifierType.Essence,
            10 => ModifierType.Mana,
            11 => ModifierType.Noumenon,
            12 => ModifierType.Strike,
            13 => ModifierType.Thrust,
            14 => ModifierType.Throw,
            15 => ModifierType.Shoot,
            16 => ModifierType.Cast,
            17 => ModifierType.Project,
            18 => ModifierType.Dodge,
            19 => ModifierType.Parry,
            20 => ModifierType.EvadeThrow,
            21 => ModifierType.EvadeShoot,
            22 => ModifierType.Ward,
            23 => ModifierType.Deflect,
            24 => ModifierType.Motes,
            25 => ModifierType.WealthLevel,
            _ => throw new ArgumentOutOfRangeException(
                nameof(mapping),
                mapping.ModifierTypeId,
                null
            ),
        };
    }
    
    public static ModifierType GetModifierType(ProficiencyDto mapping)
    {
        return mapping.Id switch
        {
            1 => ModifierType.Strike,
            2 => ModifierType.Dodge,
            3 => ModifierType.Thrust,
            4 => ModifierType.Parry,
            5 => ModifierType.Throw,
            6 => ModifierType.EvadeShoot,
            7 => ModifierType.Shoot,
            8 => ModifierType.EvadeShoot,
            9 => ModifierType.Cast,
            10 => ModifierType.Ward,
            11 => ModifierType.Project,
            12 => ModifierType.Deflect,
            13 => ModifierType.Vitality,
            14 => ModifierType.Health,
            15 => ModifierType.Blood,
            16 => ModifierType.Reaction,
            17 => ModifierType.Psyche,
            18 => ModifierType.Chi,
            19 => ModifierType.Essence,
            20 => ModifierType.Mana,
            21 => ModifierType.Noumenon,
            22 => ModifierType.RWP,
            23 => ModifierType.Mortis,
            24 => ModifierType.Motes,
            25 => ModifierType.WealthLevel,
            _ => throw new ArgumentOutOfRangeException(
                nameof(mapping),
                mapping.Id,
                null
            ),
        };
    }
}

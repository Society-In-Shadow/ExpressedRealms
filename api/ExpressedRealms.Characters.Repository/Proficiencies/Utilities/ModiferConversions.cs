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

    public static ModifierType GetModifierType(DbModifierTypes statType)
    {
        return statType switch
        {
            DbModifierTypes.StrengthStonePull => ModifierType.Strength,
            DbModifierTypes.StrikeProficiencyModifier => ModifierType.Strike,
            DbModifierTypes.ConstitutionStonePull => ModifierType.Constitution,
            //DbModifierTypes.WisdomStonePull => ,
            DbModifierTypes.IntelligenceStonePull => ModifierType.Intelligence,
            DbModifierTypes.WillpowerStonePull => ModifierType.Willpower,
            DbModifierTypes.DexterityStonePull => ModifierType.Dexterity,
            DbModifierTypes.ThrustProficiencyModifier => ModifierType.Thrust,
            DbModifierTypes.ShootProficiencyModifier => ModifierType.Shoot,
            DbModifierTypes.ThrowRange => ModifierType.Throw,
            DbModifierTypes.CastProficiencyModifier => ModifierType.Cast,
            DbModifierTypes.DodgeGeneralDamageProficiency => ModifierType.Dodge,
            DbModifierTypes.ParryGeneralDamageProficiency => ModifierType.Parry,
            DbModifierTypes.EvadeGeneralDamageProficiency => ModifierType.Evade,
            DbModifierTypes.WardGeneralDamageProficiency => ModifierType.Ward,
            DbModifierTypes.DeflectGeneralDamageProficiency => ModifierType.Deflection,
            DbModifierTypes.ReserveWillPower => ModifierType.RWP,
            DbModifierTypes.ThrowStat => ModifierType.Throw,
            DbModifierTypes.ProjectStat => ModifierType.Project,
            DbModifierTypes.AgilityStonePull => ModifierType.Agility,
            DbModifierTypes.Project => ModifierType.Project,
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
            _ => throw new ArgumentOutOfRangeException(
                nameof(mapping),
                mapping.ModifierTypeId,
                null
            ),
        };
    }
}

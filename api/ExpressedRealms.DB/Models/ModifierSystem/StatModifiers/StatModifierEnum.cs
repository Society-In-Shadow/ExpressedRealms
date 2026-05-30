using Ardalis.SmartEnum;

namespace ExpressedRealms.DB.Models.ModifierSystem.StatModifiers;

public sealed class StatModifierEnum : SmartEnum<StatModifierEnum, int>
{
    private StatModifierEnum(string name, int id)
        : base(name, id) { }

    public static readonly StatModifierEnum Vitality = new("Vitality", 1);
    public static readonly StatModifierEnum Health = new("Health", 2);
    public static readonly StatModifierEnum Blood = new("Blood", 3);
    public static readonly StatModifierEnum Reaction = new("Reaction", 4);
    public static readonly StatModifierEnum Psyche = new("Psyche", 5);
    public static readonly StatModifierEnum RWP = new("RWP", 6);
    public static readonly StatModifierEnum Mortis = new("Mortis", 7);
    public static readonly StatModifierEnum Chi = new("Chi", 8);
    public static readonly StatModifierEnum Essence = new("Essence", 9);
    public static readonly StatModifierEnum Mana = new("Mana", 10);
    public static readonly StatModifierEnum Noumenon = new("Noumenon", 11);

    public static readonly StatModifierEnum Strike = new("Strike", 12);
    public static readonly StatModifierEnum Thrust = new("Thrust", 13);
    public static readonly StatModifierEnum Throw = new("Throw", 14);
    public static readonly StatModifierEnum Shoot = new("Shoot", 15);
    public static readonly StatModifierEnum Cast = new("Cast", 16);
    public static readonly StatModifierEnum Project = new("Project", 17);

    public static readonly StatModifierEnum Dodge = new("Dodge", 18);
    public static readonly StatModifierEnum Parry = new("Parry", 19);
    public static readonly StatModifierEnum EvadeThrow = new("Evade Throw", 20);
    public static readonly StatModifierEnum EvadeShoot = new("Evade Shoot", 21);
    public static readonly StatModifierEnum Ward = new("Ward", 22);
    public static readonly StatModifierEnum Deflect = new("Deflect", 23);

    public static readonly StatModifierEnum Motes = new("Prima / Void", 24);
    public static readonly StatModifierEnum WealthLevel = new("Wealth Level", 25);
}

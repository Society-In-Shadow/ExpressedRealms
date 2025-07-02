using Ardalis.SmartEnum;

namespace ExpressedRealms.Characters.Repository.Proficiencies.Enums;

public sealed class ModifierType : SmartEnum<ModifierType, byte>
{
    private ModifierType(string name, byte key)
        : base(name, key) { }

    public static readonly ModifierType Strength = new("Strength", 1);
    public static readonly ModifierType Dexterity = new("Dexterity", 2);
    public static readonly ModifierType Constitution = new("Constitution", 3);
    public static readonly ModifierType Intelligence = new("Intelligence", 4);
    public static readonly ModifierType Willpower = new("Willpower", 5);
    public static readonly ModifierType Agility = new("Agility", 6);

    public static readonly ModifierType HandToHandOffense = new("Hand-to-Hand Offense", 7);
    public static readonly ModifierType MeleeOffense = new("Melee Offense", 8);
    public static readonly ModifierType Marksmanship = new("Marksmanship", 9);
    public static readonly ModifierType ThrownWeapons = new("Thrown Weapons", 10);
    public static readonly ModifierType Spellcasting = new("Spellcasting", 11);
    public static readonly ModifierType Projection = new("Projection", 12);

    public static readonly ModifierType HandToHandDefense = new("Hand-to-Hand Defense", 13);
    public static readonly ModifierType MeleeDefense = new("Melee Defense", 14);
    public static readonly ModifierType Acrobatics = new("Acrobatics", 15);
    public static readonly ModifierType Spellwarding = new("Spellwarding", 16);
    public static readonly ModifierType Deflection = new("Deflection", 17);

    public static readonly ModifierType Strike = new("Strike", 18);
    public static readonly ModifierType Thrust = new("Thrust", 19);
    public static readonly ModifierType Throw = new("Throw", 20);
    public static readonly ModifierType Shoot = new("Shoot", 21);
    public static readonly ModifierType Cast = new("Cast", 22);
    public static readonly ModifierType Project = new("Project", 23);

    public static readonly ModifierType Dodge = new("Dodge", 24);
    public static readonly ModifierType Parry = new("Parry", 25);
    public static readonly ModifierType Evade = new("Evade", 26);
    public static readonly ModifierType Ward = new("Ward", 27);
    public static readonly ModifierType Deflect = new("Deflect", 28);
    public static readonly ModifierType RWP = new ("Reserve Willpower", 29);
    public static readonly ModifierType Mortis = new ("Mortis", 30);

    public override string ToString() => Name;
}

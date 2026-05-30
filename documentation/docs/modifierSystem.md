# Modifier System
The modifier system is how powers, advantages, disadvantages and progression paths affect the character's primary and 
secondary stats.

For each of those, any number of modifiers can be applied

## Modifier Fields / Use

- Modifier - It's the actual modifier, +1, +3, etc
- Modifier Type - List of modifiers that you can adjust
  - Vitality
  - Health
  - Blood
  - Reaction
  - Psyche
  - RWP
  - Mortis
  - Chi
  - Essence
  - Mana
  - Noumenon
  - Strike
  - Thrust
  - Throw
  - Shoot
  - Cast
  - Project
  - Dodge
  - Parry
  - Evade Throw
  - Evade Shoot
  - Ward
  - Deflect
  - Wealth Level
  - Prima / Void
- Scale with Level - Basically, take the modifier defined above and multiply it by the characters level
- Include Level 0 With Scale - ???
- Target Expression - This modifier should only apply to a specific expression (Common with Power Points)

## Location and Setup
The whole modifier setup can be found here:

[Proficiencies Folder](/api/ExpressedRealms.Characters.Repository/Proficiencies/)

## Adding New Modifiers
To add a new modifier:

1. Update [StatModifierEnum](api/ExpressedRealms.DB/Models/ModifierSystem/StatModifiers/StatModifierEnum.cs) with the new modifier type
2. Update [ProficiencyDtos.cs](api/ExpressedRealms.Characters.Repository/Proficiencies/Data/ProficiencyDtos.cs) to include the new modifier type
3. Update [ModifierType.cs](api/ExpressedRealms.Characters.Repository/Proficiencies/Enums/ModifierType.cs) to include the new modifier type
4. Update [ModifierConversions.cs](api/ExpressedRealms.Characters.Repository/Proficiencies/Utilities/ModiferConversions.cs) to include the new modifier type
5. Run Migration to apply the new modifier type to DB


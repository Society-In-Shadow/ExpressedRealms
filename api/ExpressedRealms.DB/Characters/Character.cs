using ExpressedRealms.DB.Characters.AssignedXp.AssignedXpMappingModels;
using ExpressedRealms.DB.Characters.xpTables;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Blessings.CharacterBlessingMappings;
using ExpressedRealms.DB.Models.Contacts;
using ExpressedRealms.DB.Models.Expressions.ExpressionSectionSetup;
using ExpressedRealms.DB.Models.Expressions.ExpressionSetup;
using ExpressedRealms.DB.Models.Expressions.ProgressionPathSetup.ProgressionPaths;
using ExpressedRealms.DB.Models.Knowledges.CharacterKnowledgeMappings;
using ExpressedRealms.DB.Models.Powers.CharacterPowerMappingSetup;
using ExpressedRealms.DB.Models.Skills;
using ExpressedRealms.DB.Models.Statistics;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.PlayerSetup;

namespace ExpressedRealms.DB.Characters;

public class Character : ISoftDelete
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Background { get; set; }
    public Guid PlayerId { get; set; }
    public int ExpressionId { get; set; }

    public byte AgilityId { get; set; }
    public byte ConstitutionId { get; set; }
    public byte DexterityId { get; set; }
    public byte StrengthId { get; set; }
    public byte IntelligenceId { get; set; }
    public byte WillpowerId { get; set; }

    public int? FactionId { get; set; }

    public int StatExperiencePoints { get; set; }

    public bool IsInCharacterCreation { get; set; }
    public bool IsPrimaryCharacter { get; set; }
    public int AssignedXp { get; set; }
    public int PlayerNumber { get; set; }

    public int? PrimaryProgressionId { get; set; }
    public int? SecondaryProgressionId { get; set; }
    public virtual ProgressionPath? PrimaryProgressionPath { get; set; }
    public virtual ProgressionPath? SecondaryProgressionPath { get; set; }

    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public virtual Player Player { get; set; } = null!;
    public virtual Expression Expression { get; set; } = null!;

    public virtual StatLevel AgilityStatLevel { get; set; } = null!;
    public virtual StatLevel ConstitutionStatLevel { get; set; } = null!;
    public virtual StatLevel DexterityStatLevel { get; set; } = null!;
    public virtual StatLevel StrengthStatLevel { get; set; } = null!;
    public virtual StatLevel IntelligenceStatLevel { get; set; } = null!;
    public virtual StatLevel WillpowerStatLevel { get; set; } = null!;
    public virtual ExpressionSection? FactionInfo { get; set; } = null!;
    public virtual List<CharacterSkillsMapping> CharacterSkillsMappings { get; set; } = null!;
    public virtual List<CharacterKnowledgeMapping> CharacterKnowledgeMappings { get; set; } = null!;
    public virtual List<CharacterPowerMapping> CharacterPowerMappings { get; set; } = null!;
    public virtual List<CharacterBlessingMapping> CharacterBlessingMappings { get; set; } = null!;
    public virtual List<CharacterXpMapping> CharacterXpMappings { get; set; } = null!;
    public virtual List<AssignedXpMapping> AssignedXpMappings { get; set; } = null!;
    public virtual ICollection<Contact> Contacts { get; set; } = new HashSet<Contact>();
}

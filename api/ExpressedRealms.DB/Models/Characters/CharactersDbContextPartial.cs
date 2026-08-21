using ExpressedRealms.DB.Models.Characters;
using ExpressedRealms.DB.Models.Characters.AssignedXP.AssignedXpMappingModels;
using ExpressedRealms.DB.Models.Characters.AssignedXP.AssignedXpTypeModels;
using ExpressedRealms.DB.Models.Characters.Audit;
using ExpressedRealms.DB.Models.Characters.CharacterStorage.CharacterStorageModels;
using ExpressedRealms.DB.Models.Characters.CharacterStorage.CharacterStorageModels.Audit;
using ExpressedRealms.DB.Models.Characters.XpTables;
using Microsoft.EntityFrameworkCore;

// ReSharper disable once CheckNamespace

namespace ExpressedRealms.DB;

public partial class ExpressedRealmsDbContext
{
    public DbSet<Character> Characters { get; set; }
    public DbSet<CharacterAuditTrail> CharacterAuditTrails { get; set; }
    public DbSet<CharacterXpView> CharacterXpViews { get; set; }
    public DbSet<CharacterXpMapping> CharacterXpMappings { get; set; }
    public DbSet<XpSectionType> XpSectionTypes { get; set; }
    public DbSet<AssignedXpMapping> AssignedXpMappings { get; set; }
    public DbSet<AssignedXpType> AssignedXpTypes { get; set; }
    public DbSet<CharacterStorageInfo> CharacterStorageInfos { get; set; }
    public DbSet<CharacterStorageInfoAuditTrail> CharacterStorageInfoTrails { get; set; }
}

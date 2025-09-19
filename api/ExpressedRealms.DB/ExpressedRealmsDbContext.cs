using System.Reflection;
using Audit.EntityFramework;
using ExpressedRealms.DB.Characters;
using ExpressedRealms.DB.Characters.xpTables;
using ExpressedRealms.DB.Configuration;
using ExpressedRealms.DB.Models.Skills;
using ExpressedRealms.DB.Models.Statistics;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.PlayerSetup;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.Roles;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserRoles;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.DB
{
    [AuditDbContext(Mode = AuditOptionMode.OptIn)]
    public partial class ExpressedRealmsDbContext
        : AuditIdentityDbContext<
            User,
            Role,
            string,
            IdentityUserClaim<string>,
            UserRole,
            IdentityUserLogin<string>,
            IdentityRoleClaim<string>,
            IdentityUserToken<string>
        >
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public ExpressedRealmsDbContext(DbContextOptions<ExpressedRealmsDbContext> options)
            : base(options)
        {
            SetupDatabaseAudit.SetupAudit();
        }

        public DbSet<Character> Characters { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<UserAuditTrail> UserAuditTrails { get; set; }
        public DbSet<PlayerAuditTrail> PlayerAuditTrails { get; set; }
        public DbSet<UserRoleAuditTrail> UserRoleAuditTrails { get; set; }
        public DbSet<StatType> StateTypes { get; set; }
        public DbSet<StatLevel> StatLevels { get; set; }
        public DbSet<StatDescriptionMapping> StatDescriptionMappings { get; set; }
        public DbSet<CharacterSkillsMapping> CharacterSkillsMappings { get; set; }
        public DbSet<ModifierType> ModifierTypes { get; set; }
        public DbSet<SkillLevel> SkillLevels { get; set; }
        public DbSet<SkillLevelBenefit> SkillLevelBenefits { get; set; }
        public DbSet<SkillSubType> SkillSubTypes { get; set; }
        public DbSet<SkillType> SkillTypes { get; set; }
        public DbSet<SkillLevelDescriptionMapping> SkillLevelDescriptionMappings { get; set; }
        public DbSet<CharacterXpMapping> CharacterXpMappings { get; set; }
        public DbSet<XpSectionType> XpSectionTypes { get; set; }
        public DbSet<CharacterXpView> CharacterXpViews { get; set; }
    }
}

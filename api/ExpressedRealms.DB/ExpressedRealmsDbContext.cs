using Audit.EntityFramework;
using ExpressedRealms.DB.Characters;
using ExpressedRealms.DB.Configuration;
using ExpressedRealms.DB.Models.Expressions.Configuration;
using ExpressedRealms.DB.Models.Knowledges.Configuration;
using ExpressedRealms.DB.Models.Powers.Configuration;
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
            builder.ApplyConfiguration(new CharacterConfiguration());
            builder.ApplyConfiguration(new PlayerConfiguration());

            builder.ApplyConfiguration(new UserAuditTrailConfiguration());
            builder.ApplyConfiguration(new PlayerAuditTrailConfiguration());
            builder.ApplyConfiguration(new UserRoleAuditTrailConfiguration());

            builder.ApplyConfiguration(new StatTypeConfiguration());
            builder.ApplyConfiguration(new StatLevelConfiguration());
            builder.ApplyConfiguration(new StatDescriptionMappingConfiguration());

            builder.ApplyConfiguration(new CharacterSkillsMappingConfiguration());
            builder.ApplyConfiguration(new ModifierTypeConfiguration());
            builder.ApplyConfiguration(new SkillLevelConfiguration());
            builder.ApplyConfiguration(new SkillLevelBenefitConfiguration());
            builder.ApplyConfiguration(new SkillSubTypeConfiguration());
            builder.ApplyConfiguration(new SkillTypeConfiguration());
            builder.ApplyConfiguration(new SkillLevelDescriptionMappingConfiguration());

            builder.AddPowerConfiguration();
            builder.AddExpressionConfiguration();
            builder.AddKnowledgeConfiguration();
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
    }
}

using Audit.EntityFramework;
using ExpressedRealms.DB.Characters;
using ExpressedRealms.DB.Configuration;
using ExpressedRealms.DB.Models.Expressions;
using ExpressedRealms.DB.Models.Expressions.ExpressionSectionSetup;
using ExpressedRealms.DB.Models.Expressions.ExpressionSetup;
using ExpressedRealms.DB.Models.Skills;
using ExpressedRealms.DB.Models.Statistics;
using ExpressedRealms.DB.UserProfile.PlayerDBModels;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.PlayerSetup;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.DB
{
    [AuditDbContext(Mode = AuditOptionMode.OptIn)]
    public class ExpressedRealmsDbContext : AuditIdentityDbContext<User>
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new CharacterConfiguration());
            builder.ApplyConfiguration(new PlayerConfiguration());
            builder.ApplyConfiguration(new ExpressionConfiguration());
            builder.ApplyConfiguration(new ExpressionSectionsConfiguration());
            builder.ApplyConfiguration(new ExpressionSectionTypeConfiguration());
            builder.ApplyConfiguration(new ExpressionPublishStatusConfiguration());

            builder.ApplyConfiguration(new ExpressionAuditTrailConfiguration());
            builder.ApplyConfiguration(new ExpressionSectionAuditTrailConfiguration());
            builder.ApplyConfiguration(new UserAuditTrailConfiguration());
            builder.ApplyConfiguration(new PlayerAuditTrailConfiguration());

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
        }

        public ExpressedRealmsDbContext(DbContextOptions<ExpressedRealmsDbContext> options)
            : base(options)
        {
            SetupDatabaseAudit.SetupAudit();
        }

        public DbSet<Character> Characters { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Expression> Expressions { get; set; }
        public DbSet<ExpressionSection> ExpressionSections { get; set; }
        public DbSet<ExpressionSectionType> ExpressionSectionTypes { get; set; }
        public DbSet<ExpressionPublishStatus> ExpressionPublishStatus { get; set; }

        public DbSet<ExpressionSectionAuditTrail> ExpressionSectionAuditTrails { get; set; }
        public DbSet<ExpressionAuditTrail> ExpressionAuditTrails { get; set; }
        public DbSet<UserAuditTrail> UserAuditTrails { get; set; }
        public DbSet<PlayerAuditTrail> PlayerAuditTrails { get; set; }

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

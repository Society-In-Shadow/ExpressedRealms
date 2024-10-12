using ExpressedRealms.DB.Characters;
using ExpressedRealms.DB.Models.Expressions;
using ExpressedRealms.DB.Models.Skills;
using ExpressedRealms.DB.Models.Statistics;
using ExpressedRealms.DB.UserProfile.PlayerDBModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.DB
{
    public class ExpressedRealmsDbContext : IdentityDbContext<IdentityUser>
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

            builder.ApplyConfiguration(new StatTypeConfiguration());
            builder.ApplyConfiguration(new StatLevelConfiguration());
            builder.ApplyConfiguration(new StatDescriptionMappingConfiguration());

            builder.ApplyConfiguration(new CharacterSkillsMappingConfiguration());
            builder.ApplyConfiguration(new ModifierTypeConfiguration());
            builder.ApplyConfiguration(new SkillLevelConfiguration());
            builder.ApplyConfiguration(new SkillLevelBenefitConfiguration());
            builder.ApplyConfiguration(new SkillSubTypeConfiguration());
            builder.ApplyConfiguration(new SkillTypeConfiguration());
        }

        public ExpressedRealmsDbContext(DbContextOptions<ExpressedRealmsDbContext> options)
            : base(options) { }

        public DbSet<Character> Characters { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Expression> Expressions { get; set; }
        public DbSet<ExpressionSection> ExpressionSections { get; set; }
        public DbSet<ExpressionSectionType> ExpressionSectionTypes { get; set; }
        public DbSet<ExpressionPublishStatus> ExpressionPublishStatus { get; set; }

        public DbSet<StatType> StateTypes { get; set; }
        public DbSet<StatLevel> StatLevels { get; set; }
        public DbSet<StatDescriptionMapping> StatDescriptionMappings { get; set; }

        public DbSet<CharacterSkillsMapping> CharacterSkillsMappings { get; set; }
        public DbSet<ModifierType> ModifierTypes { get; set; }
        public DbSet<SkillLevel> SkillLevels { get; set; }
        public DbSet<SkillLevelBenefit> SkillLevelBenefits { get; set; }
        public DbSet<SkillSubType> SkillSubTypes { get; set; }
        public DbSet<SkillType> SkillTypes { get; set; }
    }
}

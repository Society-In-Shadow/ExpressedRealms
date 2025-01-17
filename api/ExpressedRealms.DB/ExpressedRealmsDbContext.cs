using System.Text.Json;
using Audit.Core;
using Audit.EntityFramework;
using ExpressedRealms.DB.Characters;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Expressions;
using ExpressedRealms.DB.Models.Skills;
using ExpressedRealms.DB.Models.Statistics;
using ExpressedRealms.DB.UserProfile.PlayerDBModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.DB
{
    [AuditDbContext(Mode = AuditOptionMode.OptIn)]
    public class ExpressedRealmsDbContext : AuditIdentityDbContext<IdentityUser>
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
            builder.ApplyConfiguration(new ExpressionSectionAuditTrailConfiguration());

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
            : base(options)
        {
            Audit.Core.Configuration.Setup()
                .UseEntityFramework(x => x
                    .AuditTypeExplicitMapper(m => m
                        .Map<ExpressionSection, ExpressionSectionAuditTrail>((section, audit) =>
                        {
                            audit.SectionId = section.Id;
                            audit.ExpressionId = section.ExpressionId;
                            return true;
                        })
                        .AuditEntityAction<IAuditTable>((evt, entry, audit) =>
                        {
                            audit.Action = entry.Action;
                            audit.Timestamp = DateTime.UtcNow;
                            audit.UserId = evt.Environment.UserName;

                            var changes = new List<ChangedRecord>();
                            if (string.Compare(audit.Action, "insert", StringComparison.InvariantCultureIgnoreCase) == 0)
                            {
                                changes = entry.ColumnValues.Select(x => new ChangedRecord(x.Key, null, x.Value?.ToString()))
                                    .ToList();
                            }
                            else
                            {
                                changes = entry.Changes
                                    .Where(x =>
                                        x.NewValue == null
                                            ? x.OriginalValue != null
                                            : !x.NewValue.Equals(x.OriginalValue))
                                    .Select(x => new ChangedRecord(x.ColumnName, x.OriginalValue?.ToString(),
                                        x.NewValue?.ToString()))
                                    .ToList();
                            }
                            
                            audit.ChangedProperties = JsonSerializer.Serialize(changes);
                            
                            return true;
                        })
                    )
                    .IgnoreMatchedProperties(true));
        }

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

﻿using ExpressedRealms.DB.Characters;
using ExpressedRealms.DB.Models.Expressions;
using ExpressedRealms.DB.Statistics;
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
            builder.ApplyConfiguration(new StatTypeConfiguration());
        }

        public ExpressedRealmsDbContext(DbContextOptions<ExpressedRealmsDbContext> options)
            : base(options) { }

        public DbSet<Character> Characters { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Expression> Expressions { get; set; }
        public DbSet<ExpressionSection> ExpressionSections { get; set; }
        public DbSet<ExpressionSectionType> ExpressionSectionTypes { get; set; }
        public DbSet<StatType> StateType { get; set; }
    }
}

﻿// <auto-generated />
using System;
using ExpressedRealms.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    [DbContext(typeof(ExpressedRealmsDbContext))]
    [Migration("20240505200503_AddTotalXPCostToStatLevelTable")]
    partial class AddTotalXPCostToStatLevelTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ExpressedRealms.DB.Characters.Character", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<byte>("AgilityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint")
                        .HasDefaultValue((byte)1);

                    b.Property<string>("Background")
                        .HasColumnType("text");

                    b.Property<byte>("ConstitutionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint")
                        .HasDefaultValue((byte)1);

                    b.Property<DateTimeOffset?>("DeletedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<byte>("DexterityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint")
                        .HasDefaultValue((byte)1);

                    b.Property<int>("ExpressionId")
                        .HasColumnType("integer");

                    b.Property<byte>("IntelligenceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint")
                        .HasDefaultValue((byte)1);

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<Guid>("PlayerId")
                        .HasColumnType("uuid");

                    b.Property<int>("StatExperiencePoints")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(72);

                    b.Property<byte>("StrengthId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint")
                        .HasDefaultValue((byte)1);

                    b.Property<byte>("WillpowerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint")
                        .HasDefaultValue((byte)1);

                    b.HasKey("Id");

                    b.HasIndex("AgilityId");

                    b.HasIndex("ConstitutionId");

                    b.HasIndex("DexterityId");

                    b.HasIndex("ExpressionId");

                    b.HasIndex("IntelligenceId");

                    b.HasIndex("PlayerId");

                    b.HasIndex("StrengthId");

                    b.HasIndex("WillpowerId");

                    b.ToTable("Characters");
                });

            modelBuilder.Entity("ExpressedRealms.DB.Models.Expressions.Expression", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("NavMenuImage")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ShortDescription")
                        .IsRequired()
                        .HasMaxLength(125)
                        .HasColumnType("character varying(125)");

                    b.HasKey("Id");

                    b.ToTable("Expressions", (string)null);
                });

            modelBuilder.Entity("ExpressedRealms.DB.Models.Expressions.ExpressionSection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ExpressionId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<int?>("ParentId")
                        .HasColumnType("integer");

                    b.Property<int>("SectionTypeId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ExpressionId");

                    b.HasIndex("ParentId");

                    b.HasIndex("SectionTypeId");

                    b.ToTable("ExpressionSections", (string)null);
                });

            modelBuilder.Entity("ExpressedRealms.DB.Models.Expressions.ExpressionSectionType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.ToTable("ExpressionSectionTypes", (string)null);
                });

            modelBuilder.Entity("ExpressedRealms.DB.Models.Statistics.StatDescriptionMapping", b =>
                {
                    b.Property<byte>("StatTypeId")
                        .HasColumnType("smallint");

                    b.Property<byte>("StatLevelId")
                        .HasColumnType("smallint");

                    b.Property<string>("ReasonableExpectation")
                        .IsRequired()
                        .HasMaxLength(400)
                        .HasColumnType("character varying(400)");

                    b.HasKey("StatTypeId", "StatLevelId");

                    b.HasIndex("StatLevelId");

                    b.ToTable("StatDescriptionMappings");
                });

            modelBuilder.Entity("ExpressedRealms.DB.Models.Statistics.StatLevel", b =>
                {
                    b.Property<byte>("Id")
                        .HasColumnType("smallint");

                    b.Property<int>("Bonus")
                        .HasColumnType("integer");

                    b.Property<int>("TotalXPCost")
                        .HasColumnType("integer");

                    b.Property<int>("XPCost")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("StatLevels");
                });

            modelBuilder.Entity("ExpressedRealms.DB.Models.Statistics.StatType", b =>
                {
                    b.Property<byte>("Id")
                        .HasColumnType("smallint");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("character varying(15)");

                    b.Property<string>("ShortName")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)");

                    b.HasKey("Id");

                    b.ToTable("StateTypes");
                });

            modelBuilder.Entity("ExpressedRealms.DB.UserProfile.PlayerDBModels.Player", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("character varying(15)");

                    b.Property<short>("PlayerNumber")
                        .HasColumnType("smallint");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("character varying(2)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Players");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("character varying(13)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityUser");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("ExpressedRealms.DB.UserProfile.PlayerDBModels.User", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityUser");

                    b.HasDiscriminator().HasValue("User");
                });

            modelBuilder.Entity("ExpressedRealms.DB.Characters.Character", b =>
                {
                    b.HasOne("ExpressedRealms.DB.Models.Statistics.StatLevel", "AgilityStatLevel")
                        .WithMany("CharacterAgility")
                        .HasForeignKey("AgilityId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ExpressedRealms.DB.Models.Statistics.StatLevel", "ConstitutionStatLevel")
                        .WithMany("CharacterConstitution")
                        .HasForeignKey("ConstitutionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ExpressedRealms.DB.Models.Statistics.StatLevel", "DexterityStatLevel")
                        .WithMany("CharacterDexterity")
                        .HasForeignKey("DexterityId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ExpressedRealms.DB.Models.Expressions.Expression", "Expression")
                        .WithMany("Characters")
                        .HasForeignKey("ExpressionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ExpressedRealms.DB.Models.Statistics.StatLevel", "IntelligenceStatLevel")
                        .WithMany("CharacterIntelligence")
                        .HasForeignKey("IntelligenceId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ExpressedRealms.DB.UserProfile.PlayerDBModels.Player", "Player")
                        .WithMany("Characters")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ExpressedRealms.DB.Models.Statistics.StatLevel", "StrengthStatLevel")
                        .WithMany("CharacterStrength")
                        .HasForeignKey("StrengthId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ExpressedRealms.DB.Models.Statistics.StatLevel", "WillpowerStatLevel")
                        .WithMany("CharacterWillpower")
                        .HasForeignKey("WillpowerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("AgilityStatLevel");

                    b.Navigation("ConstitutionStatLevel");

                    b.Navigation("DexterityStatLevel");

                    b.Navigation("Expression");

                    b.Navigation("IntelligenceStatLevel");

                    b.Navigation("Player");

                    b.Navigation("StrengthStatLevel");

                    b.Navigation("WillpowerStatLevel");
                });

            modelBuilder.Entity("ExpressedRealms.DB.Models.Expressions.ExpressionSection", b =>
                {
                    b.HasOne("ExpressedRealms.DB.Models.Expressions.Expression", "Expression")
                        .WithMany("ExpressionSections")
                        .HasForeignKey("ExpressionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ExpressedRealms.DB.Models.Expressions.ExpressionSection", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ExpressedRealms.DB.Models.Expressions.ExpressionSectionType", "SectionType")
                        .WithMany("ExpressionSections")
                        .HasForeignKey("SectionTypeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Expression");

                    b.Navigation("Parent");

                    b.Navigation("SectionType");
                });

            modelBuilder.Entity("ExpressedRealms.DB.Models.Statistics.StatDescriptionMapping", b =>
                {
                    b.HasOne("ExpressedRealms.DB.Models.Statistics.StatLevel", "StatLevel")
                        .WithMany("StatDescriptionMappings")
                        .HasForeignKey("StatLevelId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ExpressedRealms.DB.Models.Statistics.StatType", "StatType")
                        .WithMany("StatDescriptionMappings")
                        .HasForeignKey("StatTypeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("StatLevel");

                    b.Navigation("StatType");
                });

            modelBuilder.Entity("ExpressedRealms.DB.UserProfile.PlayerDBModels.Player", b =>
                {
                    b.HasOne("ExpressedRealms.DB.UserProfile.PlayerDBModels.User", "User")
                        .WithOne("Player")
                        .HasForeignKey("ExpressedRealms.DB.UserProfile.PlayerDBModels.Player", "UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ExpressedRealms.DB.Models.Expressions.Expression", b =>
                {
                    b.Navigation("Characters");

                    b.Navigation("ExpressionSections");
                });

            modelBuilder.Entity("ExpressedRealms.DB.Models.Expressions.ExpressionSection", b =>
                {
                    b.Navigation("Children");
                });

            modelBuilder.Entity("ExpressedRealms.DB.Models.Expressions.ExpressionSectionType", b =>
                {
                    b.Navigation("ExpressionSections");
                });

            modelBuilder.Entity("ExpressedRealms.DB.Models.Statistics.StatLevel", b =>
                {
                    b.Navigation("CharacterAgility");

                    b.Navigation("CharacterConstitution");

                    b.Navigation("CharacterDexterity");

                    b.Navigation("CharacterIntelligence");

                    b.Navigation("CharacterStrength");

                    b.Navigation("CharacterWillpower");

                    b.Navigation("StatDescriptionMappings");
                });

            modelBuilder.Entity("ExpressedRealms.DB.Models.Statistics.StatType", b =>
                {
                    b.Navigation("StatDescriptionMappings");
                });

            modelBuilder.Entity("ExpressedRealms.DB.UserProfile.PlayerDBModels.Player", b =>
                {
                    b.Navigation("Characters");
                });

            modelBuilder.Entity("ExpressedRealms.DB.UserProfile.PlayerDBModels.User", b =>
                {
                    b.Navigation("Player")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}

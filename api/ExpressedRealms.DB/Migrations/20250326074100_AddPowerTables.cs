using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddPowerTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "power_activation_timing_type",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "smallint", nullable: false),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_power_activation_timing_type", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "power_area_of_effect_type",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "smallint", nullable: false),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_power_area_of_effect_type", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "power_category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_power_category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "power_duration",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "smallint", nullable: false),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_power_duration", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "power_level",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Xp = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_power_level", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "power",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    LevelId = table.Column<int>(type: "integer", nullable: false),
                    AreaOfEffectTypeId = table.Column<byte>(type: "smallint", nullable: false),
                    ActivationTimingTypeId = table.Column<byte>(type: "smallint", nullable: false),
                    DurationId = table.Column<byte>(type: "smallint", nullable: false),
                    PowerDurationId = table.Column<byte>(type: "smallint", nullable: false),
                    ExpressionId = table.Column<int>(type: "integer", nullable: false),
                    IsPowerUse = table.Column<bool>(type: "boolean", nullable: false),
                    GameMechanicEffect = table.Column<string>(type: "text", nullable: true),
                    Limitation = table.Column<string>(type: "text", nullable: true),
                    OtherFields = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_power", x => x.Id);
                    table.ForeignKey(
                        name: "FK_power_Expressions_ExpressionId",
                        column: x => x.ExpressionId,
                        principalTable: "Expressions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_power_power_activation_timing_type_DurationId",
                        column: x => x.DurationId,
                        principalTable: "power_activation_timing_type",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_power_power_area_of_effect_type_AreaOfEffectTypeId",
                        column: x => x.AreaOfEffectTypeId,
                        principalTable: "power_area_of_effect_type",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_power_power_duration_PowerDurationId",
                        column: x => x.PowerDurationId,
                        principalTable: "power_duration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_power_power_level_LevelId",
                        column: x => x.LevelId,
                        principalTable: "power_level",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "power_category_mapping",
                columns: table => new
                {
                    PowerId = table.Column<int>(type: "integer", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_power_category_mapping", x => new { x.PowerId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_power_category_mapping_power_PowerId",
                        column: x => x.PowerId,
                        principalTable: "power",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_power_category_mapping_power_category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "power_category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "power_prerequisites",
                columns: table => new
                {
                    ParentPowerId = table.Column<int>(type: "integer", nullable: false),
                    ChildPowerId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_power_prerequisites", x => new { x.ParentPowerId, x.ChildPowerId });
                    table.ForeignKey(
                        name: "FK_power_prerequisites_power_ChildPowerId",
                        column: x => x.ChildPowerId,
                        principalTable: "power",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_power_prerequisites_power_ParentPowerId",
                        column: x => x.ParentPowerId,
                        principalTable: "power",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_power_AreaOfEffectTypeId",
                table: "power",
                column: "AreaOfEffectTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_power_DurationId",
                table: "power",
                column: "DurationId");

            migrationBuilder.CreateIndex(
                name: "IX_power_ExpressionId",
                table: "power",
                column: "ExpressionId");

            migrationBuilder.CreateIndex(
                name: "IX_power_LevelId",
                table: "power",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_power_PowerDurationId",
                table: "power",
                column: "PowerDurationId");

            migrationBuilder.CreateIndex(
                name: "IX_power_category_mapping_CategoryId",
                table: "power_category_mapping",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_power_prerequisites_ChildPowerId",
                table: "power_prerequisites",
                column: "ChildPowerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "power_category_mapping");

            migrationBuilder.DropTable(
                name: "power_prerequisites");

            migrationBuilder.DropTable(
                name: "power_category");

            migrationBuilder.DropTable(
                name: "power");

            migrationBuilder.DropTable(
                name: "power_activation_timing_type");

            migrationBuilder.DropTable(
                name: "power_area_of_effect_type");

            migrationBuilder.DropTable(
                name: "power_duration");

            migrationBuilder.DropTable(
                name: "power_level");
        }
    }
}

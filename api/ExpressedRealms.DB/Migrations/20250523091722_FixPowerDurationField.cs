using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class FixPowerDurationField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_power_power_activation_timing_type_DurationId",
                table: "power");

            migrationBuilder.DropForeignKey(
                name: "FK_power_power_duration_PowerDurationId",
                table: "power");

            migrationBuilder.DropIndex(
                name: "IX_power_PowerDurationId",
                table: "power");

            migrationBuilder.DropColumn(
                name: "PowerDurationId",
                table: "power");

            migrationBuilder.CreateIndex(
                name: "IX_power_ActivationTimingTypeId",
                table: "power",
                column: "ActivationTimingTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_power_power_activation_timing_type_ActivationTimingTypeId",
                table: "power",
                column: "ActivationTimingTypeId",
                principalTable: "power_activation_timing_type",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_power_power_duration_DurationId",
                table: "power",
                column: "DurationId",
                principalTable: "power_duration",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_power_power_activation_timing_type_ActivationTimingTypeId",
                table: "power");

            migrationBuilder.DropForeignKey(
                name: "FK_power_power_duration_DurationId",
                table: "power");

            migrationBuilder.DropIndex(
                name: "IX_power_ActivationTimingTypeId",
                table: "power");

            migrationBuilder.AddColumn<byte>(
                name: "PowerDurationId",
                table: "power",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.CreateIndex(
                name: "IX_power_PowerDurationId",
                table: "power",
                column: "PowerDurationId");

            migrationBuilder.AddForeignKey(
                name: "FK_power_power_activation_timing_type_DurationId",
                table: "power",
                column: "DurationId",
                principalTable: "power_activation_timing_type",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_power_power_duration_PowerDurationId",
                table: "power",
                column: "PowerDurationId",
                principalTable: "power_duration",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

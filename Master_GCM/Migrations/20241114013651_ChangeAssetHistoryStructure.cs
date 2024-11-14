using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Master_GCM.Migrations
{
    /// <inheritdoc />
    public partial class ChangeAssetHistoryStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DEPARTMENT",
                table: "TRN_HIST_ASSET");

            migrationBuilder.DropColumn(
                name: "DIRECTORATE",
                table: "TRN_HIST_ASSET");

            migrationBuilder.DropColumn(
                name: "NAME",
                table: "TRN_HIST_ASSET");

            migrationBuilder.DropColumn(
                name: "POSITION",
                table: "TRN_HIST_ASSET");

            migrationBuilder.DropColumn(
                name: "UNIT",
                table: "TRN_HIST_ASSET");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DEPARTMENT",
                table: "TRN_HIST_ASSET",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DIRECTORATE",
                table: "TRN_HIST_ASSET",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NAME",
                table: "TRN_HIST_ASSET",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "POSITION",
                table: "TRN_HIST_ASSET",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UNIT",
                table: "TRN_HIST_ASSET",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}

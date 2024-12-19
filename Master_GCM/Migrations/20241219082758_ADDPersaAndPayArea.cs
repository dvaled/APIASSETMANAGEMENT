using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Master_GCM.Migrations
{
    /// <inheritdoc />
    public partial class ADDPersaAndPayArea : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "STATUS",
                table: "TRN_HIST_ASSET",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "ASSETBRAND",
                table: "TRN_ASSET",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "PAYAREA",
                table: "MST_EMPLOYEE",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PERSA",
                table: "MST_EMPLOYEE",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "STATUS",
                table: "TRN_HIST_ASSET");

            migrationBuilder.DropColumn(
                name: "PAYAREA",
                table: "MST_EMPLOYEE");

            migrationBuilder.DropColumn(
                name: "PERSA",
                table: "MST_EMPLOYEE");

            migrationBuilder.AlterColumn<string>(
                name: "ASSETBRAND",
                table: "TRN_ASSET",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}

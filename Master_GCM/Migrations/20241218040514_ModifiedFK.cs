using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Master_GCM.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TRN_ASSET_MST_EMPLOYEE_NIPP",
                table: "TRN_ASSET");

            migrationBuilder.DropForeignKey(
                name: "FK_TRN_HIST_ASSET_MST_EMPLOYEE_NIPP",
                table: "TRN_HIST_ASSET");

            migrationBuilder.DropIndex(
                name: "IX_TRN_HIST_ASSET_NIPP",
                table: "TRN_HIST_ASSET");

            migrationBuilder.DropIndex(
                name: "IX_TRN_ASSET_NIPP",
                table: "TRN_ASSET");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TRN_HIST_ASSET_NIPP",
                table: "TRN_HIST_ASSET",
                column: "NIPP");

            migrationBuilder.CreateIndex(
                name: "IX_TRN_ASSET_NIPP",
                table: "TRN_ASSET",
                column: "NIPP");

            migrationBuilder.AddForeignKey(
                name: "FK_TRN_ASSET_MST_EMPLOYEE_NIPP",
                table: "TRN_ASSET",
                column: "NIPP",
                principalTable: "MST_EMPLOYEE",
                principalColumn: "NIPP");

            migrationBuilder.AddForeignKey(
                name: "FK_TRN_HIST_ASSET_MST_EMPLOYEE_NIPP",
                table: "TRN_HIST_ASSET",
                column: "NIPP",
                principalTable: "MST_EMPLOYEE",
                principalColumn: "NIPP");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Master_GCM.Migrations
{
    /// <inheritdoc />
    public partial class maintenance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NOTES",
                table: "TRN_HIST_MAINTENANCE",
                newName: "NOTESSPAREPART");

            migrationBuilder.AddColumn<string>(
                name: "NOTESACTION",
                table: "TRN_HIST_MAINTENANCE",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NOTESRESULT",
                table: "TRN_HIST_MAINTENANCE",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NOTESACTION",
                table: "TRN_HIST_MAINTENANCE");

            migrationBuilder.DropColumn(
                name: "NOTESRESULT",
                table: "TRN_HIST_MAINTENANCE");

            migrationBuilder.RenameColumn(
                name: "NOTESSPAREPART",
                table: "TRN_HIST_MAINTENANCE",
                newName: "NOTES");
        }
    }
}

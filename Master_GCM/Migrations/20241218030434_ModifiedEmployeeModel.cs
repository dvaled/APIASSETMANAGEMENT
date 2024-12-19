using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Master_GCM.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedEmployeeModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MST_USER");

            migrationBuilder.DropColumn(
                name: "ACTIVE",
                table: "MST_EMPLOYEE");

            migrationBuilder.DropColumn(
                name: "DEPARTMENT",
                table: "MST_EMPLOYEE");

            migrationBuilder.DropColumn(
                name: "DIRECTORATE",
                table: "MST_EMPLOYEE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ACTIVE",
                table: "MST_EMPLOYEE",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DEPARTMENT",
                table: "MST_EMPLOYEE",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DIRECTORATE",
                table: "MST_EMPLOYEE",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "MST_USER",
                columns: table => new
                {
                    NIPP = table.Column<string>(type: "text", nullable: false),
                    ACTIVE = table.Column<string>(type: "text", nullable: false),
                    DEPARTMENT = table.Column<string>(type: "text", nullable: false),
                    DIRECTORATE = table.Column<string>(type: "text", nullable: false),
                    NAME = table.Column<string>(type: "text", nullable: false),
                    PASSWORD = table.Column<string>(type: "text", nullable: false),
                    POSITION = table.Column<string>(type: "text", nullable: false),
                    ROLE = table.Column<string>(type: "text", nullable: false),
                    UNIT = table.Column<string>(type: "text", nullable: false),
                    USER_PICTURE = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MST_USER", x => x.NIPP);
                });
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Master_GCM.Migrations
{
    /// <inheritdoc />
    public partial class AddPurchaseDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "PURCHASEDATE",
                table: "TRN_ASSET",
                type: "date",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PURCHASEDATE",
                table: "TRN_ASSET");
        }
    }
}

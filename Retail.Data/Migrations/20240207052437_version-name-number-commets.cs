using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Retail.Data.Migrations
{
    public partial class versionnamenumbercommets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "StoreData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VersionName",
                table: "StoreData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Version");

            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "Store",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comments",
                table: "StoreData");

            migrationBuilder.DropColumn(
                name: "VersionName",
                table: "StoreData");

            migrationBuilder.DropColumn(
                name: "Comments",
                table: "Store");
        }
    }
}

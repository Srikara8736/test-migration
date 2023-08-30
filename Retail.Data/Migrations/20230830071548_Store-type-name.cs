using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Retail.Data.Migrations
{
    public partial class Storetypename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Store_StoreTypes_StoreTypeId",
                table: "Store");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StoreTypes",
                table: "StoreTypes");

            migrationBuilder.RenameTable(
                name: "StoreTypes",
                newName: "StoreType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StoreType",
                table: "StoreType",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Store_StoreType_StoreTypeId",
                table: "Store",
                column: "StoreTypeId",
                principalTable: "StoreType",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Store_StoreType_StoreTypeId",
                table: "Store");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StoreType",
                table: "StoreType");

            migrationBuilder.RenameTable(
                name: "StoreType",
                newName: "StoreTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StoreTypes",
                table: "StoreTypes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Store_StoreTypes_StoreTypeId",
                table: "Store",
                column: "StoreTypeId",
                principalTable: "StoreTypes",
                principalColumn: "Id");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Retail.Data.Migrations
{
    public partial class Storetypeimg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "StoreTypeId",
                table: "Store",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StoreTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    BackgroundImage = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Store_StoreTypeId",
                table: "Store",
                column: "StoreTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Store_StoreTypes_StoreTypeId",
                table: "Store",
                column: "StoreTypeId",
                principalTable: "StoreTypes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Store_StoreTypes_StoreTypeId",
                table: "Store");

            migrationBuilder.DropTable(
                name: "StoreTypes");

            migrationBuilder.DropIndex(
                name: "IX_Store_StoreTypeId",
                table: "Store");

            migrationBuilder.DropColumn(
                name: "StoreTypeId",
                table: "Store");
        }
    }
}

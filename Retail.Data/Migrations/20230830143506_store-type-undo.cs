using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Retail.Data.Migrations
{
    public partial class storetypeundo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Store_StoreType_StoreTypeId",
                table: "Store");

            migrationBuilder.DropTable(
                name: "StoreType");

            migrationBuilder.DropIndex(
                name: "IX_Store_StoreTypeId",
                table: "Store");

            migrationBuilder.DropColumn(
                name: "StoreTypeId",
                table: "Store");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "StoreTypeId",
                table: "Store",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StoreType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BackgroundImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Store_StoreTypeId",
                table: "Store",
                column: "StoreTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Store_StoreType_StoreTypeId",
                table: "Store",
                column: "StoreTypeId",
                principalTable: "StoreType",
                principalColumn: "Id");
        }
    }
}

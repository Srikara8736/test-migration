using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Retail.Data.Migrations
{
    public partial class storedataadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "StoreDataId",
                table: "StoreDocument",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("65B2B5DC-01A0-422C-3501-08DBE45F711E"));

            migrationBuilder.CreateIndex(
                name: "IX_StoreDocument_StoreDataId",
                table: "StoreDocument",
                column: "StoreDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_StoreDocument_StoreData_StoreDataId",
                table: "StoreDocument",
                column: "StoreDataId",
                principalTable: "StoreData",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StoreDocument_StoreData_StoreDataId",
                table: "StoreDocument");

            migrationBuilder.DropIndex(
                name: "IX_StoreDocument_StoreDataId",
                table: "StoreDocument");

            migrationBuilder.DropColumn(
                name: "StoreDataId",
                table: "StoreDocument");
        }
    }
}

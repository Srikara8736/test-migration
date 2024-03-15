using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Retail.Data.Migrations
{
    public partial class storedataiddrawing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "StoreDataId",
                table: "DrawingList",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_DrawingList_StoreDataId",
                table: "DrawingList",
                column: "StoreDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_DrawingList_StoreData_StoreDataId",
                table: "DrawingList",
                column: "StoreDataId",
                principalTable: "StoreData",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DrawingList_StoreData_StoreDataId",
                table: "DrawingList");

            migrationBuilder.DropIndex(
                name: "IX_DrawingList_StoreDataId",
                table: "DrawingList");

            migrationBuilder.DropColumn(
                name: "StoreDataId",
                table: "DrawingList");
        }
    }
}

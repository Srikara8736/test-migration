using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Retail.Data.Migrations
{
    public partial class cadtype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CadFileTypeId",
                table: "StoreSpace",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("496ad282-71a1-4688-ba81-0a1ea6c9021e"));

            migrationBuilder.AddColumn<Guid>(
                name: "CadFileTypeId",
                table: "StoreData",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("496ad282-71a1-4688-ba81-0a1ea6c9021e"));

            migrationBuilder.CreateIndex(
                name: "IX_StoreSpace_CadFileTypeId",
                table: "StoreSpace",
                column: "CadFileTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreData_CadFileTypeId",
                table: "StoreData",
                column: "CadFileTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_StoreData_CodeMaster_CadFileTypeId",
                table: "StoreData",
                column: "CadFileTypeId",
                principalTable: "CodeMaster",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StoreSpace_CodeMaster_CadFileTypeId",
                table: "StoreSpace",
                column: "CadFileTypeId",
                principalTable: "CodeMaster",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StoreData_CodeMaster_CadFileTypeId",
                table: "StoreData");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreSpace_CodeMaster_CadFileTypeId",
                table: "StoreSpace");

            migrationBuilder.DropIndex(
                name: "IX_StoreSpace_CadFileTypeId",
                table: "StoreSpace");

            migrationBuilder.DropIndex(
                name: "IX_StoreData_CadFileTypeId",
                table: "StoreData");

            migrationBuilder.DropColumn(
                name: "CadFileTypeId",
                table: "StoreSpace");

            migrationBuilder.DropColumn(
                name: "CadFileTypeId",
                table: "StoreData");
        }
    }
}

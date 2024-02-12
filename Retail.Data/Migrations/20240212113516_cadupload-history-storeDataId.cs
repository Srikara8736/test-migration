using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Retail.Data.Migrations
{
    public partial class caduploadhistorystoreDataId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "StoreDataId",
                table: "CadUploadHistory",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CadUploadHistory_StoreDataId",
                table: "CadUploadHistory",
                column: "StoreDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_CadUploadHistory_StoreData_StoreDataId",
                table: "CadUploadHistory",
                column: "StoreDataId",
                principalTable: "StoreData",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CadUploadHistory_StoreData_StoreDataId",
                table: "CadUploadHistory");

            migrationBuilder.DropIndex(
                name: "IX_CadUploadHistory_StoreDataId",
                table: "CadUploadHistory");

            migrationBuilder.DropColumn(
                name: "StoreDataId",
                table: "CadUploadHistory");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Retail.Data.Migrations
{
    public partial class customerbackgroundimage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BackgroundImageId",
                table: "Customer",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customer_BackgroundImageId",
                table: "Customer",
                column: "BackgroundImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_CodeMaster_BackgroundImageId",
                table: "Customer",
                column: "BackgroundImageId",
                principalTable: "CodeMaster",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_CodeMaster_BackgroundImageId",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Customer_BackgroundImageId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "BackgroundImageId",
                table: "Customer");
        }
    }
}

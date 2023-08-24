using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Retail.Data.Migrations
{
    public partial class locationgraph : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LogoImageId",
                table: "Customer",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Latitude",
                table: "Address",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Longitude",
                table: "Address",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customer_LogoImageId",
                table: "Customer",
                column: "LogoImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Image_LogoImageId",
                table: "Customer",
                column: "LogoImageId",
                principalTable: "Image",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Image_LogoImageId",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Customer_LogoImageId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "LogoImageId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Address");
        }
    }
}

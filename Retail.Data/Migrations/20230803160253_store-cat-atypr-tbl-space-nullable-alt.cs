using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Retail.Data.Migrations
{
    public partial class storecatatyprtblspacenullablealt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SpaceId",
                table: "StoreCategoryAreaTypeGroup",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StoreCategoryAreaTypeGroup_SpaceId",
                table: "StoreCategoryAreaTypeGroup",
                column: "SpaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_StoreCategoryAreaTypeGroup_Space_SpaceId",
                table: "StoreCategoryAreaTypeGroup",
                column: "SpaceId",
                principalTable: "Space",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StoreCategoryAreaTypeGroup_Space_SpaceId",
                table: "StoreCategoryAreaTypeGroup");

            migrationBuilder.DropIndex(
                name: "IX_StoreCategoryAreaTypeGroup_SpaceId",
                table: "StoreCategoryAreaTypeGroup");

            migrationBuilder.DropColumn(
                name: "SpaceId",
                table: "StoreCategoryAreaTypeGroup");
        }
    }
}

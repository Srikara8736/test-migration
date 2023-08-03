using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Retail.Data.Migrations
{
    public partial class categorytblareatypealt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_AreaType_AreaTypeId",
                table: "Category");

            migrationBuilder.AlterColumn<Guid>(
                name: "AreaTypeId",
                table: "Category",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_AreaType_AreaTypeId",
                table: "Category",
                column: "AreaTypeId",
                principalTable: "AreaType",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_AreaType_AreaTypeId",
                table: "Category");

            migrationBuilder.AlterColumn<Guid>(
                name: "AreaTypeId",
                table: "Category",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Category_AreaType_AreaTypeId",
                table: "Category",
                column: "AreaTypeId",
                principalTable: "AreaType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

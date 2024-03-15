using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Retail.Data.Migrations
{
    public partial class statusmodDrawingtbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DrawingList_CodeMaster_StatusId",
                table: "DrawingList");

            //migrationBuilder.DropIndex(
            //    name: "IX_DrawingList_StatusId",
            //    table: "DrawingList");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "DrawingList");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "DrawingList",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "DrawingList");

            migrationBuilder.AddColumn<Guid>(
                name: "StatusId",
                table: "DrawingList",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_DrawingList_StatusId",
                table: "DrawingList",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_DrawingList_CodeMaster_StatusId",
                table: "DrawingList",
                column: "StatusId",
                principalTable: "CodeMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

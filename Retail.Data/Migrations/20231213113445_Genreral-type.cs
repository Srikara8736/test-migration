using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Retail.Data.Migrations
{
    public partial class Genreraltype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GeneralListTypeData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Koncept1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Koncept2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Realm2 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RealPercentage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralListTypeData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralListTypeData_Store_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Store",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GeneralListTypeData_StoreId",
                table: "GeneralListTypeData",
                column: "StoreId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeneralListTypeData");
        }
    }
}

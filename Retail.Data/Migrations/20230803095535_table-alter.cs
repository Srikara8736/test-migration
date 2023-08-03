using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Retail.Data.Migrations
{
    public partial class tablealter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryAreaTypeGroup");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "User");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Customer",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "StoreCategoryAreaTypeGroup",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AreaTypeGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreCategoryAreaTypeGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreCategoryAreaTypeGroup_AreaTypeGroup_AreaTypeGroupId",
                        column: x => x.AreaTypeGroupId,
                        principalTable: "AreaTypeGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoreCategoryAreaTypeGroup_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoreCategoryAreaTypeGroup_Store_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Store",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StoreCategoryAreaTypeGroup_AreaTypeGroupId",
                table: "StoreCategoryAreaTypeGroup",
                column: "AreaTypeGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreCategoryAreaTypeGroup_CategoryId",
                table: "StoreCategoryAreaTypeGroup",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreCategoryAreaTypeGroup_StoreId",
                table: "StoreCategoryAreaTypeGroup",
                column: "StoreId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoreCategoryAreaTypeGroup");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Customer");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "CategoryAreaTypeGroup",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AreaTypeGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryAreaTypeGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryAreaTypeGroup_AreaTypeGroup_AreaTypeGroupId",
                        column: x => x.AreaTypeGroupId,
                        principalTable: "AreaTypeGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryAreaTypeGroup_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryAreaTypeGroup_Store_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Store",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryAreaTypeGroup_AreaTypeGroupId",
                table: "CategoryAreaTypeGroup",
                column: "AreaTypeGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryAreaTypeGroup_CategoryId",
                table: "CategoryAreaTypeGroup",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryAreaTypeGroup_StoreId",
                table: "CategoryAreaTypeGroup",
                column: "StoreId");
        }
    }
}

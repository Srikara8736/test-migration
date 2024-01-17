using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Retail.Data.Migrations
{
    public partial class catspace : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CadStoreCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UploadHistoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StoreDataId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CadTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CadStoreCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CadStoreCategory_CadUploadHistory_UploadHistoryId",
                        column: x => x.UploadHistoryId,
                        principalTable: "CadUploadHistory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CadStoreCategory_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CadStoreCategory_CodeMaster_CadTypeId",
                        column: x => x.CadTypeId,
                        principalTable: "CodeMaster",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CadStoreCategory_Store_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Store",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CadStoreCategory_StoreData_StoreDataId",
                        column: x => x.StoreDataId,
                        principalTable: "StoreData",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CadStoreSpace",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UploadHistoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SpaceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StoreDataId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CadTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CadStoreSpace", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CadStoreSpace_CadUploadHistory_UploadHistoryId",
                        column: x => x.UploadHistoryId,
                        principalTable: "CadUploadHistory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CadStoreSpace_CodeMaster_CadTypeId",
                        column: x => x.CadTypeId,
                        principalTable: "CodeMaster",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CadStoreSpace_Space_SpaceId",
                        column: x => x.SpaceId,
                        principalTable: "Space",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CadStoreSpace_Store_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Store",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CadStoreSpace_StoreData_StoreDataId",
                        column: x => x.StoreDataId,
                        principalTable: "StoreData",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CadStoreCategory_CadTypeId",
                table: "CadStoreCategory",
                column: "CadTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CadStoreCategory_CategoryId",
                table: "CadStoreCategory",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CadStoreCategory_StoreDataId",
                table: "CadStoreCategory",
                column: "StoreDataId");

            migrationBuilder.CreateIndex(
                name: "IX_CadStoreCategory_StoreId",
                table: "CadStoreCategory",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_CadStoreCategory_UploadHistoryId",
                table: "CadStoreCategory",
                column: "UploadHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CadStoreSpace_CadTypeId",
                table: "CadStoreSpace",
                column: "CadTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CadStoreSpace_SpaceId",
                table: "CadStoreSpace",
                column: "SpaceId");

            migrationBuilder.CreateIndex(
                name: "IX_CadStoreSpace_StoreDataId",
                table: "CadStoreSpace",
                column: "StoreDataId");

            migrationBuilder.CreateIndex(
                name: "IX_CadStoreSpace_StoreId",
                table: "CadStoreSpace",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_CadStoreSpace_UploadHistoryId",
                table: "CadStoreSpace",
                column: "UploadHistoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CadStoreCategory");

            migrationBuilder.DropTable(
                name: "CadStoreSpace");
        }
    }
}

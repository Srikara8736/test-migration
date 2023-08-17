using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Retail.Data.Migrations
{
    public partial class categorytypegroupnamechange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StoreCategoryAreaType_AreaTypeGroup_AreaTypeGroupId",
                table: "StoreCategoryAreaType");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreCategoryAreaType_Category_CategoryId",
                table: "StoreCategoryAreaType");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreCategoryAreaType_Space_SpaceId",
                table: "StoreCategoryAreaType");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreCategoryAreaType_Store_StoreId",
                table: "StoreCategoryAreaType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StoreCategoryAreaType",
                table: "StoreCategoryAreaType");

            migrationBuilder.RenameTable(
                name: "StoreCategoryAreaType",
                newName: "StoreCategoryAreaTypeGroup");

            migrationBuilder.RenameIndex(
                name: "IX_StoreCategoryAreaType_StoreId",
                table: "StoreCategoryAreaTypeGroup",
                newName: "IX_StoreCategoryAreaTypeGroup_StoreId");

            migrationBuilder.RenameIndex(
                name: "IX_StoreCategoryAreaType_SpaceId",
                table: "StoreCategoryAreaTypeGroup",
                newName: "IX_StoreCategoryAreaTypeGroup_SpaceId");

            migrationBuilder.RenameIndex(
                name: "IX_StoreCategoryAreaType_CategoryId",
                table: "StoreCategoryAreaTypeGroup",
                newName: "IX_StoreCategoryAreaTypeGroup_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_StoreCategoryAreaType_AreaTypeGroupId",
                table: "StoreCategoryAreaTypeGroup",
                newName: "IX_StoreCategoryAreaTypeGroup_AreaTypeGroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StoreCategoryAreaTypeGroup",
                table: "StoreCategoryAreaTypeGroup",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StoreCategoryAreaTypeGroup_AreaTypeGroup_AreaTypeGroupId",
                table: "StoreCategoryAreaTypeGroup",
                column: "AreaTypeGroupId",
                principalTable: "AreaTypeGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreCategoryAreaTypeGroup_Category_CategoryId",
                table: "StoreCategoryAreaTypeGroup",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreCategoryAreaTypeGroup_Space_SpaceId",
                table: "StoreCategoryAreaTypeGroup",
                column: "SpaceId",
                principalTable: "Space",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StoreCategoryAreaTypeGroup_Store_StoreId",
                table: "StoreCategoryAreaTypeGroup",
                column: "StoreId",
                principalTable: "Store",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StoreCategoryAreaTypeGroup_AreaTypeGroup_AreaTypeGroupId",
                table: "StoreCategoryAreaTypeGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreCategoryAreaTypeGroup_Category_CategoryId",
                table: "StoreCategoryAreaTypeGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreCategoryAreaTypeGroup_Space_SpaceId",
                table: "StoreCategoryAreaTypeGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreCategoryAreaTypeGroup_Store_StoreId",
                table: "StoreCategoryAreaTypeGroup");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StoreCategoryAreaTypeGroup",
                table: "StoreCategoryAreaTypeGroup");

            migrationBuilder.RenameTable(
                name: "StoreCategoryAreaTypeGroup",
                newName: "StoreCategoryAreaType");

            migrationBuilder.RenameIndex(
                name: "IX_StoreCategoryAreaTypeGroup_StoreId",
                table: "StoreCategoryAreaType",
                newName: "IX_StoreCategoryAreaType_StoreId");

            migrationBuilder.RenameIndex(
                name: "IX_StoreCategoryAreaTypeGroup_SpaceId",
                table: "StoreCategoryAreaType",
                newName: "IX_StoreCategoryAreaType_SpaceId");

            migrationBuilder.RenameIndex(
                name: "IX_StoreCategoryAreaTypeGroup_CategoryId",
                table: "StoreCategoryAreaType",
                newName: "IX_StoreCategoryAreaType_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_StoreCategoryAreaTypeGroup_AreaTypeGroupId",
                table: "StoreCategoryAreaType",
                newName: "IX_StoreCategoryAreaType_AreaTypeGroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StoreCategoryAreaType",
                table: "StoreCategoryAreaType",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StoreCategoryAreaType_AreaTypeGroup_AreaTypeGroupId",
                table: "StoreCategoryAreaType",
                column: "AreaTypeGroupId",
                principalTable: "AreaTypeGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreCategoryAreaType_Category_CategoryId",
                table: "StoreCategoryAreaType",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreCategoryAreaType_Space_SpaceId",
                table: "StoreCategoryAreaType",
                column: "SpaceId",
                principalTable: "Space",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StoreCategoryAreaType_Store_StoreId",
                table: "StoreCategoryAreaType",
                column: "StoreId",
                principalTable: "Store",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

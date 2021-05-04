namespace DragonSouvenirs.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class FixNameOfCategoryIdInProductCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategories_Categories_CatgoryId",
                table: "ProductCategories");

            migrationBuilder.RenameColumn(
                name: "CatgoryId",
                table: "ProductCategories",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductCategories_CatgoryId",
                table: "ProductCategories",
                newName: "IX_ProductCategories_CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategories_Categories_CategoryId",
                table: "ProductCategories",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategories_Categories_CategoryId",
                table: "ProductCategories");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "ProductCategories",
                newName: "CatgoryId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductCategories_CategoryId",
                table: "ProductCategories",
                newName: "IX_ProductCategories_CatgoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategories_Categories_CatgoryId",
                table: "ProductCategories",
                column: "CatgoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

namespace DragonSouvenirs.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class FixCompositKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CartProducts_IsDeleted",
                table: "CartProducts");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CartProducts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "CartProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CartProducts_IsDeleted",
                table: "CartProducts",
                column: "IsDeleted");
        }
    }
}

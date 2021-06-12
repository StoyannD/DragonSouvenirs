namespace DragonSouvenirs.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddStreetNumberToUserEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StreetNumber",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StreetNumber",
                table: "AspNetUsers");
        }
    }
}
